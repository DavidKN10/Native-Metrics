#pragma once

#include <cstdint>
#include <chrono>
#include <unordered_map>
#include <NativeMetrics/Types.hpp>

constexpr int CHAR_LENGTH_MAX = 260;

struct PreviousNetworkSample {
    u64 receivedBytes = 0;
    u64 sentBytes = 0;
    std::chrono::steady_clock::time_point timestamp;
};

std::unordered_map<u64, PreviousNetworkSample> networkHistory;

struct NetworkAdapterInfo {
    wchar_t alias[CHAR_LENGTH_MAX] = {};
    wchar_t description[CHAR_LENGTH_MAX] = {};

    u64 luid = 0;
    u64 type = 0;

    bool isConnected = false;
    bool isOperational = false;

    u64 receiveLinkSpeedBits = 0;
    u64 transmitLinkSpeedBits = 0;
    u64 receivedBytes = 0;
    u64 sentBytes = 0;

    f64 downloadBytesPerSec = 0.0;
    f64 uploadBytesPerSec = 0.0;
};
