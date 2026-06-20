#pragma once

// standard library headers
#include <iostream>
#include <cstdint>
#include <cstring>
#include <string>
#include <iostream>
#include <vector>
#include <tchar.h>
#include <cwchar>
#include <wchar.h>
#include <stdio.h>

// Windows API headers
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

#ifdef NATIVEMETRICS_EXPORTS
#define NATIVEMETRICS_API __declspec(dllexport)
#else 
#define NATIVEMETRICS_API __declspec(dllimport)
#endif

extern "C" {
	NATIVEMETRICS_API u64 getTotalMemory();
	NATIVEMETRICS_API u64 getAvailableMemory();
	NATIVEMETRICS_API u64 getApproxPercentInUse();
}

