#pragma once

// C++ standard library
#include <algorithm>
#include <cstdint>
#include <cstring>
#include <cwchar>
#include <iostream>
#include <stdio.h>
#include <string>
#include <tchar.h>
#include <vector>
#include <wchar.h>

// Windows API
#include <WS2tcpip.h>
#include <WinSock2.h>
#include <Windows.h>
#include <TlHelp32.h>
#include <Psapi.h>
#include <fileapi.h>
#include <intsafe.h>
#include <iphlpapi.h>
#include <ipifcons.h>
#include <processthreadsapi.h>
#include <sysinfoapi.h>

// Native Metrics library
#include <NativeMetrics/Models/CpuInfo.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>

const u32 ONE_SEC = 1000;

namespace NativeMetrics::CPU {
inline PreviousSystemTimeSample previousTime{};

std::wstring getProcessorName();

u32 getLogicalProcessors();
u32 getPhysicalCores();
u32 getBaseSpeedMHz();

f64 getUsage();

void getPerformanceInformation(CpuInfo& cpuInfo);

CpuInfo collectInfo();
} // namespace NativeMetrics::CPU
