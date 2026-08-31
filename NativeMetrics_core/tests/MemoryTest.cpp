#include <chrono>
#include <iostream>
#include <thread>

#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/MemoryInfo.hpp>

void testMemoryStats() {
    MemoryInfo memoryInfo{};
    i32 bufferSize = sizeof(memoryInfo);
    if (getMemoryInfo(&memoryInfo, bufferSize)) {
        std::cout << "Total memory: " << memoryInfo.totalMemory << std::endl;
        std::cout << "Available memory: " << memoryInfo.availableMemory<< std::endl;
        std::cout << "Percent in use: " << memoryInfo.percentInUse << "%" << std::endl;
        std::cout << "Current commit: " << memoryInfo.commitCurrent << std::endl;
        std::cout << "Commit limit: " << memoryInfo.commitLimit << std::endl;
        std::cout << "Commit peak: " << memoryInfo.commitPeak << std::endl;
        std::cout << "Paged pool: " << memoryInfo.pagedPool << std::endl;
        std::cout << "Non-paged pool: " << memoryInfo.nonPagedPool << std::endl << std::endl;
    } else {
        std::cout << "Failed to retrieve memory information." << std::endl;
    }
}

int main() {
    while (true) {
        testMemoryStats();
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }

}