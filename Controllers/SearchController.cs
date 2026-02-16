using Mazada.Data;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mazada.Controllers;

public class SearchController : Controller
{
    private readonly MvcAppDbContext _context;
    public SearchController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("search")]
    public async Task<IActionResult> Search()
    {
        // TODO: Add query null checker
        var query = Request.Query["query"].ToString();
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { message = "Search field is empty!"});
        }

        // TODO: Add products null checker
        var products = await _context.Product.Include(p => p.ProductCategories).Where( product => EF.Functions.Like(product.Name,  $"%{query}%")).ToArrayAsync();
        if (products.IsNullOrEmpty())
        {
            return BadRequest(new { message = "No product/s found!"});
        }

        // Map products to productViewModels
        var productViewModels = products.Select(product => new ProductViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            ProductShortDescription = product.ShortDescription,
            // ProductCategoryId = p.ProductCategory?.Id ?? 0,
            ProductCategoryIds = product.ProductCategories?.Select(pc => pc.CategoryId).ToList() ?? [],
            ProductPrice = product.Price,
            ProductImagePath = product.ImagePath,
            ProductStocks = product.Stocks
        }).ToArray();

        return View(productViewModels);
    }
}