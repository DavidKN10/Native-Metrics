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
    

    public static FormattedValue PercentString(double value)
    {
        string stringValue = $"{value:F1}";
        return new FormattedValue(stringValue, "%");
    }

    public static FormattedValue MillihertzString(uint value)
    {
        string stringValue = $"{value}";
        FormattedValue formattedValue = new FormattedValue(stringValue, " MHz");
        return formattedValue;
    }
}
