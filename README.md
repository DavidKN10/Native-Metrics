# Native Metrics

<div align=center>
  <img src="docs/screenshots/app_icon.png" alt="Image of app's logo.">
</div>

System monitoring application for Windows. Built with WinUI 3 with a native C++ backend.

## Index
- [Native Metrics](#native-metrics)
  - [Index](#index)
  - [Screenshots](#screenshots)
  - [Features](#features)
  - [System Requirements](#system-requirements)
    - [Installation Requirements:](#installation-requirements)
    - [Build Requirements:](#build-requirements)
    - [Not Supported/Not Tested](#not-supportednot-tested)
  - [Quick Install](#quick-install)
  - [Quick Build](#quick-build)
  - [Documentation](#documentation)
  - [License](#license)


## Screenshots
<img src="docs/screenshots/overview_pag.png" alt="Image of app's overview page."/>
<img src="docs/screenshots/processes_page.png" alt="Image of app's processes page."/>
<img src="docs/screenshots/cpu_page.png" alt="Image of app's CPU info page."/>

## Features

With Native Metrics, you can view the following information:
- System overview like operating system and hardware information.
- View every running process and see data like process ID, memory usage, commit size, and private memory.
- Performance page that contains metrics about a deivce's CPU, memory, disks, and graphics cards. These metrics update at every time interval.
- Each page contains a live graph displaying one metric:
    - CPU page displays CPU usage percentage.
    - Memory page displays memory usage percentage.
    - Disk pages display a graph for active time and another for transfer rate.

**Note:** GPU page has limited implementation as I programmed the backend to use DXGI and DXCore which provide limited information about a graphics card.

## System Requirements
### Installation Requirements:
Operating System:
- Windows 10
- Windows 11

Architecture:
- x64 processor

**Recommended:**
- Windows 11
- Latest Windows update
- 1920x1080 display

### Build Requirements:
Software:
- Visual Studio 2026
- Visual Studio 18 2026
- CMake 4.4.3
- Ninja 1.13.2
- Git
- Windows SDK 10.0.28000.2957
- Windows Driver Kit 10.0.28000.2526

### Not Supported/Not Tested
- Windows 7
- Windows 8/8.1
- Linux
- macOS
- 32-bit Windows

## Quick Install

## Quick Build

## Documentation

## License
Native Metrics is licensed under MIT license. See [LICENSE](https://github.com/DavidKN10/Native-Metrics/blob/main/LICENSE) for more information.