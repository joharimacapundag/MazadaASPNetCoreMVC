namespace Mazada.Models;

public interface IProductViewModel
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductShortDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStocks { get; set; }
    public string? ProductImagePath { get; set; }
}