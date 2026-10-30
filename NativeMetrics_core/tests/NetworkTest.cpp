#include <chrono>
#include <iostream>
#include <thread>

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
            std::cout << "Download Speed: " << currentNetwork.downloadBytesPerSec << " B/s" << std::endl;
            std::cout << "Upload Speed: " << currentNetwork.uploadBytesPerSec << " B/s" << std::endl;
            std::cout << "Download link speed: " << currentNetwork.receiveLinkSpeedBits << " b/s" << std::endl;
            std::cout << "Upload link speed: " << currentNetwork.transmitLinkSpeedBits << " b/s" << std::endl;
            std::cout << "Throughput: " << currentNetwork.throughput << " B/s" << std::endl << std::endl;
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