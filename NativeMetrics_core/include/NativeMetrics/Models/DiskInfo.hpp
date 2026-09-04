#pragma once

#include <chrono>
#include <cstdint>
#include <unordered_map>


#include <NativeMetrics/Types.hpp>

constexpr int DRIVE_LETTER_LENGTH = 4; 
constexpr int DRIVE_TYPE_LENGTH = 256;
constexpr int BUFFER_SIZE = 256; 
constexpr int GUID_SIZE = 50;

struct PreviousDiskSample {
    u64 bytesRead{};
    u64 bytesWritten{};
    std::chrono::steady_clock::time_point timestamp{};
};

inline std::unordered_map<std::wstring, PreviousDiskSample> diskMetricHistory{};

struct DiskInfo {
    wchar_t driveLetter[DRIVE_LETTER_LENGTH] {};
    wchar_t driveType[DRIVE_TYPE_LENGTH] {};
    wchar_t volumeName[BUFFER_SIZE] {};
    wchar_t fileSystemName[BUFFER_SIZE] {};
    wchar_t busType[BUFFER_SIZE] {};
    wchar_t vendor[BUFFER_SIZE] {};
    wchar_t model[BUFFER_SIZE] {};
    wchar_t guidPath[GUID_SIZE] {};

    u64 availableSpaceBytes{};
    u64 totalSpaceBytes{};
    u64 freeSpaceBytes{};

    u64 bytesRead{};
    u64 bytesWritten{};
    f64 readSpeed{};
    f64 writeSpeed{};
};