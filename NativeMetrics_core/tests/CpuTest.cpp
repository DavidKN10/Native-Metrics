#include <chrono>
#include <iostream>
#include <thread>
#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>

void testPerformanceStats() {
    u64 totalMemory = getTotalMemory();
    u64 availableMemory = getAvailableMemory();
    u64 percentInUse = getApproxPercentInUse();
    f64 cpuUsage = getCpuUsage();
    std::cout << "Total memory: " << totalMemory << " GB" << std::endl;
    std::cout << "Available memory: " << availableMemory << " GB" << std::endl;
    std::cout << "Percent of memory in use: " << percentInUse << " %" << std::endl;
    std::cout << "CPU Usage: " << cpuUsage << "%" << std::endl << std::endl;
}

int main() {
    while (true) {
        testPerformanceStats();
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }
    return 0;
}