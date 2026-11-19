# Backend API

# Index
- [Backend API](#backend-api)
- [Index](#index)
- [Models](#models)
  - [CpuInfo](#cpuinfo)
    - [Description](#description)
    - [Syntax](#syntax)
    - [Members](#members)
- [Functions](#functions)
  - [getCpuInfo](#getcpuinfo)
    - [Description](#description-1)
    - [Syntax](#syntax-1)
    - [Parameters](#parameters)
    - [Returns](#returns)
  - [getDiskInfo](#getdiskinfo)
    - [Description](#description-2)
    - [Syntax](#syntax-2)
    - [Parameters](#parameters-1)
    - [Returns](#returns-1)
  - [getGpuInfo](#getgpuinfo)
    - [Description](#description-3)
    - [Syntax](#syntax-3)
    - [Parameters](#parameters-2)
    - [Returns](#returns-2)
  - [getMemoryInfo](#getmemoryinfo)
    - [Description](#description-4)
    - [Syntax](#syntax-4)
    - [Parameters](#parameters-3)
    - [Returns](#returns-3)
  - [getNetworkAdapterInfo](#getnetworkadapterinfo)
    - [Description](#description-5)
    - [Syntax](#syntax-5)
    - [Parameters](#parameters-4)
    - [Returns](#returns-4)
  - [getProcessList](#getprocesslist)
    - [Description](#description-6)
    - [Syntax](#syntax-6)
    - [Parameters](#parameters-5)
    - [Returns](#returns-5)
  - [getSystemInfo](#getsysteminfo)
    - [Description](#description-7)
    - [Syntax](#syntax-7)
    - [Parameters](#parameters-6)
    - [Returns](#returns-6)
  - [getGpuSummary](#getgpusummary)
    - [Description](#description-8)
    - [Syntax](#syntax-8)
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
