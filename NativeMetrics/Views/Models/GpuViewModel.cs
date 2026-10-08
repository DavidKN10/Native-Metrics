using Microsoft.Web.WebView2.Core;
using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class GpuViewModel : INotifyPropertyChanged
{
    private string? _description;
    public ulong _luid;
    public uint _vendorId;
    public double _dedicatedVideoMemoryBytes;
    public double _localCurrentMemoryBytes;
    public double _currentUsagePercent;
    public double _sharedSystemMemoryBytes;
    public double _nonLocalCurrentMemoryBytes;

    public string? Description
    {
        get { return _description; }
        set { _description = value; OnPropertyChanged(); }
    }

    public ulong Luid
    {
        get { return _luid; }
        set { _luid = value; OnPropertyChanged(); }
    }

    public uint VendorId
    {
        get { return _vendorId; }
        set { _vendorId =  value; OnPropertyChanged(); }
    }

    public double DedicatedVideoMemoryBytes
    {
        get { return _dedicatedVideoMemoryBytes; }
        set { _dedicatedVideoMemoryBytes = value; OnPropertyChanged(); }
    }

    public double LocalCurrentMemoryBytes
    {
        get { return _localCurrentMemoryBytes; }
        set { _localCurrentMemoryBytes = value; OnPropertyChanged(); }
    }

    public double CurrentUsagePercent
    {
        get { return _currentUsagePercent; }
        set { _currentUsagePercent = value; OnPropertyChanged(); }
    }

    public double SharedSystemMemoryBytes
    {
        get { return _sharedSystemMemoryBytes; }
        set { _sharedSystemMemoryBytes = value;OnPropertyChanged(); }
    }

    public double NonLocalCurrentMemoryBytes
    {
        get { return _nonLocalCurrentMemoryBytes; }
        set { _nonLocalCurrentMemoryBytes = value; OnPropertyChanged(); }
    }

    public GpuViewModel()
    {
        _description = string.Empty;
        _luid = 0;
        _vendorId = 0;
        _dedicatedVideoMemoryBytes = 0.0;
        _localCurrentMemoryBytes = 0.0;
        _currentUsagePercent = 0.0;
        _sharedSystemMemoryBytes = 0.0;
        _nonLocalCurrentMemoryBytes= 0.0;
    }

    public GpuViewModel(GpuInfo gpu)
    {
        Description = gpu.description;
        Luid = gpu.luid;
        VendorId = gpu.vendorId;
        DedicatedVideoMemoryBytes= gpu.dedicatedVideoMemoryBytes;
        LocalCurrentMemoryBytes = gpu.localCurrentMemoryBytes;
        CurrentUsagePercent = gpu.currentUsagePercent;
        SharedSystemMemoryBytes = gpu.sharedSystemMemoryBytes;
        NonLocalCurrentMemoryBytes = gpu.nonLocalCurrentMemoryBytes;
    }

    public void Update(GpuInfo gpu)
    {
        Description = gpu.description;
        Luid = gpu.luid;
        VendorId = gpu.vendorId;
        DedicatedVideoMemoryBytes= gpu.dedicatedVideoMemoryBytes;
        LocalCurrentMemoryBytes = gpu.localCurrentMemoryBytes;
        CurrentUsagePercent = gpu.currentUsagePercent;
        SharedSystemMemoryBytes = gpu.sharedSystemMemoryBytes;
        NonLocalCurrentMemoryBytes = gpu.nonLocalCurrentMemoryBytes;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
