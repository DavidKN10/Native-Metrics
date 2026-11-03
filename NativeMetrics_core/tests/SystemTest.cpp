#include <iostream>

#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/SystemInfo.hpp>

int main() {
    SystemInfo systemInfo{};
    i32 bufferSize = sizeof(systemInfo);
    
    if (getSystemInfo(&systemInfo, bufferSize)) {
        std::wcout << "Device name: " << systemInfo.deviceName << std::endl;
        std::wcout << "System type: " << systemInfo.systemType << std::endl;
        std::wcout << "Windows edition: " << systemInfo.windowsEdition << std::endl;
        std::wcout << "Windows version: " << systemInfo.windowsVersion << std::endl;
        std::wcout << "Windows build number: " << systemInfo.buildNumber << std::endl;
        std::wcout << "Hostname: " << systemInfo.hostname << std::endl;
        std::wcout << "Firmware: " << systemInfo.firmware << std::endl << std::endl;
        
        std::wcout << "Processor: " << systemInfo.processor << std::endl;
        std::wcout << "GPU: " << systemInfo.gpu << std::endl;
        std::wcout << "Installed RAM: " << systemInfo.installedRam << std::endl;
        std::wcout << "Storage capacity: " << systemInfo.storageCapacity << std::endl;
    } else {
        std::cout << "Failed to retrieve system info." << std::endl;
    }

	return 0;
}