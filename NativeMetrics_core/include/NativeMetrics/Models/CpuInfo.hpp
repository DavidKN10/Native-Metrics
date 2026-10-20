#pragma once

#include <NativeMetrics/Types.hpp>

constexpr int PROCESSOR_NAME_LENGTH = 260;

struct PreviousSystemTimeSample {
    u64 idleTime{};
    u64 kernelTime{};
    u64 userTime{};
};

struct CpuInfo {
    wchar_t processorName[PROCESSOR_NAME_LENGTH] {};

    u32 logicalProcessors{};
    u32 cores{};
    u32 baseSpeed{};
    u32 handles{};
    u32 processCount{};
    u32 threadCount{};

    f64 cpuUsage{};
};