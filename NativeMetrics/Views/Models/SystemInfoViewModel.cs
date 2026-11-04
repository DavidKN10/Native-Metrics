using NativeMetrics.Services;
using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class SystemInfoViewModel
{
    private string? _deviceName;
    private string? _systemType;
    private string? _windowsVersion;
    private string? _windowsEdition;
    private string? _hostname;
    private string? _firmware;
    private string? _processor;
    private ulong _buildNumber;
    private ulong _installedRam;
    private ulong _storageCapacity;
    private ulong _usedStorage;
    private List<GpuSummary> _gpuList;
    
    public string? DeviceName 
    {
        get { return _deviceName; }
        set { _deviceName = value; }
    }

    public string? SystemType
    {
        get { return _systemType; }
        set { _systemType = value; }
    }

    public string? WindowsVersion
    {
        get { return _windowsVersion; }
        set { _windowsVersion = value; }
    }

    public string? WindowsEdition
    {
        get { return _windowsEdition; }
        set { _windowsEdition = value; }
    }

    public string? Hostname
    {
        get { return _hostname; }
        set { _hostname = value; }
    }

    public string? Firmware
    {
        get { return _firmware; }
        set { _firmware = value; }
    }

    public string? Processor
    {
        get { return _processor; }
        set { _processor = value; }
    }

    public ulong BuildNumber
    {
        get { return _buildNumber; }
        set { _buildNumber = value; }
    }

    public ulong InstalledRam
    {
        get { return _installedRam; }
        set { _installedRam = value; }
    }

    public ulong StorageCapacity
    {
        get { return _storageCapacity; }
        set { _storageCapacity = value; }
    }

    public ulong UsedStorage
    {
        get { return _usedStorage; }
        set { _usedStorage = value; }
    }

    public List<GpuSummary> GpuList
    {
        get { return _gpuList; }
        set { _gpuList = value; }
    }

    public FormattedValue InstalledRamFormatted { get; set; }
    public FormattedValue StorageCapacityFormatted { get; set; }
    public FormattedValue UsedStorageFormatted { get; set; }

    public SystemInfoViewModel()
    {
        _deviceName = string.Empty;
        _systemType = string.Empty;
        _windowsVersion = string.Empty;
        _windowsEdition = string.Empty;
        _hostname = string.Empty;
        _firmware = string.Empty;
        _processor = string.Empty;
         _buildNumber = 0;
        _installedRam = 0;
        _storageCapacity = 0;
        _usedStorage = 0;

        _gpuList = new();

        InstalledRam = new();
        StorageCapacityFormatted = new();
        UsedStorageFormatted = new();
    }

    public void Update()
    {
        SystemInfo systemInfo = new();
        int bufferSize = Marshal.SizeOf<SystemInfo>();
        
        if (NativeMetricsService.getSystemInfo(ref systemInfo, bufferSize))
        {
            DeviceName = systemInfo.deviceName;
            SystemType = systemInfo.systemType;
            WindowsVersion = systemInfo.windowsVersion;
            WindowsEdition = systemInfo.windowsEdition;
            Hostname = systemInfo.hostname;
            Firmware = systemInfo.firmware;
            Processor = systemInfo.processor;
            BuildNumber = systemInfo.buildNumber;
            InstalledRam = systemInfo.installedRam;
            StorageCapacity = systemInfo.storageCapacity;
            UsedStorage = systemInfo.usedStorage;
        }

        GpuList = RetreiveGpu();

        InstalledRamFormatted = Formatter.ByteMetricString(InstalledRam);
        StorageCapacityFormatted = Formatter.ByteMetricString(StorageCapacity);
        UsedStorageFormatted = Formatter.ByteMetricString(UsedStorage);
    }

    private List<GpuSummary> RetreiveGpu()
    {
        GpuSummary[] gpuAdapters = new GpuSummary[8];
        if (!NativeMetricsService.getGpuSummary(gpuAdapters, gpuAdapters.Length, out int adaptersWritten))
        {
            return new(); 
        }
        return gpuAdapters.ToList();
    }
}
