#pragma once

#include <Windows.h>
#include <TlHelp32.h>
#include <tchar.h>
#include <stdio.h>

#include <NativeMetrics/Types.hpp>

#define DIV 1024	// to convert bytes to other units

u64 bytesToMB(u64 bytesAmount) {
	return bytesAmount / (DIV * DIV);
}

u64 bytesToGB(u64 bytesAmount) {
	return bytesAmount / (DIV * DIV * DIV);
}

void printError(TCHAR const* message) {
	u32 eNum;				// Windows error code
	TCHAR sysMessage[256];	// buffer for translated error message
	TCHAR* p;				// use to walk through string and remove unwanted characters

	eNum = GetLastError();
	FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL, eNum, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), sysMessage, 256, NULL);

	p = sysMessage;
	while ((*p > 31) || (*p == 9)) {
		++p;
	}

	do {
		*p-- = 0;
	} while (
		(p >= sysMessage) &&
		((*p == '.') || (*p < 33))
		);

	_tprintf(TEXT("\n WARNING: %s failed with error %d (%s)\n"), message, eNum, sysMessage);
}