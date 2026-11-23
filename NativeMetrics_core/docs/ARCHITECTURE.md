# Backend Architecture

# Index
- [Backend Architecture](#backend-architecture)
- [Index](#index)
- [Overview](#overview)
- [Module Organization](#module-organization)
- [Data Flow](#data-flow)
- [Windows API](#windows-api)
- [Data Collection Strategy](#data-collection-strategy)
- [State Management](#state-management)
- [Threading Model](#threading-model)
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

# Module Organization

# Data Flow

# Windows API

# Data Collection Strategy

# State Management

# Threading Model

# Memory Management

# Error Handling

# Extending the Library

# Performance Considerations

# Limitations

# Dependencies