using Humanizer;

namespace Mazada.Models;

public class ProductViewModel : IProductViewModel
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductShortDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStocks { get; set; }
    public int ProductCategoryId { get; set; }
    public List<int>? ProductCategoryIds { get; set; }
    public string? ProductImagePath { get; set; }
    
}