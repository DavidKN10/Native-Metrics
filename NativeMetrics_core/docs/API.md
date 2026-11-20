# Backend API

# Index
- [Backend API](#backend-api)
- [Index](#index)
- [Models](#models)
  - [CpuInfo](#cpuinfo)
    - [Description](#description)
    - [Syntax](#syntax)
    - [Members](#members)
  - [DiskInfo](#diskinfo)
    - [Description](#description-1)
    - [Syntax](#syntax-1)
    - [Members](#members-1)
  - [GpuInfo](#gpuinfo)
    - [Description](#description-2)
    - [Syntax](#syntax-2)
    - [Members](#members-2)
  - [MemoryInfo](#memoryinfo)
    - [Description](#description-3)
    - [Syntax](#syntax-3)
    - [Members](#members-3)
  - [NetworkAdapterInfo](#networkadapterinfo)
    - [Description](#description-4)
    - [Syntax](#syntax-4)
    - [Members](#members-4)
  - [ProcessInfo](#processinfo)
    - [Description](#description-5)
    - [Syntax](#syntax-5)
    - [Members](#members-5)
  - [SystemInfo](#systeminfo)
    - [Description](#description-6)
    - [Syntax](#syntax-6)
    - [Members](#members-6)
  - [GpuSummary](#gpusummary)
    - [Description](#description-7)
    - [Syntax](#syntax-7)
    - [Members](#members-7)
- [Functions](#functions)
  - [getCpuInfo](#getcpuinfo)
    - [Description](#description-8)
    - [Syntax](#syntax-8)
    - [Parameters](#parameters)
    - [Returns](#returns)
  - [getDiskInfo](#getdiskinfo)
    - [Description](#description-9)
    - [Syntax](#syntax-9)
    - [Parameters](#parameters-1)
    - [Returns](#returns-1)
  - [getGpuInfo](#getgpuinfo)
    - [Description](#description-10)
    - [Syntax](#syntax-10)
    - [Parameters](#parameters-2)
    - [Returns](#returns-2)
  - [getMemoryInfo](#getmemoryinfo)
    - [Description](#description-11)
    - [Syntax](#syntax-11)
    - [Parameters](#parameters-3)
    - [Returns](#returns-3)
  - [getNetworkAdapterInfo](#getnetworkadapterinfo)
    - [Description](#description-12)
    - [Syntax](#syntax-12)
    - [Parameters](#parameters-4)
    - [Returns](#returns-4)
  - [getProcessList](#getprocesslist)
    - [Description](#description-13)
    - [Syntax](#syntax-13)
    - [Parameters](#parameters-5)
    - [Returns](#returns-5)
  - [getSystemInfo](#getsysteminfo)
    - [Description](#description-14)
    - [Syntax](#syntax-14)
    - [Parameters](#parameters-6)
    - [Returns](#returns-6)
  - [getGpuSummary](#getgpusummary)
    - [Description](#description-15)
    - [Syntax](#syntax-15)
    - [Parameters](#parameters-7)
    - [Returns](#returns-7)

# Models

## CpuInfo
### Description
- Contains processor information.

### Syntax
```cpp
struct CpuInfo {
  wchar_t processorName[260];
  u32     logicalProcessors;
  u32     cores;
  u32     baseSpeed;
  u32     handles;
  u32     processCount;
  u32     threadCount;
  f64     cpuUsage;
}
```

### Members
`processorName`
- wchar string that contains the processor name.

`logicalProcessors`
- Contains number of logical processors

`cores`
- Contains number of cores in a system.

`baseSpeed`
- Contains the CPU's base speed.

`handles`
- Contains the number of handles. 

`processCount`
- Contains the total amount of processes currently running.

`threadCount`
- Contains the total number of current threads.

`cpuUsage`
- Contains the amount of CPU that is being utilized as a percentage. Uses the average usage across all processors.

## DiskInfo
### Description
- Contains information for a single drive in the system.
 
### Syntax
```cpp
struct DiskInfo {
  wchar_t driveLetter[256];
  wchar_t driveType[256];
  wchar_t volumeName[256];
  wchar_t fileSystemName[256];
  wchar_t busType[256];
  wchar_t model[256];
  wchar_t guidPath[50];
  u64     totalSpaceBytes;
  u64     availableSpaceBytes;
  u64     freeSpaceBytes;
  u64     bytesRead;
  u64     bytesWritten;
  f64     readSpeed;
  f64     writeSpeed;
  f64     activeTimePercent;
  f64     transferRate;
}
```

### Members
`driveLetter`
- Contains the drive letter.

`driveType`
- Contains the type of drive.
- These are the possible values for `driveType`:
  - `RAM disk`
  - `CD-ROM`
  - `Remote drive`
  - `Fixed Drive`
  - `Removable drive`
  - `UNKNOWN`

`volumeName`
- Contains the name of the drive.

`fileSystemName`
- Contains the file system type.

`busType`
- Contains the type of bus.

`model`
- Contains the name of the model of the physical disk that the drive belongs to.

`guidPath`
- Contains the file path for the drive's GUID. Stored as `\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\`

`totalSpaceBytes`
- Contains the drive's total space capacity in bytes.

`availableSpaceBytes`
- Contains the drive's available space in bytes.

`freeSpaceBytes`
- Contains the drive's remaining space in bytes. This is different from `availableSpaceBytes` because there might be more free space available, but the drive does not have permission to use all of it.

`bytesRead`
- Contains how many bytes were read since last function call.

`bytesWritten`
- Contains how many bytes were written since last function call.

`readSpeed`
- Contains the drive's read speed in bytes/sec.

`writeSpeed`
- Contains the drive's write speed in bytes/sec.

`activeTimePercent`
- Contains the percent of time that the drive was busy writing or reading since last function call.

`transferRate`
- Contains the rate at which bytes are being transferred, either read or written, since last function call. Stored in bytes/sec.

## GpuInfo
### Description
- Contains information about an individual graphics card in the system. 

### Syntax
```cpp
struct GpuInfo{
    wchar_t description[128];
    u64     luid;
    u32     vendorId;
    f64     dedicatedVideoMemoryBytes;
    f64     localCurrentMemoryBytes;
    f64     currentUsagePercent;
    f64     sharedSystemMemoryBytes;
    f64     nonLocalCurrentMemoryBytes;
}
```

### Members
`description`
- Contains the graphics card's name.

`luid`
- Contains the graphics card's LUID.

`vendorId`
- Contains the graphics card's vendor id.

`dedicatedVideoMemoryBytes`
- Contains the graphics card's dedicated video memory in bytes.

`localCurrentMemoryBytes`
- Contains the grpahics card's current local memory in bytes.

`currentUsagePercent`
- Contains the percent of the GPU that is being used.

`sharedSystemMemoryBytes`
- Contains the total amount of shared system memory in bytes.

`nonLocalCurrentMemoryBytes`
- Contains the graphics card's current non-local memory in bytes.

## MemoryInfo
### Description
- Contains the system's memory information.

### Syntax
```cpp
struct MemoryInfo {
  u64 pageSizeBytes;
  u64 totalMemoryBytes;
  u64 availableMemoryBytes;
  u64 memoryUsePercent;
  u64 commitCurrentBytes;
  u64 commitLimitBytes;
  u64 commitPeakBytes;
  u64 pagedPoolBytes;
  u64 nonPagedPoolBytes;
}
```

### Members
`pageSizeBytes`
- Contains the page size in bytes.

`totalMemoryBytes`
- Contains system's total memory in bytes.

`availableMemoryBytes`
- Contains the system's remaining memory in bytes.

`memoryUsePercent`
- Contains the percent of memory that is being used.

`commitCurrentBytes`
- Contains the current commit size in bytes.

`commitLimitBytes`
- Contains the commit size limit in bytes.

`commitPeakBytes`
- Contains the highest commit size in bytes.

`pagedPoolBytes`
- Contains the amount of paged pool memory in bytes.

`nonPagedPoolBytes`
- Contains the amount of non-paged pool memory in bytes.

## NetworkAdapterInfo
### Description
- Stores infomration about a single physical network adapter in the system.

### Syntax
```cpp
struct NetworkAdapterInfo{
  wchar_t alias[260];
  wchar_t description[260];
  u64     luid;
  u64     type;
  u64     receiveLinkSpeedBits;
  u64     transmitLinkSpeedBits;
  u64     receivedBytes;
  u64     sentBytes;
  f64     downloadBytesPerSec;
  f64     uploadBytesPerSec;
  f64     throughput;
  bool    isConnected;
  bool    isOperational;
}

```

### Members
`alias`
- Contains the alias name for the network interface.

`description`
- Contains the description of the network interface.

`luid`
- Contains the network interface's LUID.

`type`
- Contains the network interface's type of network.

`receiveLinkSpeedBits`
- Contains the link speed for receiving bits.

`transmitLinkSpeedBits`
- Contains the link speed for transmitting bits.

`receiveBytes`
- Contains the amount of bytes received since last function call.

`sentBytes`
- Contains the amount of bytes sent since last function call.

`downloadBytesPerSec`
- Contains the download speed in bytes/sec.

`uploadBytesPerSec`
- Contains the upload speed in bytes/sec.

`throughput`
- Contains the network interface's throughput in bytes/sec.

`isConnected`
- Shows whether or not the interface is connected to the network.

`isOperational`
- Shows whether or not the interface is up and able to pass packets.

## ProcessInfo
### Description
- Stores information about a single process in the system.

### Syntax
```cpp
struct ProcessInfo{
  wchar_t processName[260];
  wchar_t processPath[1024];
  f64     memoryUsage;
  f64     commitSize;
  f64     privateMemory;
  u32     processId;
  u32     threadsCount;
  u32     parentProcessId;
  i32     priorityClassBase;
  u32     priorityClass;
}
```

### Members
`processName`
- Contains the process name.

`processPath`
- Contains path were the process is being run.

`memoryUsage`
- Contains how much memory the process is using in bytes.

`commitSize`
- Contains the process's commit size in bytes.

`privateMemory`
- Contains the process's private memory in bytes.

`processId`
- Contains the process's ID.

`threadsCount`
- Contains the process's number of threads.

`parentProcessId`
- Contains the process ID of the parent process.

`priorityClassBase`
- Contains the process's base priority.

`priorityClass`
- Contains the process's priority.

## SystemInfo
### Description
- Provides an overview of system specs.

### Syntax
```cpp
struct SystemInfo {
  wchar_t deviceName[256];
  wchar_t systemType[256];
  wchar_t windowsVersion[256];
  wchar_t windowsEdition[256];
  wchar_t hostname[256];
  wchar_t firmware[256];
  wchar_t processor[256];
  u64     buildNumber;
  u64     installedRam;
  u64     storageCapacity;
  u64     usedStorage;
}
```

### Members
`deviceName`
- Contains the name of the device.

`systemType`
- Contains the system architecture.

`windowsVersion`
- Contains Windows version.

`windowsEdition`
- Contains Windows edition.

`hostname`
- Contains network hostname.

`firmware`
- Contains firmware type.

`processor`
- Contains name of processor.

`buildNumber`
- Contains Windows build number.

`installedRam`
- Contains total amount of RAM installed in bytes.

`storageCapacity`
- Contains the total amount of storage capacity across all drives in bytes.

`usedStorage`
- Contains the total amound of storage used across all drives in bytes.

## GpuSummary
### Description
- Contains the name of a single graphics card in the system.

### Syntax
```cpp
struct GpuSummary {
  wchar_t name[256];
}
```

### Members
`name`
- Contains the graphics card's name.

# Functions
## getCpuInfo
### Description
- Retrieves processor information.

### Syntax
```cpp
bool getCpuInfo(
 [out]  CpuInfo* buffer, 
 [in]   i32 bufferSize
);
```

### Parameters

`buffer`
- A pointer to a CpuInfo struct that will receive the processor information.

`bufferSize`
- The size of the CpuInfo struct.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.

## getDiskInfo
### Description
- Retrieves information of all disks in a system.

### Syntax
```cpp
bool getDiskInfo(
  [out] DiskInfo* buffer,
  [in]  i32 bufferSize,
  [out] i32* disksWritten
);
```

### Parameters

`buffer`
- A pointer to an array of DiskInfo structs. Each item in the array will represent one disk.

`bufferSize`
- The size of the array of DiskInfo structs.

`disksWritten`
- The number of disks that were added to the array.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.

## getGpuInfo
**Note**: I used DXGI and DXCore to retrieve GPU information which does not return accurate GPU usage metrics. I'll maybe try to make an implementation that retrieves more accurate information based on the GPU a user has (Ex. Using an Nvidia library for Nvidia GPUs).  

### Description
- Retrieves information of all graphics cards in the system.

### Syntax
```cpp
bool getGpuInfo(
  [out] GpuInfo* buffer,
  [in]  i32 bufferSize,
  [out] i32 adaptersWritten
);
```

### Parameters
`buffer`
- A pointer to an array of GpuInfo structs. Each item in the array will represent one GPU in the system.

`bufferSize`
- The size of the array of GpuInfo structs.

`adaptersWritten`
- The number of GPUs that were added to the array.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.

## getMemoryInfo
### Description
- Retrieves memory information.

### Syntax
```cpp
bool getMemoryInfo(
  [out] MemoryInfo* buffer,
  [in]  i32 bufferSize,
);
```

### Parameters
`buffer`
- A pointer to a MemoryInfo struct that received memory information. 

`bufferSize`
- The size of the MemoryInfo structs.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.

## getNetworkAdapterInfo
### Description
- Retrieves information of all physical network adapters in the system.

### Syntax
```cpp
bool getNetworkAdapterInfo(
  [out] NetworkAdapterInfo* buffer,
  [in]  i32 bufferSize,
  [out] i32 adaptersWritten
);
```

### Parameters
`buffer`
- A pointer to an array of NetworkAdapterInfo structs. Each item in the array will represent one physical adapter in the system.

`bufferSize`
- The size of the array of NetworkAdapterInfo structs.

`adaptersWritten`
- The number of physical adapters that were added to the array.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.

## getProcessList
### Description
- Retrieves information of all running processes in the system.

### Syntax
```cpp
bool getProcessList(
  [out] ProcessInfo* buffer,
  [in]  i32 bufferSize,
  [out] i32 processesWritten 
);
```

### Parameters
`buffer`
- A pointer to an array of ProcessInfo structs. Each item in the array will represent one currently running process in the system.

`bufferSize`
- The size of the array of ProcessInfo structs.

`adaptersWritten`
- The number of processes that were added to the array.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.

## getSystemInfo
### Description
- Retrieves basic system information to provide a summary.

### Syntax
```cpp
bool getSystemInfo(
  [out] SystemInfo* buffer,
  [in]  i32 bufferSize,
);
```

### Parameters
`buffer`
- A pointer to a SystemInfo struct that will receive system information. 

`bufferSize`
- The size of the MemoryInfo struct.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.


## getGpuSummary
### Description
- Retrieves basic information of all graphics cards in the system. This function is primarily utilized for providing an overview of the system. If you want detailed information, use [getGpuInfo](#getgpuinfo).

### Syntax
```cpp
bool getGpuSummary(
  [out] GpuSummary* buffer,
  [in]  i32 bufferSize,
  [out] i32 adaptersWritten
);
```

### Parameters
`buffer`
- A pointer to an array of GpuSummary structs. Each item in the array will represent one GPU in the system.

`bufferSize`
- The size of the array of GpuSummary structs.

`adaptersWritten`
- The number of GPUs that were added to the array.

### Returns
- If the functions succeeds, the return value is `true`. If the functions fails, the return value is `false`.
