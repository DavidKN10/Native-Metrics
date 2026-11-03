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

SystemInfo collectSystemInfo() {
    SystemInfo system{};

    std::wstring deviceName = getDeviceName();
    wcsncpy_s(system.deviceName, deviceName.c_str(), _TRUNCATE);

    return system;
}
