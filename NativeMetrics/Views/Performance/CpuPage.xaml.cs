using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NativeMetrics.Services;
using NativeMetrics.Services.Performance;
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
    public sealed partial class CpuPage : Page
    {
        private CpuManager _manager;
        private CpuUpdateService _updateService;

        public CpuPage()
        {
            InitializeComponent();

            this.Loaded += CpuPage_Loaded;
            this.Unloaded += CpuPage_Unloaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var context = (PerformanceContext)e.Parameter;

            _manager = context.CpuManager;
            _updateService = context.CpuUpdateService;
        }

        private async void CpuPage_Loaded(object sender, RoutedEventArgs e)
        {
            _updateService?.StartTimer();
            await _manager.RefreshAsync();
        }

        private void CpuPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _updateService?.StopTimer();
        }
    }
}
