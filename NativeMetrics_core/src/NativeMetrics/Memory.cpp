#include <NativeMetrics/Memory.hpp>

void getGlobalMemoryStatus(MemoryInfo& memoryInfo) {
    MEMORYSTATUSEX statex{};
    statex.dwLength = sizeof(statex);
    GlobalMemoryStatusEx(&statex);

    memoryInfo.totalMemoryBytes = statex.ullTotalPhys;
    memoryInfo.availableMemoryBytes = statex.ullAvailPhys;
    memoryInfo.memoryUsePercent = static_cast<u64>(statex.dwMemoryLoad);
}

void getMemoryPerformanceInformation(MemoryInfo& memoryInfo) {
    PERFORMANCE_INFORMATION buffer{};
    u32 bufferSize = sizeof(buffer);
    if (GetPerformanceInfo(&buffer, bufferSize)) {
        memoryInfo.pageSizeBytes = static_cast<u64>(buffer.PageSize);
        memoryInfo.commitCurrentBytes = static_cast<u64>(buffer.CommitTotal) * memoryInfo.pageSizeBytes;
        memoryInfo.commitLimitBytes = static_cast<u64>(buffer.CommitLimit) * memoryInfo.pageSizeBytes;
        memoryInfo.commitPeakBytes = static_cast<u64>(buffer.CommitPeak) * memoryInfo.pageSizeBytes;
        memoryInfo.pagedPoolBytes = static_cast<u64>(buffer.KernelPaged) * memoryInfo.pageSizeBytes;
        memoryInfo.nonPagedPoolBytes = static_cast<u64>(buffer.KernelNonpaged) * memoryInfo.pageSizeBytes;
    }
}

MemoryInfo collectMemoryInfo() {
    MemoryInfo memoryInfo{};

    getGlobalMemoryStatus(memoryInfo);

    getMemoryPerformanceInformation(memoryInfo);

    return memoryInfo;
}