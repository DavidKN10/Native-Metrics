using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

using NativeMetrics.Views;
using NativeMetrics.Views.Network;
using Microsoft.UI.Windowing;

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

            var presenter = AppWindow.Presenter as OverlappedPresenter;
            ApplySavedResolution();
            presenter.IsResizable = false;
            
            // show an initial page
            ContentFrame.Navigate(typeof(OverviewPage));
        }

        private void ApplySavedResolution()
        {
            var savedResolution = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Resolution"] as string;

            Windows.Graphics.SizeInt32? size = savedResolution switch
            {
                "Small" => new Windows.Graphics.SizeInt32(1280, 720),
                "Medium" => new Windows.Graphics.SizeInt32(1600, 900),
                "Large" => new Windows.Graphics.SizeInt32(1920, 1080),
                _ => null
            };

            if (size is { } requestedSize)
            {
                AppWindow.Resize(requestedSize);
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
