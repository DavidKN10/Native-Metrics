#include <NativeMetrics/NativeMetrics.hpp>

u64 getTotalMemory() {
	MEMORYSTATUSEX statex{};
	statex.dwLength = sizeof(statex);
	GlobalMemoryStatusEx(&statex);

	u64 totalPhysicalMemory = bytesToGB(statex.ullTotalPhys);
	
	return totalPhysicalMemory;
}

u64 getAvailableMemory() {
	MEMORYSTATUSEX statex{};
	statex.dwLength = sizeof(statex);
	GlobalMemoryStatusEx(&statex);

	u64 availablePhysicalMemory = bytesToGB(statex.ullAvailPhys);

	return availablePhysicalMemory;
}

u64 getApproxPercentInUse() {
	MEMORYSTATUSEX statex{};
	statex.dwLength = sizeof(statex);
	GlobalMemoryStatusEx(&statex);
	
	u64 approxMemPercent = static_cast<u64>(statex.dwMemoryLoad);

	return approxMemPercent;
}
