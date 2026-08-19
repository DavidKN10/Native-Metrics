#include <iostream>
#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/ProcessInfo.hpp>

void testPerformanceStats() {
    u64 totalMemory = getTotalMemory();
    u64 availableMemory = getAvailableMemory();
    u64 percentInUse = getApproxPercentInUse();
    f64 cpuUsage = getCpuUsage();
    std::cout << "Total memory: " << totalMemory << " GB" << std::endl;
    std::cout << "Available memory: " << availableMemory << " GB" << std::endl;
    std::cout << "Percent of memory in use: " << percentInUse << " %" << std::endl;
    std::cout << "CPU Usage: " << cpuUsage << " GB" << std::endl;
}

void testProcesses() {
    std::vector<ProcessInfo> processList(1024);
    i32 processesWritten = 0;

    if (getProcessList(processList.data(), 1024, &processesWritten)) {
        for (i32 i = 0; i < processesWritten; i++) {
            ProcessInfo& process = processList[i]; 
            std::wcout << L"Process: " << process.processName << std::endl;
            std::wcout << L"PID: " << process.processId << std::endl;
            std::wcout << L"Threads: " << process.threadsCount << std::endl;
            std::wcout << L"Memory Usage (MB): " << process.memoryUsage << std::endl;
            std::cout << std::endl;
        } 
    } else {
        std::cout << "Failed to retrieve process list" << std::endl;
    }
}

int main() {
    
    testPerformanceStats();
    testProcesses();

    return 0;
}
