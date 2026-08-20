#include <chrono>
#include <iostream>
#include <thread>
#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/ProcessInfo.hpp>

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
    while (true) {
        testProcesses();
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }
    return 0;
}