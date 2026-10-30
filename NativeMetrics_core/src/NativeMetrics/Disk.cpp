#include <NativeMetrics/Disk.hpp>

void getDriveString(std::vector<DiskInfo>& diskList) {
    wchar_t buffer[512] = {};

    u32 length = GetLogicalDriveStringsW(std::size(buffer), buffer);
    if (length != 0) {
        for (wchar_t* drive = buffer; *drive; drive += wcslen(drive) + 1) {
            DiskInfo currentDisk{};
            wcsncpy_s(currentDisk.driveLetter, drive, _TRUNCATE);

            diskList.push_back(currentDisk);
        }
    }
}

void getDriveGuid(DiskInfo& disk) {
    wchar_t buffer[50];

    BOOL result = GetVolumeNameForVolumeMountPointW(
        disk.driveLetter,
        buffer, 
        ARRAYSIZE(buffer) 
    );

    if (result) {
        wcsncpy_s(disk.guidPath, buffer, _TRUNCATE);
    }
}

void getDriveType(DiskInfo& disk) {
    u32 type = GetDriveTypeW(disk.driveLetter);

    switch (type) {
        case DRIVE_RAMDISK:
            wcsncpy_s(disk.driveType, L"RAM disk", _TRUNCATE);
            break;
        case DRIVE_CDROM:
            wcsncpy_s(disk.driveType, L"CD-ROM", _TRUNCATE);
            break;
        case DRIVE_REMOTE:
            wcsncpy_s(disk.driveType, L"Remote drive", _TRUNCATE);
            break;
        case DRIVE_FIXED:
            wcsncpy_s(disk.driveType, L"Fixed drive", _TRUNCATE);
            break;
        case DRIVE_REMOVABLE:
            wcsncpy_s(disk.driveType, L"Removable drive", _TRUNCATE);
            break;
        default:
            wcsncpy_s(disk.driveType, L"UNKNOWN", _TRUNCATE);
            break;
    }
}

void getDriveSpace(DiskInfo& disk) {
    ULARGE_INTEGER availableBytes{};
    ULARGE_INTEGER totalBytes{};
    ULARGE_INTEGER freeBytes{};

    if (GetDiskFreeSpaceExW(disk.driveLetter, &availableBytes, &totalBytes, &freeBytes)) {
        disk.availableSpaceBytes = availableBytes.QuadPart;
        disk.totalSpaceBytes = totalBytes.QuadPart;
        disk.freeSpaceBytes = freeBytes.QuadPart;
    }
}

void getVolumeInformation(DiskInfo& disk) {
    wchar_t volumeNameBuffer[256] = {};
    u32 volumeNameSize = std::size(volumeNameBuffer);

    wchar_t fileSystemBuffer[256] = {};
    u32 fileSystemNameSize = std::size(fileSystemBuffer);

    if (GetVolumeInformationW(disk.driveLetter, volumeNameBuffer, volumeNameSize, nullptr, nullptr, nullptr, fileSystemBuffer, fileSystemNameSize)) {
        wcsncpy_s(disk.volumeName, volumeNameBuffer, _TRUNCATE);
        wcsncpy_s(disk.fileSystemName, fileSystemBuffer, _TRUNCATE);
    }
}

u32 getPhysicalDiskNumber(DiskInfo& disk) {
    // convert from "C:\" to "\\.\C:" for CreateFileW
    std::wstring devicePath = L"\\\\.\\";
    devicePath += disk.driveLetter[0];
    devicePath += L":";

    HANDLE diskHandle =
        CreateFileW(
            devicePath.c_str(), 
            0, 
            FILE_SHARE_READ | FILE_SHARE_WRITE, 
            nullptr, 
            OPEN_EXISTING, 
            FILE_ATTRIBUTE_NORMAL, 
            nullptr
        );

    if (diskHandle == INVALID_HANDLE_VALUE) {
        return 0; 
    }

    // get physical disk
    std::vector<BYTE> buffer(1024);
    u32 bufferSize = static_cast<u32>(buffer.size()); 
    DWORD bytesReturned = 0;

    BOOL result = DeviceIoControl(
        diskHandle, 
        IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, 
        nullptr, 0, 
        buffer.data(), 
        bufferSize, 
        &bytesReturned, 
        nullptr
    );
    
    CloseHandle(diskHandle);

    if (!result) {
        return 0; 
    }

    VOLUME_DISK_EXTENTS* extents = reinterpret_cast<VOLUME_DISK_EXTENTS*>(buffer.data());
    
    if (extents->NumberOfDiskExtents > 0) {
        return extents->Extents[0].DiskNumber;
    }

    return 0;
}

