#include <NativeMetrics/Network.hpp>

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
            wcsncpy_s(currentAdapter.alias, currentRow.Alias, _TRUNCATE);
            wcsncpy_s(currentAdapter.description, currentRow.Description, _TRUNCATE);
            currentAdapter.luid = currentRow.InterfaceLuid.Value;
            currentAdapter.type = currentRow.Type;
            currentAdapter.isConnected = currentRow.MediaConnectState == MediaConnectStateConnected;
            currentAdapter.isOperational = currentRow.OperStatus == IfOperStatusUp;
            currentAdapter.receiveLinkSpeedBits = currentRow.ReceiveLinkSpeed;
            currentAdapter.transmitLinkSpeedBits = currentRow.TransmitLinkSpeed;
            currentAdapter.receivedBytes = currentRow.InOctets;
            currentAdapter.sentBytes = currentRow.OutOctets;

            // updating download and upload speed
            auto it = networkHistory.find(currentAdapter.luid);
            if (it == networkHistory.end()) {
                networkHistory[currentAdapter.luid] = {currentAdapter.receivedBytes, currentAdapter.sentBytes, std::chrono::steady_clock::now()};
                currentAdapter.downloadBytesPerSec = 0.0;
                currentAdapter.uploadBytesPerSec = 0.0;
            } else {
                auto& previous = it->second;
                f64 elapsedSeconds = std::chrono::duration<f64>(std::chrono::steady_clock::now() - previous.timestamp).count();

                u64 bytesReceivedDelta = currentAdapter.receivedBytes - previous.receivedBytes;
                u64 bytesSentDelta = currentAdapter.sentBytes - previous.sentBytes;

                currentAdapter.downloadBytesPerSec = bytesReceivedDelta / elapsedSeconds;
                currentAdapter.uploadBytesPerSec = bytesSentDelta / elapsedSeconds;

                previous.receivedBytes = currentAdapter.receivedBytes;
                previous.sentBytes = currentAdapter.sentBytes;
                previous.timestamp = std::chrono::steady_clock::now();
            }

            networkAdapters.push_back(currentAdapter);
        }
    }
    FreeMibTable(table);
    return networkAdapters;
}