#include <NativeMetrics/System.hpp>

std::wstring getDeviceName() {
    wchar_t buffer[WCHAR_LENGTH]{}; 
    DWORD bufferSize = sizeof(buffer);

    bool result = GetComputerNameExW(ComputerNamePhysicalDnsHostname, buffer, &bufferSize);
    if (!result) {
        return L""; 
    }
    
    std::wstring name{buffer};
    return name;
}

std::wstring getProcessorAcrhitecture() {
    SYSTEM_INFO si{};
    GetNativeSystemInfo(&si);
    WORD architecture = si.wProcessorArchitecture;

    switch (architecture) {
        case PROCESSOR_ARCHITECTURE_INTEL: 
            return L"x86";
        case PROCESSOR_ARCHITECTURE_ARM: 
            return L"ARM";
        case PROCESSOR_ARCHITECTURE_IA64: 
            return L"Intel Itanium-based";
        case PROCESSOR_ARCHITECTURE_AMD64: 
            return L"x86-64";
        case PROCESSOR_ARCHITECTURE_ARM64: 
            return L"ARM64";
        default:
            return L"Uknown";
    }
}

SystemInfo collectSystemInfo() {
    SystemInfo system{};

    std::wstring deviceName = getDeviceName();
    wcsncpy_s(system.deviceName, deviceName.c_str(), _TRUNCATE);
    
    std::wstring architecture = getProcessorAcrhitecture();
    wcsncpy_s(system.systemType, architecture.c_str(), _TRUNCATE);

    return system;
}
