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

std::wstring getHostname() {
    wchar_t buffer[WCHAR_LENGTH]{};
    DWORD bufferSize = sizeof(buffer);

    bool result = GetComputerNameExW(ComputerNameDnsHostname, buffer, &bufferSize);
    if (!result) {
        return L""; 
    }

    std::wstring hostname{buffer};
    return hostname;
}

std::wstring getFirmwareType() {
    FIRMWARE_TYPE firmware{};
    
    bool result = GetFirmwareType(&firmware);
    if (!result) {
        return L"Unknown"; 
    }

    switch (firmware) {
        case FirmwareTypeBios:
            return L"BIOS";
        case FirmwareTypeUefi:
            return L"UEFI";
        default:
            return L"Unknown";
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

void getStorageCapcity(SystemInfo& systemInfo) {
    std::vector<DiskInfo> diskList{};

    // Get drive strings.
    wchar_t buffer[512]{};
    u32 length = GetLogicalDriveStringsW(std::size(buffer), buffer);
    if (length != 0) {
        for (wchar_t* drive = buffer; *drive; drive += wcslen(drive) + 1) {
            DiskInfo currentDisk{}; 
            wcsncpy_s(currentDisk.driveLetter, drive, _TRUNCATE);

            diskList.push_back(currentDisk);
        }
    }
    
    for (auto& disk : diskList) {
        getDriveSpace(disk); 
    }
    
    u64 totalStorage{};
    u64 usedStorage{};
    for (auto& disk : diskList) {
        totalStorage += disk.totalSpaceBytes;
        usedStorage += disk.totalSpaceBytes - disk.freeSpaceBytes;
    }

    systemInfo.storageCapacity = totalStorage;
    systemInfo.usedStorage = usedStorage;
}

std::vector<GpuSummary> getGpuAdapterNames() {
    std::vector<GpuSummary> gpuList{};
    
    // Get hardware adapters first.
    IDXCoreAdapterFactory* dxCoreFactory = nullptr;
    if (FAILED(DXCoreCreateAdapterFactory(&dxCoreFactory))) {
        return gpuList;
    }

    // Get graphics adapters that are D3D12 compatible.
    const GUID filterAttributes[]{DXCORE_ADAPTER_ATTRIBUTE_D3D12_GRAPHICS};
    IDXCoreAdapterList* dxCoreAdapterList = nullptr;
    if (FAILED(dxCoreFactory->CreateAdapterList(_countof(filterAttributes), filterAttributes, IID_PPV_ARGS(&dxCoreAdapterList)))) {
        return gpuList;
    }

    // Set preferences so hardware adapters are prioritized.
    DXCoreAdapterPreference preferences[] = {DXCoreAdapterPreference::Hardware};
    dxCoreAdapterList->Sort(_countof(preferences), preferences);

    u32 totalCount = dxCoreAdapterList->GetAdapterCount();
    std::vector<IDXCoreAdapter*> hardwareAdapters{};
    for (u32 i = 0; i < totalCount; ++i) {
        IDXCoreAdapter* adapter = nullptr;

        // Get only hardware adapters.
        if (SUCCEEDED(dxCoreAdapterList->GetAdapter(i, IID_PPV_ARGS(&adapter)))) {
            bool isHardware{};
            if (SUCCEEDED(adapter->GetProperty(DXCoreAdapterProperty::IsHardware, &isHardware))) {
                if (!isHardware) {
                    break;
                }

                hardwareAdapters.push_back(adapter);
            }
        }
    }

    for (size_t i = 0; i < hardwareAdapters.size(); ++i) {
        GpuSummary currentAdapter{};
        size_t descriptionSize{};

        if (SUCCEEDED(hardwareAdapters[i]->GetPropertySize(DXCoreAdapterProperty::DriverDescription, &descriptionSize))) {
            std::vector<char> description(descriptionSize);
            hardwareAdapters[i]->GetProperty(DXCoreAdapterProperty::DriverDescription, descriptionSize, description.data());
            std::string descriptionStr(description.begin(), description.end());

            std::wstring descriptionStrW = convertStringToWstring(descriptionStr);
            wcsncpy_s(currentAdapter.name, descriptionStrW.c_str(), _TRUNCATE);
        }
        gpuList.push_back(currentAdapter);
    }

    dxCoreFactory->Release();
    return gpuList;
}

SystemInfo collectSystemInfo() {
    SystemInfo system{};

    std::wstring deviceName = getDeviceName();
    wcsncpy_s(system.deviceName, deviceName.c_str(), _TRUNCATE);
    
    std::wstring architecture = getProcessorAcrhitecture();
    wcsncpy_s(system.systemType, architecture.c_str(), _TRUNCATE);

    std::wstring hostname = getHostname();
    wcsncpy_s(system.hostname, hostname.c_str(), _TRUNCATE);

    std::wstring firmware = getFirmwareType();
    wcsncpy_s(system.firmware, firmware.c_str(), _TRUNCATE);

    std::wstring processor = getProcessorName();
    wcsncpy_s(system.processor, processor.c_str(), _TRUNCATE);

    getWindowsInfo(system);
    getStorageCapcity(system);

    return system;
}
