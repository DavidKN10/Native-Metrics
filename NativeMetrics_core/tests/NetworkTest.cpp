#include <thread>
#include <chrono>
#include <iostream>
#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/NetworkAdapterInfo.hpp>

void testNetworkStats() {
    std::vector<NetworkAdapterInfo> networkInfo(200);
    i32 adaptersWritten = 0;

    if (getNetworkAdapterInfo(networkInfo.data(), 200, &adaptersWritten)) {
        for (i32 i = 0; i < adaptersWritten; i++) {
            NetworkAdapterInfo& currentNetwork = networkInfo[i];
            std::wcout << L"Alias: " << currentNetwork.alias << std::endl;
            std::wcout << L"LUID: " << currentNetwork.luid << std::endl;
            std::wcout << L"Type: " << currentNetwork.type << std::endl;
            std::wcout << L"Description: " << currentNetwork.description << std::endl;
            std::wcout << L"Download Speed: " << currentNetwork.downloadBytesPerSec << L" B/s" << std::endl;
            std::wcout << L"Upload Speed: " << currentNetwork.uploadBytesPerSec << L" B/s" << std::endl << std::endl;
        }
    }
    std::cout << std::endl;
}

int main() {
    while (true) {
        testNetworkStats();
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }
    return 0;
}