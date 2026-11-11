using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using NativeMetrics.Services;
using Windows.Graphics;
using System.Runtime.InteropServices.Marshalling;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeMetrics.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedTheme = (ThemeSelector.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedTheme is "Light" or "Dark")
        {
            var requestedTheme = selectedTheme == "Light" ? ElementTheme.Light : ElementTheme.Dark;

            if (XamlRoot?.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = requestedTheme;
            }

            Windows.Storage.ApplicationData.Current.LocalSettings.Values["AppTheme"] = selectedTheme;
        }
    }

    public void SetWindowResolution(float wScale, float hScale)
    {
        var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
        var appWindow = AppWindow.GetFromWindowId(windowId);
        
        WindowHelper.SetWindowResolution(appWindow, wScale, hScale);
    }
    
    private void ResolutionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedResolution = (ResolutionSelector.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        switch (selectedResolution) 
        {
            case "Compact":
                SetWindowResolution(0.60f, 0.70f);
                break;
            case "Default":
                SetWindowResolution(0.75f, 0.80f);
                break;
            case "Large":
                SetWindowResolution(0.90f, 0.90f);
                break;
        }

        Windows.Storage.ApplicationData.Current.LocalSettings.Values["Resolution"] = selectedResolution;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        var savedTheme = localSettings.Values["AppTheme"] as string;
        var savedResolution = localSettings.Values["Resolution"] as string;
        
        foreach (ComboBoxItem item in ThemeSelector.Items)
        {
            if (item.Tag.ToString() == savedTheme)
            {
                ThemeSelector.SelectedItem = item;
                break;
            }
        }

        foreach (ComboBoxItem item in ResolutionSelector.Items)
        {
            if (item.Tag.ToString() == savedResolution)
            {
                ResolutionSelector.SelectedItem = item;
                break;
            }
        }
    }
}
