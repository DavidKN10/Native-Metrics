#pragma once

#include <NativeMetrics/NativeMetrics.hpp>

struct MemoryInfo {
	u64 totalMemory = 0;
	u64 availableMemory = 0;
	u64 percentInUse = 0;
};
