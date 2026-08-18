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
        private PerformanceManager _performanceManager;
        private PerformanceUpdateService _updateService;

        public PerformancePage()
        {
            InitializeComponent();
            _performanceManager = new PerformanceManager();
            _updateService = new PerformanceUpdateService(_performanceManager);

            this.Loaded += PerformancePage_Loaded;
            this.Unloaded += PerformancePage_Unloaded;
        }

        private async void PerformancePage_Loaded(object sender, RoutedEventArgs e)
        {
            await _performanceManager.RefreshAsync();
        }

        private async void PerformancePage_Unloaded(object sender, RoutedEventArgs e)
        {
            _updateService?.StopTimer(); 
        }
    }
}
