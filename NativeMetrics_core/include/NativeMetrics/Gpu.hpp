#pragma once

// C++ standard library
#include <vector>

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

// DXGI
#include <dxgi1_7.h>

// Native Metrics library  
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>
#include <NativeMetrics/Models/GpuInfo.hpp>

void getGraphicsAdapters(std::vector<GpuInfo>& gpuList);

std::vector<GpuInfo> collectGpuInfo();
