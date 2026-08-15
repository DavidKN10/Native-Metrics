using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;
 
public class PerformanceViewModel : INotifyPropertyChanged
{
    private ulong _totalMemory;
    private ulong _availableMemory;
    private ulong _aprroxMemPercent;
    private double _cpuUsage;

    public ulong TotalMemory { 
        get { return _totalMemory; } 
        set { _totalMemory = value; }
    }

    public ulong AvailableMemory { 
        get { return _availableMemory; }
        set { _availableMemory = value; }
    }

    public ulong ApproxMemPercent { 
        get { return _aprroxMemPercent; }
        set {_aprroxMemPercent = value; }
    }

    public double CpuUsage { 
        get { return _cpuUsage; } 
        set { _cpuUsage = value; }
    }

    public PerformanceViewModel()
    {
        _totalMemory = 0;
        _availableMemory = 0;
        _aprroxMemPercent = 0;
        _cpuUsage = 0.0;
    }

    public PerformanceViewModel(
        ulong totalMemory, 
        ulong availableMemory, 
        ulong approxMemPercent, 
        double cpuUsage)
    {
        _totalMemory = totalMemory;
        _availableMemory = availableMemory;
        _aprroxMemPercent = approxMemPercent;
        _cpuUsage = cpuUsage;
    }

    public void Update(
        ulong totalMemory, 
        ulong availableMemory, 
        ulong approxMemPercent, 
        double cpuUsage)
    {
        TotalMemory = totalMemory;
        AvailableMemory = availableMemory;
        ApproxMemPercent = approxMemPercent;
        CpuUsage = cpuUsage;
        OnPropertyChanged();
    }
 
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
