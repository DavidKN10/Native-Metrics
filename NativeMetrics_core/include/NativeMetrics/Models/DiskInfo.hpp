#pragma once

#include <cstdint>

#include <NativeMetrics/Types.hpp>

constexpr int DRIVE_LETTER_LENGTH = 4; 
constexpr int DRIVE_TYPE_LENGTH = 256;
constexpr int BUFFER_SIZE = 256; 

struct DiskInfo {
    wchar_t driveLetter[DRIVE_LETTER_LENGTH] {};
    wchar_t driveType[DRIVE_TYPE_LENGTH] {};
    wchar_t volumeName[BUFFER_SIZE] {};
    wchar_t fileSystemName[BUFFER_SIZE] {};
    wchar_t busType[BUFFER_SIZE] {};
    wchar_t vendor[BUFFER_SIZE] {};
    wchar_t model[BUFFER_SIZE] {};

    u64 availableSpaceBytes{};
    u64 totalSpaceBytes{};
    u64 freeSpaceBytes{};
};