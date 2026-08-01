using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class ProcessesViewModel
{
    public ProcessesViewModel(ProcessInfo process) 
    {
        ProcessName = process.processName;
        ProcessId = process.processId;
        ThreadsCount = process.threadsCount;
        ParentProcessId = process.parentProcessId;
        PriorityClassBase = process.priorityClassBase;
        PriorityClass = process.priorityClass;
        MemoryUsage = process.memoryUsage;
        CommitSize = process.commitSize;
        PrivateMemory = process.privateMemory;
    }

    public string? ProcessName { get; set; }

    public uint ProcessId {  get; set; }
    public uint ThreadsCount { get; set; }
    public uint ParentProcessId { get; set; }
    public int PriorityClassBase { get; set; }
    public uint PriorityClass { get; set; }

    public double MemoryUsage { get; set; }
    public double CommitSize { get; set; }
    public double PrivateMemory {  get; set; }
    
    public void UpdateThreadsCount(ProcessInfo process)
    {
        ThreadsCount = process.threadsCount;
    }
   
    public void UpdateMemoryUsage(ProcessInfo processs) 
    {
        MemoryUsage = processs.memoryUsage;  
    }
    
    public void UpdatePrivateMemory(ProcessInfo process)
    {
        PrivateMemory = process.privateMemory;
    }
}
