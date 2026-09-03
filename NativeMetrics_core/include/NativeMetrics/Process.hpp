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
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>
#include <NativeMetrics/Models/ProcessInfo.hpp>

void pew32ToProcessInfo(ProcessInfo& process, PROCESSENTRY32W& pew32);

void getProcessMemory(ProcessInfo& process, PROCESS_MEMORY_COUNTERS_EX& processMemory);

std::vector<ProcessInfo> collectProcesses();