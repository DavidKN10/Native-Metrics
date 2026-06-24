#pragma once

#include <cstdint>
#include <cstring>
#include <string>
#include <iostream>
#include <vector>
#include <tchar.h>
#include <cwchar>
#include <wchar.h>
#include <stdio.h>

#include <WS2tcpip.h>
#include <WinSock2.h>
#include <Windows.h>
#include <intsafe.h>
#include <processthreadsapi.h>
#include <TlHelp32.h>
#include <Psapi.h>
#include <iphlpapi.h>

// Native Metrics headers
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>
#include <NativeMetrics/Models/ProcessInfo.hpp>
#include <NativeMetrics/Models/NetworkAdapterInfo.hpp>

#ifdef NATIVEMETRICS_EXPORTS
#define NATIVEMETRICS_API __declspec(dllexport)
#else 
#define NATIVEMETRICS_API __declspec(dllimport)
#endif

const u32 ONE_SEC = 1000;

std::vector<ProcessInfo> collectProcesses();

extern "C" {
	// RAM stats
	NATIVEMETRICS_API u64 getTotalMemory();
	NATIVEMETRICS_API u64 getAvailableMemory();
	NATIVEMETRICS_API u64 getApproxPercentInUse();

	// CPU stats
	NATIVEMETRICS_API f64 getCpuUsage();

	// Process information
	NATIVEMETRICS_API bool getProcessList(ProcessInfo* buffer, i32 bufferSize, i32* processesWritten);

	// Network stats
    NATIVEMETRICS_API bool getNetworkAdapterInfo(NetworkAdapterInfo* buffer, i32 bufferSize, i32* adaptersWritten);
}
