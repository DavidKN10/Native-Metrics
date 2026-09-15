using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct DiskInfo 
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
    public string driveLetter;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string driveType;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string volumeName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string fileSystemName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string busType;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string vendor;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string model;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
    public string guidPath;

    public ulong totalSpaceBytes;
    public ulong availableSpaceBytes;
    public ulong freeSpaceBytes;
    public ulong bytesRead;
    public ulong bytesWritten;
    public double readSpeed;
    public double writeSpeed;
}
