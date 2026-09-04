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
    HANDLE diskHandle =
        CreateFileW(
            disk.driveLetter, 
            GENERIC_READ, 
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
    u32 bufferSize = sizeof(buffer); 
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

    if (descriptor->VendorIdOffset != 0) {
        const char* vendorId = reinterpret_cast<const char*>(buffer.data() + descriptor->VendorIdOffset);
        std::wstring vendorIdW = AsciiToWide(vendorId); 
        wcsncpy_s(disk.vendor, vendorIdW.c_str(), _TRUNCATE);
    }

    if (descriptor->ProductIdOffset != 0) {
        const char* productId = reinterpret_cast<const char*>(buffer.data() + descriptor->ProductIdOffset);
        std::wstring productIdW = AsciiToWide(productId); 
        wcsncpy_s(disk.model, productIdW.c_str(), _TRUNCATE);
    }
    
    CloseHandle(diskHandle);
    return true;
}

std::vector<DiskInfo> collectDiskInfo() {
    std::vector<DiskInfo> disks{};
    getDriveString(disks);

    for (auto& disk : disks) {
        getDriveType(disk);
        getDriveSpace(disk);
        getVolumeInformation(disk);

        u32 diskNumber = getPhysicalDiskNumber(disk);

        if (!getPhysicalDiskInfo(diskNumber, disk)) {
            continue; 
        }
    }

    return disks;
}