#pragma once

#include <NativeMetrics/Types.hpp>

constexpr int PROCESSOR_NAME_LENGTH = 260;

struct CpuInfo {
    wchar_t processorName[PROCESSOR_NAME_LENGTH] = {};
    u32 logicalProcessors = 0;
    u32 cores = 0;
    u32 baseSpeed = 0;

    f64 cpuUsage = 0.0f;
};