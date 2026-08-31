#pragma once

#include <NativeMetrics/NativeMetrics.hpp>

struct MemoryInfo {
	u64 totalMemory = 0;
	u64 availableMemory = 0;
	u64 percentInUse = 0;
    u64 commitCurrent = 0;
    u64 commitLimit = 0;
    u64 commitPeak = 0;
    u64 pagedPool = 0;
    u64 nonPagedPool = 0;
};
