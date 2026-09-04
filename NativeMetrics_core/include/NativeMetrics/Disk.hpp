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
#include <NativeMetrics/Models/DiskInfo.hpp>

void getDriveString(std::vector<DiskInfo>& diskList);

void getDriveType(DiskInfo& disk);

void getDriveSpace(DiskInfo& disk);

void getVolumeInformation(DiskInfo& disk);

u32 getPhysicalDiskNumber(DiskInfo& disk);

void getBusType(DiskInfo& disk, STORAGE_BUS_TYPE& type);

bool getPhysicalDiskInfo(u32 diskNumber, DiskInfo& disk);

std::vector<DiskInfo> collectDiskInfo();