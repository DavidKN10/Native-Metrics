#pragma once

#include <cstdint>

#include <NativeMetrics/Types.hpp>

constexpr int PROCESS_NAME_MAX = 260;

struct ProcessInfo {
	wchar_t processName[PROCESS_NAME_MAX] = {};

	u32 processId = 0;
	u32 threadsCount = 0;
	u32 parentProcessId = 0;
	i32 priorityClassBase = 0;
	u32 priorityClass = 0;

	f64 memoryUsage = 0.0;
	f64 commitSize = 0.0;
	f64 privateMemory = 0.0;
};