using System.Collections.Generic;
using MiniPos.Models;

namespace MiniPos.Services;

public class ProductsService
{
    #region static factory
    private static ProductsService? instance;
    public static ProductsService Instance
    {
        get
        {
            instance ??= new ProductsService();
            return instance;
        }
    }

    private ProductsService() {}
    #endregion

    private List<Product> productList = [];

    public ICollection<Product> GetProducts()
    {
        return productList;
    }

    public void AddProduct(Product product)
    {
        productList.Add(product);
    }

    public void LoadProductsFromDataSet(string[,] dataset, int headersRowIndex = 0)
    {
        for (int row = headersRowIndex + 1; row < dataset.GetLength(0); row++)
        {
            Product product = new();

            product.Code = dataset[row, 0];
            product.Name = dataset[row, 1];
            product.Price = decimal.Parse(dataset[row, 2]);

            AddProduct(product);
        }        
    }
}
