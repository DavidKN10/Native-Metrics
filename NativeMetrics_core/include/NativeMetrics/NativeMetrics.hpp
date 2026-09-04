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
#include <fileapi.h>

// Native Metrics library  
#include <NativeMetrics/Cpu.hpp>
#include <NativeMetrics/Disk.hpp>
#include <NativeMetrics/Memory.hpp>
#include <NativeMetrics/Network.hpp>
#include <NativeMetrics/Process.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>
#include <NativeMetrics/Models/CpuInfo.hpp>
#include <NativeMetrics/Models/DiskInfo.hpp>
#include <NativeMetrics/Models/MemoryInfo.hpp>
#include <NativeMetrics/Models/NetworkAdapterInfo.hpp>
#include <NativeMetrics/Models/ProcessInfo.hpp>

#ifdef NATIVEMETRICS_EXPORTS
	#define NATIVEMETRICS_API __declspec(dllexport)
#else 
	#define NATIVEMETRICS_API __declspec(dllimport)
#endif

extern "C" {
	NATIVEMETRICS_API bool getCpuInfo(CpuInfo* buffer, i32 bufferSize);
	
    NATIVEMETRICS_API bool getDiskInfo(DiskInfo* buffer, i32 bufferSize, i32* disksWritten);
	
	NATIVEMETRICS_API bool getMemoryInfo(MemoryInfo* buffer, i32 bufferSize);

	NATIVEMETRICS_API bool getNetworkAdapterInfo(NetworkAdapterInfo* buffer, i32 bufferSize, i32* adaptersWritten);
		
	NATIVEMETRICS_API bool getProcessList(ProcessInfo* buffer, i32 bufferSize, i32* processesWritten);
}
