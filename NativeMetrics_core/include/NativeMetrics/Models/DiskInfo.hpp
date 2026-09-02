#pragma once

#include <cstdint>

#include <NativeMetrics/Types.hpp>

constexpr int DRIVE_LETTER_LENGTH = 4; 
constexpr int DRIVE_TYPE_LENGTH = 256;

struct DiskInfo {
    wchar_t driveLetter[DRIVE_LETTER_LENGTH] = {};
    wchar_t driveType[DRIVE_TYPE_LENGTH] = {};
};