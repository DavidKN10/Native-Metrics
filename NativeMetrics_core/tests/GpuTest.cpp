#include <chrono>
#include <iostream>
#include <thread>

#include <NativeMetrics/NativeMetrics.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Models/GpuInfo.hpp>

void testGpu() {
    std::vector<GpuInfo> gpuList(1024);
    i32 adaptersWritten = 0;

    if (getGpuInfo(gpuList.data(), 1024, &adaptersWritten)) {
        for (i32 i = 0; i < adaptersWritten; i++) {
            GpuInfo& gpu = gpuList[i];
            std::cout << "LUID: " << gpu.luid << std::endl;
            std::wcout << L"Description: " << gpu.description << std::endl;
            std::cout << "Video memory: " << gpu.dedicatedVideoMemoryBytes << " B" << std::endl;
            std::cout << "System memory: " << gpu.dedicatedSystemMemoryBytes << " B" << std::endl;
            std::cout << "Shared memory: " << gpu.sharedSystemMemoryBytes << " B" << std::endl;
        }
    }
}

int main() {
    while (true) {
        testGpu();        
        std::this_thread::sleep_for(std::chrono::seconds(1));
    }

	return 0;
}
