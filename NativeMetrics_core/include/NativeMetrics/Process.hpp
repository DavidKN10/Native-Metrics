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
#include <Psapi.h>
#include <TlHelp32.h>
#include <fileapi.h>
#include <intsafe.h>
#include <iphlpapi.h>
#include <ipifcons.h>
#include <processthreadsapi.h>
#include <sysinfoapi.h>

// Native Metrics library
#include <NativeMetrics/Models/ProcessInfo.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>

namespace NativeMetrics::Process {
void pew32ToProcessInfo(ProcessInfo& process, PROCESSENTRY32W& pew32);

void getMemory(ProcessInfo& process, PROCESS_MEMORY_COUNTERS_EX& processMemory);

std::vector<ProcessInfo> collectProcesses();
} // namespace NativeMetrics::Process
