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
#include <NativeMetrics/Models/NetworkAdapterInfo.hpp>
#include <NativeMetrics/Types.hpp>
#include <NativeMetrics/Util.hpp>

namespace NativeMetrics::Network {
bool isDisplayAdapter(const MIB_IF_ROW2& row);

void rowToAdapter(NetworkAdapterInfo& adapter, const MIB_IF_ROW2& row);

void updateSpeed(NetworkAdapterInfo& adapter);

std::vector<NetworkAdapterInfo> collectNetworkAdapters();
} // namespace NativeMetrics::Network
