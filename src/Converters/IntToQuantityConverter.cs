using Microsoft.UI.Xaml.Data;
using MiniPos.Constants;
using System;

namespace MiniPos;
public partial class IntToQuantityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int intValue)
        {
            return AppConstants.MultipleSymbol + intValue.ToString();
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue && strValue.StartsWith(AppConstants.MultipleSymbol))
        {
            strValue = strValue.Substring(1); // Remove the "x" prefix
            if (int.TryParse(strValue, out var result))
            {
                return result;
            }
        }

        return value;
    }
}
