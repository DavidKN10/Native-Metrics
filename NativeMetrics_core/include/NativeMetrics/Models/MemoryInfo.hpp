#pragma once

#include <NativeMetrics/NativeMetrics.hpp>

struct MemoryInfo {
    u64 pageSizeBytes{};
    u64 totalMemoryBytes{};
    u64 availableMemoryBytes{};
    u64 memoryUsePercent{};
    u64 commitCurrentBytes{};
    u64 commitLimitBytes{};
    u64 commitPeakBytes{};
    u64 pagedPoolBytes{};
    u64 nonPagedPoolBytes{};
};