void getBusType(DiskInfo& disk, STORAGE_BUS_TYPE& type) {
    switch (type) { 
        case BusTypeSata:
            wcsncpy_s(disk.busType, L"SATA", _TRUNCATE);
            break;
        case BusTypeNvme:
            wcsncpy_s(disk.busType, L"NVMe", _TRUNCATE);
            break;
        case BusTypeUsb:
            wcsncpy_s(disk.busType, L"USB", _TRUNCATE);
            break;
        case BusTypeScsi:
            wcsncpy_s(disk.busType, L"SCSI", _TRUNCATE);
            break;
        case BusTypeRAID:
            wcsncpy_s(disk.busType, L"RAID", _TRUNCATE);
            break;
        case BusTypeVirtual:
            wcsncpy_s(disk.busType, L"Virtual", _TRUNCATE);
            break;
        default:
            wcsncpy_s(disk.busType, L"Unknown", _TRUNCATE);
            break;
    }
}

bool getPhysicalDiskInfo(u32 diskNumber, DiskInfo& disk) {
    std::wstring diskName = L"\\\\.\\PhysicalDrive";
    diskName += std::to_wstring(diskNumber);

    HANDLE diskHandle = CreateFileW(
        diskName.c_str(), 
        0, 
        FILE_SHARE_READ | FILE_SHARE_WRITE, 
        nullptr, 
        OPEN_EXISTING, 
        0, 
        nullptr
    );

    if (diskHandle == INVALID_HANDLE_VALUE) {
        return false; 
    }

    STORAGE_PROPERTY_QUERY query{};
    query.PropertyId = StorageDeviceProperty;
    query.QueryType = PropertyStandardQuery;
    u32 bufferSize = sizeof(query);
    
    STORAGE_DESCRIPTOR_HEADER header{};
    u32 headerSize = sizeof(header);
    DWORD bytesReturned = 0;

    // first call: determine buffer size required
    BOOL result = DeviceIoControl(
        diskHandle, 
        IOCTL_STORAGE_QUERY_PROPERTY, 
        &query, bufferSize, 
        &header, headerSize, 
        &bytesReturned, 
        nullptr
    );


    if (!result) {
        CloseHandle(diskHandle);
        return false;
    }

    std::vector<BYTE> buffer(header.Size);

    // second call: retrieve properties
    result = DeviceIoControl(
        diskHandle, 
        IOCTL_STORAGE_QUERY_PROPERTY, 
        &query, sizeof(query),
        buffer.data(), static_cast<DWORD>(buffer.size()),
        &bytesReturned,
        nullptr
    );

    if (!result) {
        CloseHandle(diskHandle);
        return false;
    }
    
    STORAGE_DEVICE_DESCRIPTOR* descriptor = reinterpret_cast<STORAGE_DEVICE_DESCRIPTOR*>(buffer.data());
    getBusType(disk, descriptor->BusType); 

    if (descriptor->ProductIdOffset != 0) {
        const char* productId = reinterpret_cast<const char*>(buffer.data() + descriptor->ProductIdOffset);
        std::wstring productIdW = AsciiToWide(productId); 
        wcsncpy_s(disk.model, productIdW.c_str(), _TRUNCATE);
    }
    
    CloseHandle(diskHandle);
    return true;
}

