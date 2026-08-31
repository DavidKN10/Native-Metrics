#include <chrono>
#include <iostream>
#include <thread>

#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/CpuInfo.hpp>

void testPerformanceStats() {
    CpuInfo cpuStats = {};
    i32 bufferSize = sizeof(cpuStats);
    if (getCpuInfo(&cpuStats, bufferSize)) {
        std::wcout << L"Processor name: " << cpuStats.processorName << std::endl;
        std::cout << "Logical processors: " << cpuStats.logicalProcessors << std::endl;
        std::cout << "Cores: " << cpuStats.cores << std::endl;
        std::cout << "Base speed: " << cpuStats.baseSpeed << std::endl;
        std::cout << "CPU usage: " << cpuStats.cpuUsage << "%" << std::endl;
        std::cout << "Handles: " << cpuStats.handles << std::endl;
        std::cout << "Process count: " << cpuStats.processCount << std::endl;
        std::cout << "Thread count: " << cpuStats.threadCount << std::endl << std::endl;
    } else {
        std::cout << "Failed to retrieve CPU info" << std::endl;
    }
}

int main() {
    while (true) {
        testPerformanceStats();
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }
    return 0;
}