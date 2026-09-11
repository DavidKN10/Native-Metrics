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
		for (i32 i = 0; i < adaptersWritten - 1; i++) {
			GpuInfo& gpu = gpuList[i];
			std::cout << "LUID: " << gpu.luid << std::endl;
            std::cout << "Vendor ID: " << gpu.vendorId << std::endl;
			std::wcout << L"Description: " << gpu.description << std::endl;
			std::cout << "Usage: " << gpu.currentUsagePercent << "%" << std::endl;
			std::cout << "Dedicated Memory: " << gpu.localCurrentMemoryBytes / (1024 * 1024 * 1024) << "/" << gpu.dedicatedVideoMemoryBytes / (1024 * 1024 * 1024) << " B" << std::endl;
			std::cout << "Shared memory: " << gpu.nonLocalCurrentMemoryBytes / (1024 * 1024 * 1024) << "/" << gpu.sharedSystemMemoryBytes / (1024 * 1024 * 1024) << " B" << std::endl;
		}
		std::cout << std::endl;
	}
}

int main() {
	while (true) {
		testGpu();        
		std::this_thread::sleep_for(std::chrono::seconds(1));
	}

	return 0;
}
