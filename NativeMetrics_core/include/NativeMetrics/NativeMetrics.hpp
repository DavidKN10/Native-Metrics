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
#include <ipifcons.h>
#include <WinSock2.h>
#include <Windows.h>
#include <intsafe.h>
#include <processthreadsapi.h>
#include <TlHelp32.h>
#include <Psapi.h>
#include <iphlpapi.h>
#include <sysinfoapi.h>

// Native Metrics library  
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>
#include <NativeMetrics/Models/CpuInfo.hpp>
#include <NativeMetrics/Models/ProcessInfo.hpp>
#include <NativeMetrics/Models/NetworkAdapterInfo.hpp>
#include <NativeMetrics/Models/MemoryInfo.hpp>

#ifdef NATIVEMETRICS_EXPORTS
#define NATIVEMETRICS_API __declspec(dllexport)
#else 
#define NATIVEMETRICS_API __declspec(dllimport)
#endif

const u32 ONE_SEC = 1000;

// CPU stats
std::wstring getProcessorName();
u32 getLogicalProcessors();
u32 getPhysicalCores();
u32 getBaseSpeedMHz();
f64 getCpuUsage();
void getCpuPerformanceInformation(CpuInfo& cpuInfo);
CpuInfo collectCpuInfo();

// RAM stats
u64 getTotalMemory();
u64 getAvailableMemory();
u64 getApproxPercentInUse();
void getGlobalMemoryStatus(MemoryInfo& memoryInfo);
void getMemoryPerformanceInformation(MemoryInfo& memoryInfo);
MemoryInfo collectMemoryInfo();

// Process stats
std::vector<ProcessInfo> collectProcesses();

// Network stats
std::vector<NetworkAdapterInfo> collectNetworkAdapters();

extern "C" {
	NATIVEMETRICS_API bool getProcessList(ProcessInfo* buffer, i32 bufferSize, i32* processesWritten);
	
	NATIVEMETRICS_API bool getCpuInfo(CpuInfo* buffer, i32 bufferSize);

	NATIVEMETRICS_API bool getMemoryInfo(MemoryInfo* buffer, i32 bufferSize);

	NATIVEMETRICS_API bool getNetworkAdapterInfo(NetworkAdapterInfo* buffer, i32 bufferSize, i32* adaptersWritten);
}
