using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace NativeMetrics.Services;

public static class ExeHelper
{
    private static SoftwareBitmapSource _defaultIcon;
    public static async Task<SoftwareBitmapSource> GetIconAsync(string exePath)
    {
        if (string.IsNullOrEmpty(exePath) || !File.Exists(exePath))
        {
            return await GetDefaultIconAsync();
        }

        try
        {
            StorageFile file = await (StorageFile.GetFileFromPathAsync(exePath));

            using (StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(
                ThumbnailMode.SingleItem,
                64,
                ThumbnailOptions.UseCurrentScale))
            {
                if (thumbnail != null)
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(thumbnail);
                    SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Premultiplied);

                    SoftwareBitmapSource source = new SoftwareBitmapSource();
                    await source.SetBitmapAsync(softwareBitmap);
                    return source;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Icon extraction failed for {exePath}: {ex.Message}");
        }
        return await GetDefaultIconAsync();
    }

    public static async Task<SoftwareBitmapSource> GetDefaultIconAsync()
    {
        if (_defaultIcon != null)
        {
            return _defaultIcon;
        }

        try
        {
            StorageFile defaultIconFile = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Assets/placeholder16x16.ico"));

            using (var stream = await defaultIconFile.OpenReadAsync())
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied);

                _defaultIcon = new SoftwareBitmapSource();
                await _defaultIcon.SetBitmapAsync(softwareBitmap);
                return _defaultIcon;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to load default icon: {ex.Message}");
            return null;
        }
    }
}
