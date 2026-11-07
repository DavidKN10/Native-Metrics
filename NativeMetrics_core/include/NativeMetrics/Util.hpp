#pragma once

#include <stdio.h>
#include <string>
#include <tchar.h>

#include <WS2tcpip.h>
#include <WinSock2.h>
#include <Windows.h>
#include <TlHelp32.h>
#include <ipifcons.h>
#include <netioapi.h>

#include <NativeMetrics/Types.hpp>

#define DIV 1024 // to convert bytes to other units

namespace NativeMetrics {
void printError(TCHAR const* message);

namespace Convert {
u64 bytesToKB(u64 bytesAmount);

u64 bytesToMB(u64 bytesAmount);

u64 bytesToGB(u64 bytesAmount);

u64 fileTimeToU64(const FILETIME& ft);

u64 luidToU64(LUID& luid);

std::wstring AsciiToWide(const char* narrowStr);

std::wstring convertStringToWstring(const std::string& str);
} // namespace NativeMetrics::Convert
} // namespace NativeMetrics
