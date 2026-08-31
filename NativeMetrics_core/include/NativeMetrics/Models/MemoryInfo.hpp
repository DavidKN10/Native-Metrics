#pragma once

#include <NativeMetrics/NativeMetrics.hpp>

struct MemoryInfo {
    u64 pageSizeBytes = 0;
	u64 totalMemoryBytes = 0;
	u64 availableMemoryBytes = 0;
	u64 memoryUsePercent = 0;
    u64 commitCurrentBytes = 0;
    u64 commitLimitBytes = 0;
    u64 commitPeakBytes = 0;
    u64 pagedPoolBytes = 0;
    u64 nonPagedPoolBytes = 0;
};
