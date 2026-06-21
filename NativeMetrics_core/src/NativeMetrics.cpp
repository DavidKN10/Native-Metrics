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

f64 getCpuUsage() {
	FILETIME idle1, kernel1, user1;
	FILETIME idle2, kernel2, user2;

	// capture initial values
	GetSystemTimes(&idle1, &kernel1, &user1);
	
	Sleep(ONE_SEC);

	// capture second values
	GetSystemTimes(&idle2, &kernel2, &user2);

	u64 idleTime1 = fileTimeToU64(idle1);
	u64 kernelTime1 = fileTimeToU64(kernel1);
	u64 userTime1 = fileTimeToU64(user1);

	u64 idleTime2 = fileTimeToU64(idle2);
	u64 kernelTime2 = fileTimeToU64(kernel2);
	u64 userTime2 = fileTimeToU64(user2);
	
	u64 idleDelta = idleTime2 - idleTime1;
	u64 kernelDelta = kernelTime2 - kernelTime1;
	u64 userDelta = userTime2 - userTime1;

	u64 totalDelta = kernelDelta + userDelta;	// kernel time already contains idle time

	f64 cpuUsage = (1.0 - (static_cast<f64>(idleDelta)) / (static_cast<f64>(totalDelta))) * 100.0;
	return cpuUsage;
}
