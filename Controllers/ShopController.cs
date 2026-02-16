using Mazada.Data;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mazada.Controllers;

public class ShopController : Controller
{
    private readonly MvcAppDbContext _context;
    public ShopController(MvcAppDbContext context)
    {
        _context = context;
    }
    [Route("shop")]
    [HttpGet]
    public async Task<IActionResult> Shop()
    {
        // Check the user id if null
        if (!int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            return Unauthorized(new { message = "User is not logged on!" });
        }

        // Check the user id if exist in db
        var user = await _context.User.FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            return NotFound(new { message = "User is not found!" });
        }

        // Check if the user is a seller
        var seller = await _context.Seller.FirstOrDefaultAsync(seller => seller.UserId == user.Id);
        if (seller == null)
        {
            return NotFound(new { message = "User is not a seller!" });
        }

        // Check if the user's shop has product/s
        var shop = await _context.Seller.Include(s => s.Products!).ThenInclude(p => p.ProductCategories).FirstOrDefaultAsync(s => s.Id == seller.Id);
        if (shop == null || shop.Products.IsNullOrEmpty())
        {
            return NotFound(new { message = "Shop has no products!" });
        }

        // Map product's categories
        var categoryViewModels = await _context.Category
            .Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            }).ToListAsync();


        // Map user's shop product/s to shop product view models
        var productViewModels = shop.Products?
            .Select(product => new ShopProductViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductShortDescription = product.ShortDescription,
                ProductPrice = product.Price,
                ProductStocks = product.Stocks,
                ProductImagePath = product.ImagePath,
                // ProductCategoryId = product.ProductCategory?.CategoryId ?? 0,
                ProductCategoryIds = product.ProductCategories?.Select(pc => pc.CategoryId).ToList() ?? [],
            }).ToList();

        // Map into shop view model
        var shopViewModel = new ShopViewModel
        {
            ShopName = seller.ShopName,
            Categories = categoryViewModels,
            Products = productViewModels,
        };

        return View(shopViewModel);
    }


}