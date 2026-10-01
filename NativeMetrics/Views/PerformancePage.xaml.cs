using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NativeMetrics.Services;
using NativeMetrics.Views.Models;
using NativeMetrics.Views.Performance;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PerformancePage : Page
    {
        private readonly PerformanceContext _performanceContext;
       
        public PerformancePage()
        {
            InitializeComponent();

            _performanceContext = new();
            _performanceContext.DiskManager.Disks.CollectionChanged += Disks_CollectionChanged;

            // show an initial page
            PerformanceContentFrame.Navigate(typeof(CpuPage), _performanceContext);
            
            this.Loaded += PerformancePage_Loaded;
            this.Unloaded += PerformancePage_Unloaded;
        }
    
        private async void PerformancePage_Loaded(object sender, RoutedEventArgs e)
        {
            await _performanceContext.CpuManager.RefreshAsync();
            await _performanceContext.MemoryManager.RefreshAsync();
            await _performanceContext.DiskManager.RefreshAsync();
            SynchronizeDiskNavigationItems();
        }

        private async void PerformancePage_Unloaded(object sender, RoutedEventArgs e)
        {
            _performanceContext.CpuUpdateService.StopTimer();
            _performanceContext.MemoryUpdateService.StopTimer();
            _performanceContext.DiskUpdateService.StopTimer();
        }

        private void PerformanceNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                if (item.Tag is DiskViewModel disk)
                {
                    PerformanceContentFrame.Navigate(typeof(DiskPage), disk);
                    return;
                }

                switch(item.Tag?.ToString())
                {
                    case "CpuPage":
                        PerformanceContentFrame.Navigate(typeof(CpuPage), _performanceContext); 
                        break;
                    case "MemoryPage":
                        PerformanceContentFrame.Navigate(typeof(MemoryPage), _performanceContext);
                        break;
                }
            }
        }

        private void Disks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SynchronizeDiskNavigationItems();
        }

        private void SynchronizeDiskNavigationItems()
        {
            var disks = _performanceContext.DiskManager.Disks;
            var diskItems = PerformanceNavigationView.MenuItems
                .OfType<NavigationViewItem>()
                .Where(item => item.Tag is DiskViewModel)
                .ToList();

            foreach (NavigationViewItem item in diskItems)
            {
                if (!disks.Contains((DiskViewModel)item.Tag))
                {
                    PerformanceNavigationView.MenuItems.Remove(item);
                }
            }

            foreach (DiskViewModel disk in disks)
            {
                if (diskItems.Any(item => ReferenceEquals(item.Tag, disk)))
                {
                    continue;
                }

                string label = string.Join(" ", new[] { disk.DriveLetter, disk.VolumeName }
                    .Where(value => !string.IsNullOrWhiteSpace(value)));
                PerformanceNavigationView.MenuItems.Add(new NavigationViewItem
                {
                    Content = string.IsNullOrWhiteSpace(label) ? "Disk" : label,
                    Tag = disk
                });
            }
        }
    }
}