void updateSpeedAndTime(DiskInfo& disk, DISK_PERFORMANCE& diskPerformance) {
    auto it = diskMetricHistory.find(std::wstring(disk.guidPath));
    auto now = std::chrono::steady_clock::now();

    // Adding a new disk.
    if (it == diskMetricHistory.end()) {
        diskMetricHistory[std::wstring(disk.guidPath)] = {
            disk.bytesRead,
            disk.bytesWritten, 
            static_cast<u64>(diskPerformance.ReadTime.QuadPart),
            static_cast<u64>(diskPerformance.WriteTime.QuadPart),
            static_cast<u64>(diskPerformance.IdleTime.QuadPart),
            static_cast<u64>(diskPerformance.QueryTime.QuadPart),
            now
        };

        disk.readSpeed = 0.0;
        disk.writeSpeed = 0.0;
        disk.activeTimePercent = 0.0;
        return;
    } 
    
    auto& previous = it->second;
    f64 elapsedSeconds = std::chrono::duration<f64>(std::chrono::steady_clock::now() - previous.timestamp).count();
    
    if (elapsedSeconds > 0.0 && disk.bytesRead >= previous.bytesRead) {
        // update read speed
        f64 readBytesDelta = static_cast<f64>(disk.bytesRead - previous.bytesRead);
        disk.readSpeed = readBytesDelta / elapsedSeconds;
    } else {
        disk.readSpeed = 0.0; 
    }

    if (elapsedSeconds > 0.0 && disk.bytesWritten >= previous.bytesWritten) {
        // update write speed
        f64 writeBytesDelta = static_cast<f64>(disk.bytesWritten - previous.bytesWritten);
        disk.writeSpeed = writeBytesDelta / elapsedSeconds;
    } else {
        disk.writeSpeed = 0.0;
    }

    // update transfer rate
    f64 bytesTransferred = static_cast<f64>((disk.bytesRead - previous.bytesRead) + (disk.bytesWritten - previous.bytesWritten));
    disk.transferRate = bytesTransferred / elapsedSeconds;

    // update active time
    u64 currentReadTime = static_cast<u64>(diskPerformance.ReadTime.QuadPart);
    u64 currentWriteTime = static_cast<u64>(diskPerformance.WriteTime.QuadPart);
    u64 currentIdleTime = static_cast<u64>(diskPerformance.IdleTime.QuadPart);
    u64 currentQueryTime = static_cast<u64>(diskPerformance.QueryTime.QuadPart);

    if (currentQueryTime > previous.queryTime && currentIdleTime >= previous.idleTime) {
        u64 elapsed = currentQueryTime - previous.queryTime;
        u64 idle = currentIdleTime - previous.idleTime;

        // clamp the value to 0-100%
        disk.activeTimePercent = std::clamp(100.0 * (1.0 - static_cast<f64>(idle) / static_cast<f64>(elapsed)), 0.0, 100.0);
    } else {
        disk.activeTimePercent = 0.0;
    }

    // Turn this sample into the previous sample so we can use it in the next sample.
    previous = {
        disk.bytesRead, disk.bytesWritten, 
        currentReadTime, currentWriteTime, currentIdleTime, currentQueryTime, 
        now
    };
}

bool getDiskPerformance(u32 diskNumber, DiskInfo& disk) {
    std::wstring diskName = L"\\\\.\\PhysicalDrive";
    diskName += std::to_wstring(diskNumber);

    HANDLE diskHandle = CreateFileW(
        diskName.c_str(), 0, 
        FILE_SHARE_READ | FILE_SHARE_WRITE, 
        nullptr, 
        OPEN_EXISTING, 
        0, nullptr
    );

    if (diskHandle == INVALID_HANDLE_VALUE) {
        return false;
    }
    
    DISK_PERFORMANCE buffer{};
    u32 bufferSize = sizeof(buffer);
    DWORD bytesReturned = 0;
    
    BOOL result = DeviceIoControl(
        diskHandle, 
        IOCTL_DISK_PERFORMANCE, 
        nullptr, 0, 
        &buffer, bufferSize, 
        &bytesReturned,
        nullptr 
    );

    if (!result) {
        CloseHandle(diskHandle);
        return false;
    }
    
    disk.bytesRead = buffer.BytesRead.QuadPart;
    disk.bytesWritten = buffer.BytesWritten.QuadPart;

    updateSpeedAndTime(disk, buffer);
    
    CloseHandle(diskHandle);
    return true;
}

std::vector<DiskInfo> collectDiskInfo() {
    std::vector<DiskInfo> disks{};
    getDriveString(disks);

    for (auto& disk : disks) {
        getDriveGuid(disk);
        getDriveType(disk);
        getDriveSpace(disk);
        getVolumeInformation(disk);

        u32 diskNumber = getPhysicalDiskNumber(disk);
        
        if (!getPhysicalDiskInfo(diskNumber, disk)) {
            continue; 
        }

        if (!getDiskPerformance(diskNumber, disk)) {
            continue;
        }
    }

    return disks;
}