#include <chrono>
#include <iostream>
#include <thread>

#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/CpuInfo.hpp>

void testPerformanceStats() {
    CpuInfo cpuStats = collectCpuInfo();
    std::cout << "CPU Usage: " << cpuStats.cpuUsage << "%" << std::endl;
    std::cout << "Cores: " << cpuStats.cores << std::endl;
    std::cout << "Logical Processors: " << cpuStats.logicalProcessors << std::endl << std::endl;
}

int main() {
    while (true) {
        testPerformanceStats();
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }
    return 0;
}