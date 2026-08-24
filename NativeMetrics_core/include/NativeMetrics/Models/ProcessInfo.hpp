#pragma once

#include <cstdint>
#include <unordered_map>

#include <Windows.h>

#include <NativeMetrics/Types.hpp>

constexpr int PROCESS_NAME_LENGTH = 260;
constexpr int PROCESS_PATH_LENGTH = 1024;

struct ProcessInfo {
	wchar_t processName[PROCESS_NAME_LENGTH] = {};
    wchar_t processPath[PROCESS_PATH_LENGTH] = {};

	u32 processId = 0;
	u32 threadsCount = 0;
	u32 parentProcessId = 0;
	i32 priorityClassBase = 0;
	u32 priorityClass = 0;

	f64 memoryUsage = 0.0;
	f64 commitSize = 0.0;
	f64 privateMemory = 0.0;
};