#include <NativeMetrics/Network.hpp>

bool isDisplayAdapter(const MIB_IF_ROW2& row) {
    if (row.MediaConnectState != MediaConnectStateConnected) {
        return false;
    }

    if (row.OperStatus != IfOperStatusUp) {
        return false;
    }

    switch (row.Type) {
        case IF_TYPE_ETHERNET_CSMACD:
            return true;
        case IF_TYPE_IEEE80211:
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
    if (it == networkHistory.end()) {
        networkHistory[adapter.luid] = {adapter.receivedBytes, adapter.sentBytes, std::chrono::steady_clock::now()};
        adapter.downloadBytesPerSec = 0.0;
        adapter.uploadBytesPerSec = 0.0;
    } else {
        auto& previous = it->second;
        f64 elapsedSeconds = std::chrono::duration<f64>(std::chrono::steady_clock::now() - previous.timestamp).count();
        
        u64 bytesReceivedDelta = adapter.receivedBytes - previous.receivedBytes;
        u64 bytesSentDelta = adapter.sentBytes - previous.sentBytes;

        adapter.downloadBytesPerSec = bytesReceivedDelta / elapsedSeconds;
        adapter.uploadBytesPerSec = bytesSentDelta / elapsedSeconds;

        previous.receivedBytes = adapter.receivedBytes;
        previous.sentBytes = adapter.sentBytes;
        previous.timestamp = std::chrono::steady_clock::now();
    }
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