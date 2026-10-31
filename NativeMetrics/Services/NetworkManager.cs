using NativeMetrics.Services.Models;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class NetworkManager
{
    public ObservableCollection<NetworkViewModel> Networks { get; } = new();
    private readonly Dictionary<ulong, NetworkViewModel> networkLookup = new();
    
    public async Task RefreshAsync()
    {
        NetworkAdapterInfo[] newNetworks = RetrieveNetworks();
        Synchronize(newNetworks);
    }
    
    private NetworkAdapterInfo[] RetrieveNetworks()
    {
        NetworkAdapterInfo[] networkList = new NetworkAdapterInfo[200];
        if (!NativeMetricsService.getNetworkAdapterInfo(networkList, networkList.Length, out int networksWritten))
        {
            return Array.Empty<NetworkAdapterInfo>();
        }

        return networkList.Take(networksWritten).ToArray();
    }
    
    private void Synchronize(NetworkAdapterInfo[] networks)
    {
        HashSet<ulong> luids = BuildNetworkLuidSet(networks);

        foreach (NetworkAdapterInfo network in networks) 
        {
            // update network in Dictionary and observable collection
            if (networkLookup.ContainsKey(network.luid))
            {
                networkLookup[network.luid].Update(network);
            }
            // add new network to Dictionary and ObservableCollection
            else
            {
                NetworkViewModel newNetwork = new NetworkViewModel(network);
                networkLookup.Add(network.luid, newNetwork);
                Networks.Add(newNetwork);
            }
        }

        // remove disconnected networks in Dictionary and ObservableCollection
        foreach (var kvp in networkLookup.ToList())
        {
            var luid = kvp.Key;
            if (!luids.Contains(luid))
            {
                networkLookup.Remove(luid);

                NetworkViewModel? networkToRemove = Networks.FirstOrDefault(x => x.Luid == luid);
                if (networkToRemove != null)
                {
                    Networks.Remove(networkToRemove);
                }
            }
        }
    }

    private HashSet<ulong> BuildNetworkLuidSet(NetworkAdapterInfo[] networks)
    {
        HashSet<ulong> luids = new();
        foreach(NetworkAdapterInfo network in networks)
        {
            luids.Add(network.luid);
        }
        return luids;
    }
}
