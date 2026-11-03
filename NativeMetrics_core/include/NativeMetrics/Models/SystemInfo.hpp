#pragma once

#include <NativeMetrics/Types.hpp>

constexpr int WCHAR_LENGTH = 256;

struct SystemInfo {
    // system 
    wchar_t deviceName[WCHAR_LENGTH]{};
    wchar_t systemType[WCHAR_LENGTH]{};
    wchar_t windowsEdition[WCHAR_LENGTH]{};
    wchar_t windowsVersion[WCHAR_LENGTH]{};
    wchar_t buildNumber[WCHAR_LENGTH]{};
    wchar_t hostname[WCHAR_LENGTH]{};
    wchar_t firmware[WCHAR_LENGTH]{};

    // hardware
    wchar_t processor[WCHAR_LENGTH]{};
    wchar_t gpu[WCHAR_LENGTH]{};
    u64 installedRam{};
    u64 storageCapacity{};
};
