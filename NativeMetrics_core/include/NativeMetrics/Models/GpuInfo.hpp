#pragma once

#include <cstdint>

#include <NativeMetrics/Types.hpp>

constexpr int DESCRIPTION_SIZE = 128;

struct GpuInfo {
    wchar_t description[DESCRIPTION_SIZE] {};
    
    u64 luid{};
    u32 vendorId{};

    f64 dedicatedVideoMemoryBytes{};
    f64 localCurrentMemoryBytes{};
    f64 currentUsagePercent{};

    f64 sharedSystemMemoryBytes{};
    f64 nonLocalCurrentMemoryBytes{};

};