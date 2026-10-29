using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public class DynamicAxisScaler
{
    public double CurrentMax { get; set; }
    public string Unit { get; set; }
    public double Divisor { get; set; }
    public HistoryBuffer<double> History;

    public DynamicAxisScaler()
    {
        Unit = "B/s";
        History = new();
    }

    public void Update()
    {
        double historyMax = History.MaxValue();

        if (historyMax == 0.0)
        {
            CurrentMax = 0.0;
            Unit = "B/s";
            double rounded = Round(CurrentMax);
            CurrentMax = rounded;
        }
        
        if (historyMax != CurrentMax)
        {
            CurrentMax = historyMax;
            FormatMax();
            double rounded = Round(CurrentMax);
            CurrentMax = rounded;
        }
    }

    public void FormatMax()
    {
        if (CurrentMax >= 1_073_741_824UL)  // 1024^3 bytes
        {
            Unit = "GB/s";
            Divisor = 1024 * 1024 * 1034;
            CurrentMax /= Divisor;
        }
        else if (CurrentMax >= 1_048_576UL)  // 1024^2 bytes
        {
            Unit = "MB/s";
            Divisor = 1024 * 1024;
            CurrentMax /= Divisor;
        }
        else if (CurrentMax >= 1_024UL)  // 1024 bytes
        {
            Unit = "KB/s";
            Divisor = 1024;
            CurrentMax /= Divisor;
        }
        else
        {
            Divisor = 1;
            Unit = "B/s";
        }
    }

    public double Round(double value)
    {
        if (value < 0.5)
        {
            return 0.5;
        } 

        double[] steps =
        {
            0.5,
            1,
            2,
            5,
            10,
            20,
            50,
            100,
            500,
            1000
        };

        foreach(double step in steps)
        {
            if (value <= step)
            {
                return step;
            }
        }
        return Math.Ceiling(value / 1000) * 1000;
    }

}
