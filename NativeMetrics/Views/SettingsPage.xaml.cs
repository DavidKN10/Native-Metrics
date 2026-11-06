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

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        var savedTheme = localSettings.Values["AppTheme"] as string;
        
        foreach (ComboBoxItem item in ThemeSelector.Items)
        {
            if (item.Tag.ToString() == savedTheme)
            {
                ThemeSelector.SelectedItem = item;
                break;
            }
        }
    }
}
