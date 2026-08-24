using NativeMetrics.Services;
using NativeMetrics.Services.Models;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class ProcessManager
{
    public ObservableCollection<ProcessesViewModel> Processes { get; } = new();

    private readonly Dictionary<uint, ProcessesViewModel> processLookup = new Dictionary<uint, ProcessesViewModel>();

    public async Task RefreshAsync()
    {
        ProcessInfo[] newProcesses = RetrieveProcessSnapshot();

        Synchronize(newProcesses);
    }

    private ProcessInfo[] RetrieveProcessSnapshot()
    {
        ProcessInfo[] processList = new ProcessInfo[1024];
        
        if (!NativeMetricsService.getProcessList(processList, processList.Length, out int processesWritten))
        {
            return Array.Empty<ProcessInfo>();
        }

        return processList.Take(processesWritten).ToArray();
    }
    
    private void Synchronize(ProcessInfo[] snapshot)
    {
        HashSet<uint> snapshotProcessIds = BuildSnapshotPidSet(snapshot);
 
        foreach(ProcessInfo process in snapshot)
        {
            // update process in Dictionary and ObservableCollection
            if (processLookup.ContainsKey(process.processId))
            {
                processLookup[process.processId].ThreadsCount = process.threadsCount;
                processLookup[process.processId].MemoryUsage = process.memoryUsage;
                processLookup[process.processId].PrivateMemory = process.privateMemory;

                ProcessesViewModel? processToUpdate = Processes.FirstOrDefault(x => x.ProcessId == process.processId);
                if (processToUpdate != null)
                {
                    processToUpdate.Update(process);
                }
                
            }
            // add new process to Dictionary and ObservableCollection
            else
            {
                ProcessesViewModel newProcess = new ProcessesViewModel(process);
                processLookup.Add(process.processId, newProcess);
                Processes.Add(newProcess);

                _ = LoadIconAsync(newProcess);
            }
        }

        // remove exited processes in Dictionary and ObservableCollection
        foreach (var (processId, process) in processLookup)
        {
            if (!snapshotProcessIds.Contains(processId))
            {
                processLookup.Remove(processId);

                ProcessesViewModel? processToRemove = Processes.FirstOrDefault(x => x.ProcessId == processId);
                if (processToRemove != null)
                {
                    Processes.Remove(processToRemove);
                }
            }
        }         
    }

    private HashSet<uint> BuildSnapshotPidSet(ProcessInfo[] snapshot)
    {
        HashSet<uint> processIds = new HashSet<uint>();
        foreach (ProcessInfo process in snapshot)
        {
            processIds.Add(process.processId);
        }
        return processIds;
    }

    private async Task LoadIconAsync(ProcessesViewModel process)
    {
        var icon = await ExeHelper.GetIconAsync(process.ProcessPath);
        process.IconSource = icon;
    }
}
