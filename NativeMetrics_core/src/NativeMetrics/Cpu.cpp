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
    FILETIME idle, kernel, user;
    GetSystemTimes(&idle, &kernel, &user);

    u64 currentIdleTime = fileTimeToU64(idle);
    u64 currentKernelTime = fileTimeToU64(kernel);
    u64 currentUserTime = fileTimeToU64(user);

    u64 idleDelta = currentIdleTime - previousTime.idleTime;
    u64 kernelDelta = currentKernelTime - previousTime.kernelTime;
    u64 userDelta = currentUserTime - previousTime.userTime;

    u64 totalDelta = kernelDelta + userDelta;   // kernel time already contains idle time

    f64 cpuUsage = (1.0 - (static_cast<f64>(idleDelta)) / (static_cast<f64>(totalDelta))) * 100.0;
    
    previousTime.idleTime = currentIdleTime;
    previousTime.kernelTime = currentKernelTime;
    previousTime.userTime = currentUserTime;
    
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