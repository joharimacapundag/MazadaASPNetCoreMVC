namespace Mazada.Models;

public class ShopViewModel
{
    public string? ShopName { get; set; }
    public List<CategoryViewModel>? Categories { get; set;}
    public List<ShopProductViewModel>? Products { get; set; }
}