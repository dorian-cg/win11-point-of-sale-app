using MiniPos.Constants;
using MiniPos.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MiniPos.Services;

public class ProductReceiptService
{
    #region static factory
    private static ProductReceiptService? instance;
    public static ProductReceiptService Instance
    {
        get
        {
            instance ??= new ProductReceiptService();
            return instance;
        }
    }
    private ProductReceiptService() { }
    #endregion

    public string[,] BuildDataSet(Collection<ProductReceiptItem> productReceiptItemList, bool includeHeaders = true)
    {
        var cols = 5; // there are 2 properties in ProductReceiptItem and 3 in Product base class
        var rows = productReceiptItemList.Count() + (includeHeaders ? 1 : 0);
        
        var dataset = new string[rows, cols];

        if(includeHeaders)
        {
            dataset[0, 0] = "codigo";
            dataset[0, 1] = "nombre";
            dataset[0, 2] = "precio_unitario";
            dataset[0, 3] = "cantidad";
            dataset[0, 4] = "precio_total";
        }
        
        for (var row = includeHeaders ? 1 : 0; row < rows; row++)
        {
            var index = includeHeaders ? row - 1 : row;
            dataset[row, 0] = productReceiptItemList[index].Code;
            dataset[row, 1] = productReceiptItemList[index].Name;
            dataset[row, 2] = productReceiptItemList[index].Price.ToString(AppConstants.DefaultDecimalFormat, CultureInfo.CurrentCulture);
            dataset[row, 3] = productReceiptItemList[index].Quantity.ToString();
            dataset[row, 4] = productReceiptItemList[index].Price.ToString(AppConstants.DefaultDecimalFormat, CultureInfo.CurrentCulture);
        }

        return dataset;
    }
}
