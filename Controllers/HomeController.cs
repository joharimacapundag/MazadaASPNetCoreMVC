using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mazada.Models;
using Mazada.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mazada.Controllers;

public class HomeController : Controller
{

    private MvcAppDbContext _context;

    public HomeController(MvcAppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Check if db has a featured products
        var featuredProducts = await _context.FeaturedProduct
            .Include(f => f.Product)
            .Include(f => f.Section)
            .OrderBy(f => f.Section!.Order) // Sort the section
            .ThenBy(f => f.Priority) // and sort by priority within that section
            .ToListAsync();

        if (featuredProducts.IsNullOrEmpty())
        {
            return View(); // 
        }

        // Map the list to the HomeViewModel
        var homeViewModel = new HomeViewModel
        {
            SectionViewModels = featuredProducts
            .GroupBy(f => f.Section) // Create groups by section
            .Select(group => new SectionViewModel
            {
                Name = group.Key?.Name,
                // Map the feature products to featured product view models
                FeaturedProductViewModels = group.Select(f => new FeaturedProductViewModel
                {
                    ProductId = f.Product?.Id ?? 0,
                    ProductName = f.Product?.Name,
                    ProductShortDescription = f.Product?.ShortDescription ?? "",
                    ProductStocks = f.Product?.Stocks ?? 0,
                    ProductPrice = f.Product?.Price ?? 0,
                    ProductImagePath = f.Product?.ImagePath ?? "",
                    Priority = f.Priority
                }).ToList()
            }).ToList()
        };


        return View(homeViewModel);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
