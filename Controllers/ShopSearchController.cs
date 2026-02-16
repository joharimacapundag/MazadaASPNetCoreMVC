using Mazada.Data;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mazada.Controllers;

[Route("shop")]
public class ShopSearchController : Controller
{
    private readonly MvcAppDbContext _context;
    public ShopSearchController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("search")]
    [HttpGet]
    public async Task<IActionResult> Search()
    {

        // Check the user id if null
        if (!int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            return Unauthorized(new { message = "User is not logged on!" });
        }

        // Check if the user is a seller
        var seller = await _context.Seller.FirstOrDefaultAsync(seller => seller.UserId == userId);
        if (seller == null)
        {
            return NotFound(new { message = "User is not a seller!" });
        }

        // Get the query
        var query = Request.Query["query"].ToString();

        // Ready the db query
        var queryable = _context.Product
            .Include(p => p.ProductCategories)
            .Where(p => p.SellerId == seller.Id);

        // Check if query is empty or white space
        if (!string.IsNullOrWhiteSpace(query))
        {
            queryable = queryable
                .Include(product => product.ProductCategories)
                .Where(
                    product => product.SellerId == seller.Id && 
                    EF.Functions.Like(product.Name,  $"%{query}%")
                );
        }

        // Check if the product/s exists
        var products = await queryable.ToArrayAsync();
        
        if (products.IsNullOrEmpty())
        {
            return NotFound(new { message = "No products found!"});
        }

        
        // Map the products to the product view models
        var shopProductViewModels = products.Select(product => new ShopProductViewModel
        {
            ProductId = product.Id,
            // ProductCategoryId = product.ProductCategory?.CategoryId ?? 0,
            ProductCategoryIds = product.ProductCategories?.Select(pc => pc.CategoryId).ToList() ?? [],
            ProductName = product.Name,
            ProductImagePath = product.ImagePath,
            ProductPrice = product.Price,
            ProductShortDescription = product.ShortDescription,
            ProductStocks = product.Stocks

        }).ToArray();

        return PartialView("_ProductGrid", shopProductViewModels);
    }
}