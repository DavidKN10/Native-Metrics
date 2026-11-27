# Backend Architecture

# Index
- [Backend Architecture](#backend-architecture)
- [Index](#index)
- [Overview](#overview)
- [Project Organization](#project-organization)
- [Data Flow](#data-flow)
  - [CPU](#cpu)
  - [Disk](#disk)
  - [GPU](#gpu)
  - [Memory](#memory)
  - [Network](#network)
  - [Processes](#processes)
  - [System](#system)
- [Windows API](#windows-api)
  - [CPU](#cpu-1)
  - [Disk](#disk-1)
  - [GPU](#gpu-1)
  - [Memory](#memory-1)
  - [Network](#network-1)
  - [Processes](#processes-1)
  - [System](#system-1)
- [Data Collection Strategy](#data-collection-strategy)
  - [CPU](#cpu-2)
  - [Disk](#disk-2)
  - [GPU](#gpu-2)
  - [Memory](#memory-2)
  - [Network](#network-2)
  - [Processes](#processes-2)
  - [System](#system-2)
- [Memory Management](#memory-management)
- [Error Handling](#error-handling)
- [Extending the Library](#extending-the-library)
- [Performance Considerations](#performance-considerations)
- [Limitations](#limitations)
- [Dependencies](#dependencies)

# Overview
The purpose of this library was for me to learn how to make libraries that could be used in different programming languages. This library was also used as a sort of backend for the main app that was created with WinUI3. Some of the goals for this library included native system metric collection, cross-language friendly, minimal dependencies, and being read-only.

**Native system metric collection**: I wanted the library to be Windows native. In order to achive this, I used the Windows API to retrieve many metrics.

**Cross-language friendly**: As I mentioned before, I wanted this library to be usable outside of C/C++. So I ensured that this library would output simple data structures, fixed-sized buffers, and primitive data types in order to make them usable. For example, I could have easily used vectors for some of the functions, but I instead used arrays since the majority of languages have an implementation for it.

**Minimal dependencies**: This library neededed to be not a hassle to build or use. So I ensured that it utilized the minimum amount of dependencies. In the end, it only uses Windows API, Windows SDK, Windows Driver Kit, and DXCore/DXGI.

**Read-only**: I felt it would be better if this library only read information instead of modifying it. The main reason being that there would be a lot security considerations if my application was also able to modify low-level system operations.

At its core, the library has functions that will retrieve information from a specific part of a system. For all functions, you will need to initialize their respective struct, feed it to the function, and the function will write the data to the struct.

Here is an example for retrieving CPU information. First we create a `CpuInfo` struct and give it to the `getCpuInfo` function.
```cpp
CpuInfo cpu{};
i32 bufferSize = sizeof(cpu);
bool result = getCpuInfo(&cpu, bufferSize);
if (result) {
  std::cout << "Success!" << std::endl;
}
```

# Project Organization
The project is organized by the following:
- CPU
- Memory
- Disk
- Network
- GPU
- Processes
- System 
- Utilities
- NativeMetrics

These categories are straightforward. CPU is in charge of metrics related to the processor, Memory is in charge of metrics related to RAM, and so on. System refers to an overview of the entire system. It contains metrics from other categories but more simplified. Utilities contains helper functions that the other sections will utilize.

The main header for this project is `NativeMetrics.hpp`. This header all other headers and the functions that the library exposes. See [API](https://github.com/DavidKN10/Native-Metrics/blob/main/NativeMetrics_core/docs/API.md) for more information.

If you wanted only specific functions, you could include the other headers. For example, you can use `Cpu.hpp` if you only want CPU metrics.

# Data Flow

Below are diagrams that demonstrate how a request travels through the library and how data is returned.
## CPU

## Disk

## GPU

## Memory

## Network

## Processes

## System


# Windows API

For this project, the Windows API was used a lot. The following sections show how the API was used for each part.

## CPU
`RegKeyOpenExW`, `RegQueryValueExW`
- These functions were used to read the registry in order to obtain the processor name and the processor's base speed.

`GetSystemInfo`
- This function was used to obtain the processor's number of logial processors.

`GetLogicaProcessorInformationEx`
- This function was used to get the number of physical cores in the processor.

`GetSystemTimes`
- In order to calculate the CPU usage, I used `GetSystemTimes` to get the idle, kernel, and user time.

`GetPerformanceInfo`
- This function was used in order to obtain the number of handles, threads, and processes.

## Disk
`GetLogicalDriveStringsW`
- This function was very crucial as it was able to give us the number of drives and their drive letters. The other API functions depend on the drive letter.

`GetVolumeNameForVolumeMountPointW`
- Gives us the the GUID of the physical disk that the drive belong to. Now we have some information about the physical disks.

`GetDriveTypeW`
- Gives us the type of drive (CD-ROM, fixed drive, etc.).

`GetDiskFreeSpace`
- Gives us storage information like total space, free space, available space.

`GetVolumeInformation`
- This gives us the drive's name and file system name.

`CreateFileW` 
- Many functions related to the physical disk requrie a file handle. So we use this functions to create a file handle with the drive's path.

`DeviceIoControl`
- This function is used to find what physical disk the drive belongs to. From here, we can use this function again with different flags get more information about the physical disk.

## GPU
For GPU metrics, DXCore and DXGI was used.

`GetProperty`
- Used to obtain the graphics card's description and LUID.

`GetDesc2`
- Used to obtain the vendor ID, dedicated video memory, and shared system memory.

`QueryVideoMemoryInfo`
- Used to update the shared and and dedicated video memory usage.

## Memory
`GlobalMemoryStatusEx`
- This function gives us total memory, available memory, and memory usage percent.

`GetPerformanceInfo`
- This function gives us metrics like page size, current commit, commit limit, peak commit, paged pool memory, and non-paged pool memory.

## Network
`GetIfTable2`
- This function writes to `MIB_IF_TABLE`, which can be used to obtain a `MIB_IF_ROW2`.

`MIB_IF_ROW2`
- This struct can give us a lot of useful information about a network adapter. We can find the type, alias, description, LUID, link speed, and much more. We can also use some members like `MediaConnectState`, `OperStatus`, and `Type` to filter out adapters that we don't want to display. 

## Processes
`CreateToolHelp32Snapshot`
- Takes a snapshot of all the processes that were running when this function was called. This provides the foundation where we will collect our processes information.

`OpenProcess`
- Let's us read information about a process.

`PROCESSENTRY32W`
- This struct will cotain the current process when we iterate through the snapshot. We can see information like pid, name, threads, and base priority.

`GetPriorityClass`
- Gives us the process's priority.

`QueryFullProcessImageName`
- Gives us the path where the process was ran.

`GetProcessMemoryInfo`
- Writes to a `PROCESS_MEMORY_COUNTERS_EX` which will contain a process's information related to memory.

`PROCESS_MEMORY_COUNTERS_EX`
- Contains information like memory usage, commit size, and private memory.

## System
`GetComputerNameExW`
- Gives us the computer's name and the host name.

`GetNativeSystemInfo`
- This function was used to obtain the processor architecture.

`GetFirmwareType`
- Gives us the type of firmware like BIOS or UEFI.

`GetProductInfo`
- Gives us the Windows edition.

`RtlGetVersion`
- We can get Windows information like OS version and build number.

`GlobalMemoryStatusEx`
- We use it to display the total amount of RAM.

`GetProperty`
- This function was used in order to obtain the list of GPUs and their names.

# Data Collection Strategy
Most of the data requires a function call in the Windows API. However, there are some metrics that require us to make the calculations.

## CPU
- Sampling by calling Windows API functions.
- CPU usage requires the calculation of  the difference between the current and previous call.

## Disk
- Sampling by calling Windows API functions.
- Disk speed metrics requires the calculation of the difference between the current abd previous call.

## GPU 
- DXGI adapter enumeration.
- Adapter information obtained with DXGI or DXCore.
- Storing previous samples not required.

## Memory
- Sampling by calling Windows API.
- Storing previous samples not required.

## Network
- Sampling by calling Windows API.
- Network speed calculations required storing previous samples.

## Processes
- Sampling by getting a snapshot of all running processes using Windows API.
- Storing previous samples not required.

## System
- Sampling by calling Windows API.
- DXGI and DXCore required for making GPU list.
- Storing previous samples not required.

# Memory Management

For some of the components, memory allocation is not required. You just need to initialize a struct and pass a pointer to the library's function in order to write data to it.
In this example, I used `getMemoryInfo` to get information about the system's memory.
```cpp
MemoryInfo memInfo{};
i32 buffSize = sizeof(MemoryInfo);
if (getMemoryInfo(&memInfo, buffSize)) {
  std::cout << "Success!" << std::endl;
}
```

There are some functions that will require you to create an array of structs. You simply create a fixed sized array and pass the pointer to the array.
In this example, I created used `getProcessList` to get a list of structs.
```cpp
std::vector<ProcessInfo> list(1024);
i32 processesWritten{};
if (getProcessList(list.data(), 1024, &processesWritten)) {
  std::cout << "Success!" << std::endl;
}
```

This library utilized fixed-sized arrays and structs. As mentioned before, the reason dynamic data structures, like STL containers, are not used is due to the fact that this library was intended to be used in other programming languages as well. For example, if I wanted to use this library in C#, it would be difficult to make an implementation where a library function writes data to an std::vector. It's much simpler to use an array since both languages have them.

# Error Handling

# Extending the Library

# Performance Considerations

# Limitations

# Dependencies