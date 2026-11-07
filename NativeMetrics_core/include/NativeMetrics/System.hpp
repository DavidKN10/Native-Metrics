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
#include <SetupAPI.h>
#include <TlHelp32.h>
#include <fileapi.h>
#include <intsafe.h>
#include <iphlpapi.h>
#include <ipifcons.h>
#include <processthreadsapi.h>
#include <sysinfoapi.h>
#include <winternl.h>

// Native Metrics library
#include <NativeMetrics/Cpu.hpp>
#include <NativeMetrics/Disk.hpp>
#include <NativeMetrics/Gpu.hpp>
#include <NativeMetrics/Models/SystemInfo.hpp>
#include <NativeMetrics/System.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>

extern "C" NTSYSAPI NTSTATUS NTAPI RtlGetVersion(PRTL_OSVERSIONINFOW lpVersionInformation);

namespace NativeMetrics::System {
std::wstring getDeviceName();

std::wstring getProcessorAcrhitecture();

std::wstring getHostname();

std::wstring getFirmwareType();

void getWindowsInfo(SystemInfo& systemInfo);

void getStorageCapcity(SystemInfo& systemInfo);

u64 getTotalMemory();

std::vector<GpuSummary> getGpuAdapterNames();

SystemInfo collectSystemInfo();
} // namespace NativeMetrics::System
