using Microsoft.UI;
using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services
{
    public static class WindowHelper
    {
        public static void SetWindowResolution(AppWindow window, float wScale, float hScale)
        {
            var displayArea = DisplayArea.GetFromWindowId(window.Id, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            int width = (int)(workArea.Width * wScale);
            int height = (int)(workArea.Height * hScale);

            window.Resize(new Windows.Graphics.SizeInt32(width, height));
        }
    }
    
}
