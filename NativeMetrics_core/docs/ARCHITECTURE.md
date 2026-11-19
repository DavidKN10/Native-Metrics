# Backend Architecture

## Index
- [Backend Architecture](#backend-architecture)
  - [Index](#index)
  - [Overview](#overview)
  - [Design Decisions](#design-decisions)
  - [CPU](#cpu)
  - [Disk](#disk)
  - [GPU](#gpu)
  - [Memory](#memory)
  - [Network](#network)
  - [Processes](#processes)
  - [System Overview](#system-overview)

## Overview
At its core, the library has functions that will retrieve information from a specific part of a system. For all functions, you will need to initialize their respective struct, feed it to the function, and the function will write the data to the struct.

## Design Decisions
During the development of this project. I had to make serveral important design decisions.

**Why a C++ library?**
- I decided to make a C++ library mainly for learning purposes. My main objective was to learn how to create applications with a GUI, and how to create a library that would be able to be utilized by many programming languages.

**Why WinUI3?**
- I wanted to learn C#. So I figured that making the GUI in C# would be a good idea. I chose WinUI3 over WPF because it's supposed to be Microsoft's UI framework for modern Windows applications.

**Why fixed-sized buffers?**
- As you will see later, some of the backend's functions write data to a fixed-size array of structs. I chose to use arrays, instead of something like a vector for example, because it would be easier to perform marshalling in C#. Primitive data types and basic data structures like arrays are genreally easier for marshalling. 

## CPU
`getCpuInfo` will retreive the following:
- processor name
- number of logical processors
- number of cores
- base speed
- number of handles
- number of processes
- number of total threads
- cpu usage percentage

To use `getCpuInfo` you initialize a `CpuInfo` struct.
```cpp
CpuInfo cpu{};
```

**Note:** You may have noticed that the structs that we are going to use for marshalling are not part of the NativeMetrics namespace. The reason for this is because adding namespaces to them makes marshalling more difficult. 

After initializing the struct, you call `getCpuInfo`.
```cpp
CpuInfo cpu{};
i32 size = sizeof(cpu);
bool result = getCpuInfo(&cpu, size);
if (result) {
  std::cout << "Success!\n";
}
```


## Disk

## GPU

## Memory

## Network

## Processes

## System Overview

