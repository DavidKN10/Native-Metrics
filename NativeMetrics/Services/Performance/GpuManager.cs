using NativeMetrics.Services.Models;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services.Performance;

public class GpuManager
{
    public ObservableCollection<GpuViewModel> GPUs { get; } = new ObservableCollection<GpuViewModel>();
    private readonly Dictionary<ulong, GpuViewModel> gpuLookup = new Dictionary<ulong, GpuViewModel>();
  
    public async Task RefreshAsync()
    {
        GpuInfo[] newGpus = RetrieveGpus();
        Synchronize(newGpus);
    }

    private GpuInfo[] RetrieveGpus()
    {
        GpuInfo[] gpuList = new GpuInfo[128];
        if (!NativeMetricsService.getGpuInfo(gpuList, gpuList.Length, out int gpusWritten))
        {
            return Array.Empty<GpuInfo>();
        }

        return gpuList.Take(gpusWritten).ToArray();
    }

    private void Synchronize(GpuInfo[] gpuList)
    {
        HashSet<ulong> luidList = BuildGpuLuidSet(gpuList);

        foreach (GpuInfo gpu in gpuList) 
        {
            // update gpu in Dictionary and ObservableCollection
            if (gpuLookup.ContainsKey(gpu.luid))
            {
                gpuLookup[gpu.luid].Update(gpu);

                GpuViewModel? gpuToUpdate = GPUs.FirstOrDefault(x => x.Luid == gpu.luid);
                if (gpuToUpdate != null)
                {
                    gpuToUpdate.Update(gpu);
                }
            }
            // add new gpu to Dictionary and ObservableCollection
            else
            {
                GpuViewModel newGpu = new GpuViewModel(gpu);
                gpuLookup.Add(gpu.luid, newGpu);
                GPUs.Add(newGpu);
            }
        }

        // remove disconnected GPUs in Dictionary and ObservableCollection
        foreach (var kvp in gpuLookup.ToList())
        {
            var luid = kvp.Key;
            if (!luidList.Contains(luid))
            {
                gpuLookup.Remove(luid);

                GpuViewModel? gpuToRemove = GPUs.FirstOrDefault(x => x.Luid == luid);
                if (gpuToRemove != null)
                {
                    GPUs.Remove(gpuToRemove);
                }
            }
        }
    }

    private HashSet<ulong> BuildGpuLuidSet(GpuInfo[] gpuList) 
    {
        HashSet<ulong> luidList = new HashSet<ulong>();
        foreach (GpuInfo gpu in gpuList) 
        {
            luidList.Add(gpu.luid);     
        }
        return luidList;
    }
}
