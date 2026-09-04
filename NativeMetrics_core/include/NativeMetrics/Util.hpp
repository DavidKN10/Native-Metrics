#pragma once

#include <tchar.h>
#include <stdio.h>
#include <string>

#include <WS2tcpip.h>
#include <ipifcons.h>
#include <netioapi.h>
#include <WinSock2.h>
#include <Windows.h>
#include <TlHelp32.h>

#include <NativeMetrics/Types.hpp>

#define DIV 1024	// to convert bytes to other units

void printError(TCHAR const* message); 

u64 bytesToKB(u64 bytesAmount);

u64 bytesToMB(u64 bytesAmount);

u64 bytesToGB(u64 bytesAmount);

u64 fileTimeToU64(const FILETIME& ft);

std::wstring AsciiToWide(const char* narrowStr);
