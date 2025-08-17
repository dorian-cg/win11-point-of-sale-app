using System.Globalization;

namespace MiniPos.Constants;

public static class AppConstants
{
    public const string DefaultProductCode = "#####";
    public const string DefaultProductName = "******";
    public const decimal DefaultProductUnitPrice = 0.0m;
    public const int DefaultProductQuantity = 1;
    public const decimal DefaultProductTotalPrice = 0.0m;
    public const decimal DefaultReceiptTotal = 0.0m;
    public static string DefaultDecimalFormat = "N2";
    public static string MoneySymbol => CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
    public const string MultipleSymbol = "x"; // Prefix for quantities
}
