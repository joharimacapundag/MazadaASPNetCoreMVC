namespace Mazada.Models;

public class FeaturedProductViewModel : IProductViewModel
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductShortDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStocks { get; set; }
    public string? ProductImagePath { get; set; }
    public int Priority { get; set; }
}