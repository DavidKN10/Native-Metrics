using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class NetworkUpdateService
{
    private readonly DispatcherTimer _updateTimer = new();
    private readonly NetworkManager _networkManager = new();

    public NetworkUpdateService(NetworkManager manager)
    {
        _networkManager = manager;
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        _updateTimer.Interval = TimeSpan.FromSeconds(1);
        _updateTimer.Tick += OnTimerTick;
        _updateTimer.Start();
    }

    public void StartTimer()
    {
        _updateTimer.Start();
    }

    public void StopTimer()
    {
        _updateTimer.Stop();
    }
    
    private void OnTimerTick(object? sender, object e)
    {
        RefreshManager();
    }

    private async void RefreshManager()
    {
        await _networkManager.RefreshAsync();
    }
}
