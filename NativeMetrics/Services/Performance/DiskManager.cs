using NativeMetrics.Services;
using NativeMetrics.Services.Models;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Performance;

public class DiskManager
{
    public ObservableCollection<DiskViewModel> Disks { get; } = new ObservableCollection<DiskViewModel>();
    private readonly Dictionary<string, DiskViewModel> diskLookup = new Dictionary<string, DiskViewModel>();

    public async Task RefreshAsync()
    {
        DiskInfo[] newDisks = RetrieveDisks();

        Synchronize(newDisks);

    }

    private DiskInfo[] RetrieveDisks()
    {
        DiskInfo[] diskList = new DiskInfo[128];
        if (!NativeMetricsService.getDiskInfo(diskList, diskList.Length, out int disksWritten))
        {
            return Array.Empty<DiskInfo>();
        }

        return diskList.Take(disksWritten).ToArray();
    }

    private void Synchronize(DiskInfo[] disks)
    {
        HashSet<string> guidPaths = BuildDiskGuidSet(disks);

        foreach (DiskInfo disk in disks)
        {
            // update disk in Dictionary and ObservableCollection
            if (diskLookup.ContainsKey(disk.guidPath))
            {
                diskLookup[disk.guidPath].Update(disk);

                DiskViewModel? diskToUpdate = Disks.FirstOrDefault(x => x.GuidPath == disk.guidPath);
                if (diskToUpdate != null) 
                {
                    diskToUpdate.Update(disk);
                }
            
            }
            // add new disk to Dictionary and ObservableCollection
            else
            {
                DiskViewModel newDisk = new DiskViewModel(disk);
                diskLookup.Add(disk.guidPath, newDisk);
                Disks.Add(newDisk);
            }
        }

        // remove disconnected disks in Dictionary and ObservableCollection
        foreach (var kvp in diskLookup.ToList())
        {
            var guid = kvp.Key;
            if (!guidPaths.Contains(guid))
            {
                diskLookup.Remove(guid);

                DiskViewModel? diskToRemove = Disks.FirstOrDefault(x => x.GuidPath == guid);
                if (diskToRemove != null)
                {
                    Disks.Remove(diskToRemove);
                }
            }
        }
    }

    private HashSet<string> BuildDiskGuidSet(DiskInfo[] disks)
    {
        HashSet<string> guidPaths = new HashSet<string>();
        foreach (DiskInfo disk in disks)
        {
            guidPaths.Add(disk.guidPath);
        }
        return guidPaths;
    }
}
