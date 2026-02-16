using System.Globalization;

namespace Mazada.Models;

public class CartItemViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductShortDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStocks { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => ProductPrice * Quantity;

}