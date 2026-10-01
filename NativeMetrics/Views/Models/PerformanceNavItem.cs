using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class PerformanceNavItem
{
    public string Title { get; set; }
    public Type PageType { get; set; }
    public object Data { get; set; }
}
