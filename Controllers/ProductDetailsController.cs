using Mazada.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Mazada.Controllers;

[Route("product")]
public class ProductDetailsController : ProductController
{
    private readonly MvcAppDbContext _context;
    public ProductDetailsController(MvcAppDbContext context)
    {
        _context = context;
    }
    [Route("details")]
    public async  Task<IActionResult> Details()
    {
        // TODO: Add product id null checker
        var id = Request.Query["id"];
        if (!int.TryParse(id, out var productId))
        {
            return BadRequest(new { message = "Product id is empty!"});
        }

        // TODO: Add product exist checker
        var product = await _context.Product.FirstOrDefaultAsync(product => product.Id == productId);
        if (product == null)
        {
            return NotFound(new { message = "Product is not found!"});
        }

        return ProductView(product);
    }
    
}