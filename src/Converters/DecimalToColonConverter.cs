using Microsoft.UI.Xaml.Data;
using MiniPos.Constants;
using System;
using System.Globalization;

namespace MiniPos;

public partial class DecimalToColonConverter : IValueConverter
{    
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is decimal decimalValue)
        {
            return AppConstants.MoneySymbol + decimalValue.ToString(AppConstants.DefaultDecimalFormat, CultureInfo.CurrentCulture);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if(value is string strValue && strValue.StartsWith(AppConstants.MoneySymbol))
        {
            strValue = strValue.Substring(2); // Remove the currency symbol

            if (decimal.TryParse(strValue, out decimal result))
            {
                return result;
            }
        }

        return value;
    }
}
