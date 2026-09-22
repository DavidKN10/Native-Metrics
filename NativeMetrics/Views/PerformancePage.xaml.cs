using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NativeMetrics.Services;
using NativeMetrics.Views.Models;
using System;
using System.Collections.Generic;
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

            // show an initial page
            PerformanceContentFrame.Navigate(typeof(CpuPage), _performanceContext);
            
            this.Loaded += PerformancePage_Loaded;
            this.Unloaded += PerformancePage_Unloaded;
        }
    
        private async void PerformancePage_Loaded(object sender, RoutedEventArgs e)
        {
            await _performanceContext.CpuManager.RefreshAsync();
            await _performanceContext.MemoryManager.RefreshAsync();
        }

        private async void PerformancePage_Unloaded(object sender, RoutedEventArgs e)
        {
            _performanceContext.CpuUpdateService.StopTimer();
            _performanceContext.MemoryUpdateService.StopTimer();
        }

        private void PerformanceNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
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
    }
}
