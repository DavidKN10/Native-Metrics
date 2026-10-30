#include <NativeMetrics/Network.hpp>

bool isDisplayAdapter(const MIB_IF_ROW2& row) {
    if (row.MediaConnectState != MediaConnectStateConnected) {
        return false;
    }

    if (row.OperStatus != IfOperStatusUp) {
        return false;
    }

    if (row.InterfaceAndOperStatusFlags.FilterInterface) {
        return false;
    }

    if (row.InterfaceAndOperStatusFlags.EndPointInterface) {
        return false;
    }
    
    if (!row.InterfaceAndOperStatusFlags.HardwareInterface) {
        return false; 
    }

    if (!row.InterfaceAndOperStatusFlags.ConnectorPresent) {
        return false;
    }

    switch (row.Type) {
        case IF_TYPE_ETHERNET_CSMACD:
            return true;
        case IF_TYPE_IEEE80211:
            return true;
        case IF_TYPE_WWANPP:
            return true;
        case IF_TYPE_WWANPP2:
            return true;
        default:
            return false;
    }
}

void rowToNetworkAdapter(NetworkAdapterInfo& adapter, const MIB_IF_ROW2& row) {
    wcsncpy_s(adapter.alias, row.Alias, _TRUNCATE);
    wcsncpy_s(adapter.description, row.Description, _TRUNCATE);

    adapter.luid = row.InterfaceLuid.Value;
    adapter.type = row.Type;

    adapter.isConnected = row.MediaConnectState == NdisMediaStateConnected;
    adapter.isOperational = row.OperStatus == IfOperStatusUp;

    adapter.receiveLinkSpeedBits = row.ReceiveLinkSpeed;
    adapter.transmitLinkSpeedBits = row.TransmitLinkSpeed;

    adapter.receivedBytes = row.InOctets;
    adapter.sentBytes = row.OutOctets;
}

void updateDownloadAndNetworkSpeed(NetworkAdapterInfo& adapter) {
    auto it = networkHistory.find(adapter.luid);
    auto now = std::chrono::steady_clock::now();

    // adding a new addapter
    if (it == networkHistory.end()) {
        networkHistory[adapter.luid] = {
            adapter.receivedBytes,
            adapter.sentBytes,
            std::chrono::steady_clock::now()
        };

        adapter.downloadBytesPerSec = 0.0;
        adapter.uploadBytesPerSec = 0.0;
        return;
    }

    auto& previous = it->second;
    f64 elapsedSeconds = std::chrono::duration<f64>(std::chrono::steady_clock::now() - previous.timestamp).count();
   
    // update download speed
    if (elapsedSeconds > 0.0 && adapter.receivedBytes >= previous.receivedBytes) {
        u64 bytesReceivedDelta = adapter.receivedBytes - previous.receivedBytes;
        adapter.downloadBytesPerSec = bytesReceivedDelta / elapsedSeconds;
    } else {
        adapter.receivedBytes = 0;
    }
   
    // update upload speed
    if (elapsedSeconds > 0.0 && adapter.sentBytes > previous.sentBytes) {
        u64 bytesSentDelta = adapter.sentBytes - previous.sentBytes;
        adapter.uploadBytesPerSec = bytesSentDelta / elapsedSeconds; 
    } else {
        adapter.sentBytes = 0;
    }
  
    adapter.throughput = adapter.downloadBytesPerSec + adapter.uploadBytesPerSec;
   
    // Turn this sample into the previous sample so we can use it in the next sample.
    previous = {
        adapter.receivedBytes,
        adapter.sentBytes, 
        now
    };
}

std::vector<NetworkAdapterInfo> collectNetworkAdapters() {
    std::vector<NetworkAdapterInfo> networkAdapters{};

    PMIB_IF_TABLE2 table{};
    i32 returnValue = GetIfTable2(&table);

    if (returnValue != NO_ERROR || table == nullptr) {
        printError(TEXT("GetIfTable2"));
        return networkAdapters;
    } else {
        for (u64 i = 0; i < table->NumEntries; i++) {
            const MIB_IF_ROW2& currentRow = table->Table[i];

            if (!isDisplayAdapter(currentRow)) {
                continue;
            }

            NetworkAdapterInfo currentAdapter{};
            rowToNetworkAdapter(currentAdapter, currentRow);

            updateDownloadAndNetworkSpeed(currentAdapter);
            
            networkAdapters.push_back(currentAdapter);
        }
    }
    FreeMibTable(table);
    return networkAdapters;
}