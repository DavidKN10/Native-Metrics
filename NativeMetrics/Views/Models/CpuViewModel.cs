using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class CpuViewModel : INotifyPropertyChanged
{
    private string? _processorName;
    private uint _logicalProcessors;
    private uint _cores;
    private uint _baseSpeed;
    private uint _handles;
    private uint _processCount;
    private uint _threadCount;
    private double _cpuUsage;

    public string? ProcessorName 
    { 
        get { return _processorName; } 
        set { _processorName = value; OnPropertyChanged(); }
    }

    public uint LogicalProcessors 
    { 
        get { return _logicalProcessors; } 
        set { _logicalProcessors = value; OnPropertyChanged(); }
    }

    public uint Cores 
    { 
        get { return _cores; } 
        set { _cores = value; OnPropertyChanged(); }
    }

    public uint BaseSpeed 
    { 
        get { return _baseSpeed; } 
        set { _baseSpeed = value; OnPropertyChanged(); }
    }

    public uint Handles
    {
       get { return _handles; }
       set { _handles = value; OnPropertyChanged(); }
    }
    
    public uint ProcessCount { 
        get { return _processCount; } 
        set { _processCount = value; OnPropertyChanged(); }
    }

    public uint ThreadCount
    {
        get { return _threadCount; }
        set { _threadCount = value; OnPropertyChanged(); }
    }
    
    public double CpuUsage
    {
        get { return _cpuUsage; }
        set { _cpuUsage = value; OnPropertyChanged(); }
    }

    public CpuViewModel()
    {
        _processorName = string.Empty;
        _logicalProcessors = 0;
        _cores = 0;
        _baseSpeed = 0;
        _handles = 0;
        _processCount = 0;
        _threadCount = 0;
        _cpuUsage = 0.0;
    }
   
    public void Update(CpuInfo cpu)
    {
        ProcessorName = cpu.processorName;
        LogicalProcessors = cpu.logicalProcessors;
        Cores = cpu.cores;
        BaseSpeed = cpu.baseSpeed;
        Handles = cpu.handles;
        ProcessCount = cpu.processCount;
        ThreadCount = cpu.threadCount;
        CpuUsage = cpu.cpuUsage;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

