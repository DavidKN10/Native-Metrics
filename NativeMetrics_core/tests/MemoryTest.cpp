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
        std::cout << "Total memory: " << memoryInfo.totalMemoryBytes << " B" << std::endl;
        std::cout << "Available memory: " << memoryInfo.availableMemoryBytes<< " B" << std::endl;
        std::cout << "Percent in use: " << memoryInfo.memoryUsePercent << "%" << std::endl;
        std::cout << "Current commit: " << memoryInfo.commitCurrentBytes << " B" << std::endl;
        std::cout << "Commit limit: " << memoryInfo.commitLimitBytes << " B" << std::endl;
        std::cout << "Commit peak: " << memoryInfo.commitPeakBytes << " B" << std::endl;
        std::cout << "Paged pool: " << memoryInfo.pagedPoolBytes << " B" << std::endl;
        std::cout << "Non-paged pool: " << memoryInfo.nonPagedPoolBytes << " B" << std::endl << std::endl;
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