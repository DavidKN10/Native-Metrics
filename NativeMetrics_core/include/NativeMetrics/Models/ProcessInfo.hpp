#pragma once

#include <cstdint>
#include <unordered_map>

#include <Windows.h>

#include <NativeMetrics/Types.hpp>

constexpr int PROCESS_NAME_LENGTH = 260;
constexpr int PROCESS_PATH_LENGTH = 1024;

struct ProcessInfo {
	wchar_t processName[PROCESS_NAME_LENGTH] {};
    wchar_t processPath[PROCESS_PATH_LENGTH] {};

	u32 processId{};
    u32 threadsCount{};
    u32 parentProcessId{};
    i32 priorityClassBase{};
    u32 priorityClass{};

	f64 memoryUsage{};
    f64 commitSize{};
    f64 privateMemory{};
};