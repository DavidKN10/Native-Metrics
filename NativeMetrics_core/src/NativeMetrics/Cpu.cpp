#include <NativeMetrics/Cpu.hpp>

std::wstring getProcessorName() {
    /*
        We are getting the processor name by reading from the registry.
    */

    std::wstring processorName = L"Unkown";
    HKEY key;

    // open the registry key for the first logical processor
    LSTATUS openKeyResult = RegOpenKeyExW(HKEY_LOCAL_MACHINE, L"HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", 0, KEY_READ, &key);
    if (openKeyResult == ERROR_SUCCESS) {
        wchar_t buffer[256];
        DWORD bufferSize = sizeof(buffer);

        // query the ProcessorNameString value
        LSTATUS queryValueResult = RegQueryValueExW(key, L"ProcessorNameString", nullptr, nullptr, reinterpret_cast<LPBYTE>(buffer), &bufferSize);
        if (queryValueResult == ERROR_SUCCESS) {
            processorName = buffer;
        }
        RegCloseKey(key);
    }
    return processorName;
}

u32 getLogicalProcessors() {
    SYSTEM_INFO si{};
    GetSystemInfo(&si);
    return si.dwNumberOfProcessors;
}

u32 getPhysicalCores() {
    // first call to determine required buffer size
    DWORD bufferSize = 0;
    GetLogicalProcessorInformationEx(RelationProcessorCore, nullptr, &bufferSize);

    if (GetLastError() != ERROR_INSUFFICIENT_BUFFER) {
        return 0;
    }

    // second call to retrieve data filtered by RelationProcessorCore
    std::vector<BYTE> buffer(bufferSize);
    if (!GetLogicalProcessorInformationEx(RelationProcessorCore, reinterpret_cast<PSYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX>(buffer.data()),
                                          &bufferSize)) {
        return 0;
    }

    u32 physicalCores = 0;
    DWORD offset = 0;

    // loop through variable-length structs
    while (offset < bufferSize) {
        auto info = reinterpret_cast<PSYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX>(&buffer[offset]);

        if (info->Relationship == RelationProcessorCore) {
            physicalCores++;
        }

        // move pointer forward by the size of the current struct
        offset += info->Size;
    }

    return physicalCores;
}

u32 getBaseSpeedMHz() {
    u32 baseSpeedMHz = 0;
    HKEY key;

    LSTATUS openKeyResult = RegOpenKeyExW(HKEY_LOCAL_MACHINE, L"HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", 0, KEY_READ, &key);
    if (openKeyResult == ERROR_SUCCESS) {
        DWORD buffer = 0;
        DWORD bufferSize = sizeof(LPBYTE);
        DWORD dataType = REG_DWORD;

        LSTATUS queryValueResult = RegQueryValueExW(key, L"~MHz", nullptr, &dataType, reinterpret_cast<LPBYTE>(&buffer), &bufferSize);
        if (queryValueResult == ERROR_SUCCESS) {
            baseSpeedMHz = buffer;

            RegCloseKey(key);
            return baseSpeedMHz;
        }
    }
    return baseSpeedMHz;
}

f64 getCpuUsage() {
    FILETIME idle1, kernel1, user1;
    FILETIME idle2, kernel2, user2;

    // capture initial values
    GetSystemTimes(&idle1, &kernel1, &user1);

    Sleep(ONE_SEC);

    // capture second values
    GetSystemTimes(&idle2, &kernel2, &user2);

    u64 idleTime1 = fileTimeToU64(idle1);
    u64 kernelTime1 = fileTimeToU64(kernel1);
    u64 userTime1 = fileTimeToU64(user1);

    u64 idleTime2 = fileTimeToU64(idle2);
    u64 kernelTime2 = fileTimeToU64(kernel2);
    u64 userTime2 = fileTimeToU64(user2);

    u64 idleDelta = idleTime2 - idleTime1;
    u64 kernelDelta = kernelTime2 - kernelTime1;
    u64 userDelta = userTime2 - userTime1;

    u64 totalDelta = kernelDelta + userDelta; // kernel time already contains idle time

    f64 cpuUsage = (1.0 - (static_cast<f64>(idleDelta)) / (static_cast<f64>(totalDelta))) * 100.0;
    return cpuUsage;
}

void getCpuPerformanceInformation(CpuInfo& cpuInfo) {
    PERFORMANCE_INFORMATION buffer{};
    u32 bufferSize = sizeof(buffer);
    if (GetPerformanceInfo(&buffer, bufferSize)) {
        cpuInfo.handles = buffer.HandleCount;
        cpuInfo.processCount = buffer.ProcessCount;
        cpuInfo.threadCount = buffer.ThreadCount;
    }
}

CpuInfo collectCpuInfo() {
    CpuInfo cpuInfo{};

    std::wstring processorName = getProcessorName();

    wcsncpy_s(cpuInfo.processorName, processorName.c_str(), _TRUNCATE);

    cpuInfo.cpuUsage = getCpuUsage();

    cpuInfo.logicalProcessors = getLogicalProcessors();

    cpuInfo.cores = getPhysicalCores();

    cpuInfo.baseSpeed = getBaseSpeedMHz();

    getCpuPerformanceInformation(cpuInfo);

    return cpuInfo;
}