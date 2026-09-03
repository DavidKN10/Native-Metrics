#pragma once

#include <chrono>
#include <cstdint>
#include <unordered_map>

#include <NativeMetrics/Types.hpp>

constexpr int CHAR_LENGTH_MAX = 260;

struct PreviousNetworkSample {
    u64 receivedBytes{};
    u64 sentBytes{};
    std::chrono::steady_clock::time_point timestamp{};
};

inline std::unordered_map<u64, PreviousNetworkSample> networkHistory{};

struct NetworkAdapterInfo {
    wchar_t alias[CHAR_LENGTH_MAX] {};
    wchar_t description[CHAR_LENGTH_MAX] {};

    u64 luid{};
    u64 type{};

    bool isConnected{};
    bool isOperational{};

    u64 receiveLinkSpeedBits{};
    u64 transmitLinkSpeedBits{};
    u64 receivedBytes{};
    u64 sentBytes{};

    f64 downloadBytesPerSec{};
    f64 uploadBytesPerSec{};
};
