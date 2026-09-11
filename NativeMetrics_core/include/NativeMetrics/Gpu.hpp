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

// DirectX
#include <d3d12.h>

// Native Metrics library  
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>
#include <NativeMetrics/Models/GpuInfo.hpp>

// Note: I am using DXGI for GPU metrics but it has limitations. I will implement vendor specific 
// libraries for GPU metrics in the future.
void getAdapterDesc(GpuInfo& adapter, DXGI_ADAPTER_DESC2& desc);

void udpateDedicatedMemoryUsage(GpuInfo& adapter, DXGI_QUERY_VIDEO_MEMORY_INFO& localMemoryInfo);

void updateSharedMemoryUsage(GpuInfo& adapter, DXGI_QUERY_VIDEO_MEMORY_INFO& nonLocalMemoryInfo);

void getGraphicsAdapters(std::vector<GpuInfo>& gpuList);

std::vector<GpuInfo> collectGpuInfo();
