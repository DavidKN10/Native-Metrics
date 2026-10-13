using System;
using System.Collections.Generic;
using System.Linq;
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
}

public static class Formatter
{
    private const int DIV = 1024;

    private static double BytesToKB(double value)
    {
        return value / (DIV);
    }

    private static double BytesToMB(double value)
    {
        return value / (DIV * DIV);
    }

    private static double BytesToGB(double value)
    {
        return value / (DIV * DIV * DIV);
    }

    private static double BytesToTB(double value)
    {
        // We conver to GB first because we overflow otherwise.
        return BytesToGB(value) / (DIV);
    }

    public static FormattedValue DiskMetricString(ulong value)
    {
        if (value >= 1_099_511_627_776UL)    // 1024^4
        {
            double valueDouble = value;
            return new FormattedValue($"{BytesToTB(valueDouble):F2}", " TB");
        }
        else if (value >= 1_073_741_824UL)  // 1024^3
        {
            double valueDouble = value;
            return new FormattedValue($"{BytesToGB(valueDouble):F2}", " GB");
        }
        else if (value >= 1_048_576UL)  // 1024^2 bytes
        {
            double valueDouble = value;
            return new FormattedValue($"{BytesToMB(valueDouble):F2}", " MB");
        }
        else if (value >= 1_024UL)  // 1024 bytes
        {
            double valueDouble = value;
            return new FormattedValue($"{BytesToKB(valueDouble):F2}", " KB");
        }
        else
        {
            double valueDouble = value;
            return new FormattedValue($"{valueDouble:F2}", " B");
        }
    }

    public static FormattedValue PercentString(double value)
    {
        string stringValue = $"{value:F1}";
        return new FormattedValue(stringValue, "%");
    }

    public static FormattedValue MillihertzString(uint value)
    {
        FormattedValue formattedValue = new FormattedValue(value.ToString(), " MHz");
        return formattedValue;
    }
}
