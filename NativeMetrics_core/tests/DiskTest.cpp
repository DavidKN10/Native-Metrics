#include <chrono>
#include <iostream>
#include <thread>

#include <NativeMetrics/Models/DiskInfo.hpp>
#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>

void testDisks() {
    std::vector<DiskInfo> diskList(1024);
    i32 disksWritten = 0;

    if (getDiskInfo(diskList.data(), 1024, &disksWritten)) {
        for (i32 i = 0; i < disksWritten; i++) {
            DiskInfo& disk = diskList[i];
            std::wcout << disk.vendor << L" " << disk.model << std::endl;
            std::wcout << L"Bus: " << disk.busType << std::endl;
            std::wcout << L"Type: " << disk.driveType << std::endl;
            std::wcout << L"Label: " << disk.volumeName << std::endl;
            std::wcout << L"Filesystem: " << disk.fileSystemName << std::endl;
            std::wcout << L"Drive: " << disk.driveLetter << std::endl; 
            std::cout << "Available space: " << disk.availableSpaceBytes << " B" << std::endl;
            std::cout << "Total space: " << disk.totalSpaceBytes << " B" << std::endl;
            std::cout << "Free space: " << disk.freeSpaceBytes << " B" << std::endl;
        }
        std::cout << std::endl;
    } else {
        std::cout << "Failed to retrieve disk list" << std::endl;
    }
}

int main() {
    while (true) {
        testDisks();
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }
    return 0;
}
