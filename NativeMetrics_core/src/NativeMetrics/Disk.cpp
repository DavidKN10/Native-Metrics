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

std::vector<DiskInfo> collectDiskInfo() {
    std::vector<DiskInfo> disks{};
    getDriveString(disks);

    for (auto& disk : disks) {
        getDriveType(disk);
        getDriveSpace(disk);
        getVolumeInformation(disk);
    }

    return disks;
}