#pragma once

#include <unordered_map>
#include <NativeMetrics/Types.hpp>

constexpr int WCHAR_LENGTH = 256;
constexpr int GPU_COUNT_MAX = 8;

inline std::unordered_map<u64, std::wstring> WindowsReleases = {
    // Windows 11 
    {26300, L"26H2"}, {26200, L"25H2"}, {26100, L"24H2"}, {22631, L"23H2"},
    {22621, L"22H2"}, {22000, L"21H2"}, 
    
    // Windows 10
    {19045, L"22H2"}, {19044, L"21H2"}, {19043, L"21H1"}, {19042, L"20H2"}, 
    {19041, L"2004"}, {18363, L"1909"}, {18362, L"1903"}, {17763, L"1809"}
};

struct GpuSummary {
    wchar_t name[WCHAR_LENGTH]{};
};

struct SystemInfo {
    // system 
    wchar_t deviceName[WCHAR_LENGTH]{};
    wchar_t systemType[WCHAR_LENGTH]{};
    wchar_t windowsVersion[WCHAR_LENGTH]{};
    wchar_t windowsEdition[WCHAR_LENGTH]{};
    wchar_t hostname[WCHAR_LENGTH]{};
    wchar_t firmware[WCHAR_LENGTH]{};

    // hardware
    wchar_t processor[WCHAR_LENGTH]{};

    u64 buildNumber{};
    u64 installedRam{};
    u64 storageCapacity{};
};
