#include <NativeMetrics/NativeMetrics.hpp>

bool getCpuInfo(CpuInfo* buffer, i32 bufferSize) {
    if (!buffer || bufferSize <= 0) {
        return false;
    }

    CpuInfo cpuInfo = collectCpuInfo();
    *buffer = cpuInfo;

    return true;
}

bool getMemoryInfo(MemoryInfo* buffer, i32 bufferSize) {
    if (!buffer || bufferSize <= 0) {
        return false;
    }

    MemoryInfo memoryInfo = collectMemoryInfo();
    *buffer = memoryInfo;
    
    return true;
}

bool getDiskInfo(DiskInfo* buffer, i32 bufferSize, i32* disksWritten) {
    if (!buffer || !disksWritten || bufferSize <= 0) {
        return false;
    }

    auto disks = collectDiskInfo();
    i32 count = static_cast<i32>(disks.size());
    i32 toCopy = std::min<i32>(count, bufferSize);

    for (int i = 0; i < toCopy; i++) {
        buffer[i] = disks[i];
    }
    *disksWritten = toCopy;
    return true;
}

bool getProcessList(ProcessInfo *buffer, i32 bufferSize, i32* processesWritten) {
	if (!buffer || !processesWritten || bufferSize <= 0) {
		return false;
	}

	auto processes = collectProcesses();

	i32 count = static_cast<i32>(processes.size());
	i32 toCopy = std::min<i32>(count, bufferSize);

	for (int i = 0; i < toCopy; i++) {
		buffer[i] = processes[i];
	}
	*processesWritten = toCopy;
	return true;
}

bool getNetworkAdapterInfo(NetworkAdapterInfo* buffer, i32 bufferSize, i32* adaptersWritten) {
    if (!buffer || !adaptersWritten || bufferSize <= 0) {
        return false;
    }

    auto networkAdapters = collectNetworkAdapters();

    i32 count = static_cast<i32>(networkAdapters.size());
    i32 toCopy = std::min<i32>(count, bufferSize);

    for (int i = 0; i < toCopy; i++) {
        buffer[i] = networkAdapters[i];
    }
    *adaptersWritten = toCopy;
    return true;
}
