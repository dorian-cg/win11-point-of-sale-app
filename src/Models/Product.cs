namespace MiniPos.Models;

public partial class Product
{    
    public string Code { get; set; } = string.Empty;
 
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0.0m;

    public Product(){}

    public Product(string code, string name, decimal price)
    {
        Code = code;
        Name = name;
        Price = price;
    }
}
