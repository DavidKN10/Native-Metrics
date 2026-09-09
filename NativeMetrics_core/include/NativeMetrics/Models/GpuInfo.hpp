#pragma once

#include <cstdint>

#include <NativeMetrics/Types.hpp>

constexpr int DESCRIPTION_SIZE = 128;

struct GpuInfo {
    wchar_t description[DESCRIPTION_SIZE] {};
    
    u64 luid{};
    u64 dedicatedVideoMemoryBytes{};
    u64 dedicatedSystemMemoryBytes{};
    u64 sharedSystemMemoryBytes{};

};