using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using NativeMetrics.Services;
using NativeMetrics.Views;
using NativeMetrics.Views.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow(ElementTheme requestedTheme)
        {
            InitializeComponent();


            if (Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = requestedTheme;
            }
          
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(titleBar);
                        
            // apply saved resolution
            var presenter = AppWindow.Presenter as OverlappedPresenter;
            ApplySavedResolution();
            presenter.IsResizable = false;
            
            // show an initial page
            ContentFrame.Navigate(typeof(OverviewPage));
        }

        private void ApplySavedResolution()
        {
            var savedResolution = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Resolution"] as string;
            var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
            var appWindow = AppWindow.GetFromWindowId(windowId);
           
            switch (savedResolution)
            {
                case "Compact":
                    WindowHelper.SetWindowResolution(appWindow, 0.60f, 0.70f); 
                    break;
                case "Default":
                    WindowHelper.SetWindowResolution(appWindow, 0.75f, 0.80f); 
                    break;
                case "Large":
                    WindowHelper.SetWindowResolution(appWindow, 0.90f, 0.90f); 
                    break;
            }
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected is true)
            {
                ContentFrame.Navigate(typeof(SettingsPage)); 
            }
            else if (args.SelectedItemContainer is NavigationViewItem item)
            {
                switch(item.Tag?.ToString())
                {
                    case "OverviewPage":
                        ContentFrame.Navigate(typeof(OverviewPage));
                        break;
                    case "ProcessesPage":
                        ContentFrame.Navigate(typeof(ProcessesPage));
                        break;
                    case "PerformancePage":
                        ContentFrame.Navigate(typeof(PerformancePage));
                        break;
                    case "NetworkPage":
                        ContentFrame.Navigate(typeof(NetworkPage));
                        break;
                    case "SettingsPage":
                        ContentFrame.Navigate(typeof(SettingsPage));
                        break;
                }
            }
        }
    }
}
