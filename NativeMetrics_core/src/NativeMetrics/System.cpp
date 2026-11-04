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

void getWindowsInfo(SystemInfo& systemInfo) {
    RTL_OSVERSIONINFOW version{};
    version.dwOSVersionInfoSize = sizeof(version);

    const NTSTATUS status = ::RtlGetVersion(&version);
    if (status != 0) {
        return;
    }

    systemInfo.buildNumber = version.dwBuildNumber;
    u64 majorVersion = version.dwMajorVersion;
    u64 minorVersion = version.dwMinorVersion; 
    
    // get windows edition
    if (majorVersion == 10 && minorVersion == 0) {
        if (version.dwBuildNumber >= 22000) {
            std::wstring edition = L"Windows 11 " + windowsEdition();
            wcsncpy_s(systemInfo.windowsEdition, edition.c_str(), _TRUNCATE); 
        } else {
            std::wstring edition = L"Windows 10 " + windowsEdition();
            wcsncpy_s(systemInfo.windowsEdition, edition.c_str(), _TRUNCATE);
        }
    }

    // map windows build number to release 
    auto it = WindowsReleases.find(systemInfo.buildNumber);
    if (it != WindowsReleases.end()) {
        wcsncpy_s(systemInfo.windowsVersion, it->second.c_str(), _TRUNCATE); 
    } else {
        wcsncpy_s(systemInfo.windowsVersion, L"Unkown", _TRUNCATE);
    }
}

SystemInfo collectSystemInfo() {
    SystemInfo system{};

    std::wstring deviceName = getDeviceName();
    wcsncpy_s(system.deviceName, deviceName.c_str(), _TRUNCATE);
    
    std::wstring architecture = getProcessorAcrhitecture();
    wcsncpy_s(system.systemType, architecture.c_str(), _TRUNCATE);

    getWindowsInfo(system);

    return system;
}
