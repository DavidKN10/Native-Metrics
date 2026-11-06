#pragma once

// C++ standard library
#include <algorithm>
#include <chrono>
#include <cstdint>
#include <cstring>
#include <cwchar>
#include <iostream>
#include <set>
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
#include <NativeMetrics/Models/DiskInfo.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>

namespace NativeMetrics::Disk {
inline std::unordered_map<std::wstring, NativeMetrics::Disk::PreviousDiskSample> diskMetricHistory{};

void getDriveString(std::vector<DiskInfo>& diskList);

void getDriveGuid(DiskInfo& disk);

void getDriveType(DiskInfo& disk);

void getDriveSpace(DiskInfo& disk);

void getVolumeInformation(DiskInfo& disk);

u32 getPhysicalDiskNumber(DiskInfo& disk);

void getBusType(DiskInfo& disk, STORAGE_BUS_TYPE& type);

bool getPhysicalDiskInfo(u32 diskNumber, DiskInfo& disk);

void updateSpeedAndTime(DiskInfo& disk, DISK_PERFORMANCE& diskPerformance);

bool getPerformance(u32 diskNumber, DiskInfo& disk);

std::vector<DiskInfo> collectInfo();
} // namespace NativeMetrics::Disk
