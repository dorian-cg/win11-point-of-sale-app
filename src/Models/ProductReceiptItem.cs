namespace MiniPos.Models;

public partial class ProductReceiptItem : Product
{    
    public int Quantity { get; set; } = 1;
    
    public decimal TotalPrice => Price * Quantity;

    public ProductReceiptItem() {}

    public ProductReceiptItem(Product product, int quantity = 1) : base(product.Code, product.Name, product.Price)
    {
        Quantity = quantity;
    }
}
