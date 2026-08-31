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
        std::cout << "Page size: " << memoryInfo.pageSizeBytes << " Bytes" << std::endl;
        std::cout << "Total memory: " << memoryInfo.totalMemoryBytes << std::endl;
        std::cout << "Available memory: " << memoryInfo.availableMemoryBytes<< std::endl;
        std::cout << "Percent in use: " << memoryInfo.memoryUsePercent << "%" << std::endl;
        std::cout << "Current commit: " << memoryInfo.commitCurrentBytes << std::endl;
        std::cout << "Commit limit: " << memoryInfo.commitLimitBytes << std::endl;
        std::cout << "Commit peak: " << memoryInfo.commitPeakBytes << std::endl;
        std::cout << "Paged pool: " << memoryInfo.pagedPoolBytes << std::endl;
        std::cout << "Non-paged pool: " << memoryInfo.nonPagedPoolBytes << std::endl << std::endl;
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