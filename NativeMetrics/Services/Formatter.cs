using LiveChartsCore.SkiaSharpView.TypeConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Services;

public struct FormattedValue
{
    public string Number = string.Empty;
    public string Unit = string.Empty;

    public string ValueString => $"{Number}{Unit}";

    public FormattedValue(string value, string unit)
    {
        Number = value;
        Unit = unit;
    }

    public double toDouble()
    {
        return double.Parse(Number);
    }

    public void setNumberS(string value)
    {
        Number = value;
    }

    public void setNumberF(double value)
    {
        string strValue = $"{value}";
        Number = strValue;
    }
}

public static class Formatter
{
    private const int BYTE_DIV = 1024;
    private const int BIT_DIV = 1000;

    public static double BytesToKB(double value)
    {
        return value / (BYTE_DIV);
    }

    public static double BytesToMB(double value)
    {
        return value / (BYTE_DIV * BYTE_DIV);
    }

    public static double BytesToGB(double value)
    {
        return value / (BYTE_DIV * BYTE_DIV * BYTE_DIV);
    }

    public static double BytesToTB(double value)
    {
        // We convert to GB first because we overflow otherwise.
        return BytesToGB(value) / (BYTE_DIV);
    }

    public static double BitsToKb(double value)
    {
        return value / BIT_DIV;
    }

    public static double BitsToMb(double value)
    {
        return value / (BIT_DIV * BIT_DIV);
    }

    public static double BitsToGb(double value)
    {
        return value / (BIT_DIV * BIT_DIV * BIT_DIV);
    }

    public static FormattedValue ByteMetricString(ulong value)
    {
        double valueDouble = value;

        if (value >= 1_099_511_627_776UL)    // 1024^4 bytes
        {
            return new FormattedValue($"{BytesToTB(valueDouble):F2}", " TB");
        }
        else if (value >= 1_073_741_824UL)  // 1024^3 bytes
        {
            return new FormattedValue($"{BytesToGB(valueDouble):F2}", " GB");
        }
        else if (value >= 1_048_576UL)  // 1024^2 bytes
        {
            return new FormattedValue($"{BytesToMB(valueDouble):F2}", " MB");
        }
        else if (value >= 1_024UL)  // 1024 bytes
        {
            return new FormattedValue($"{BytesToKB(valueDouble):F2}", " KB");
        }
        else
        {
            return new FormattedValue($"{valueDouble:F2}", " B");
        }
    }

    public static FormattedValue ByteMetricStringF(double value)
    {
        if (value >= 1_099_511_627_776UL)    // 1024^4 bytes
        {
            return new FormattedValue($"{BytesToTB(value):F2}", " TB");
        }
        else if (value >= 1_073_741_824UL)  // 1024^3 bytes
        {
            return new FormattedValue($"{BytesToGB(value):F2}", " GB");
        }
        else if (value >= 1_048_576UL)  // 1024^2 bytes
        {
            return new FormattedValue($"{BytesToMB(value):F2}", " MB");
        }
        else if (value >= 1_024UL)  // 1024 bytes
        {
            return new FormattedValue($"{BytesToKB(value):F2}", " KB");
        }
        else
        {
            return new FormattedValue($"{value:F2}", " B");
        }
    }

    public static FormattedValue LinkSpeedString(ulong value)
    {
        double valueDouble = value;

        if (valueDouble >= 1_000_000_000UL)
        {
            return new FormattedValue($"{BitsToGb(valueDouble):F2}", " Gbps");
        }
        else if (valueDouble >= 1_000_000UL)
        {
            return new FormattedValue($"{BitsToMb(valueDouble):F2}", " Mbps");
        }
        else if (valueDouble >= 1_000UL)
        {
            return new FormattedValue($"{BitsToKb(valueDouble):F2}", " Kbps");
        }
        else
        {
            return new FormattedValue($"{valueDouble:F2}", " bps");
        }
    }

    public static FormattedValue DiskSpeedString(double value)
    {
        if (value >= 1_099_511_627_776UL)    // 1024^4 bytes
        {
            return new FormattedValue($"{BytesToTB(value):F2}", " TB/s");
        }
        else if (value >= 1_073_741_824UL)  // 1024^3 bytes
        {
            return new FormattedValue($"{BytesToGB(value):F2}", " GB/s");
        }
        else if (value >= 1_048_576UL)  // 1024^2 bytes
        {
            return new FormattedValue($"{BytesToMB(value):F2}", " MB/s");
        }
        else if (value >= 1_024UL)  // 1024 bytes
        {
            return new FormattedValue($"{BytesToKB(value):F2}", " KB/s");
        }
        else
        {
            return new FormattedValue($"{value:F2}", " B/s");
        }
    }
    public static FormattedValue PercentString(ulong value)
    {
        double doubleValue = value;
        return new FormattedValue($"{doubleValue:F1}", "%");
    }

    public static FormattedValue PercentStringF(double value)
    {
        return new FormattedValue($"{value:F1}", "%");
    }

    public static FormattedValue MillihertzString(uint value)
    {
        FormattedValue formattedValue = new FormattedValue(value.ToString(), " MHz");
        return formattedValue;
    }
}
