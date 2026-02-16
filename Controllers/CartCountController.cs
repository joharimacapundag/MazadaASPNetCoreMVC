using Mazada.Data;
using Mazada.Extensions;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mazada.Controllers;

[Route("cart")]
public class CartCountController : Controller
{
    private readonly MvcAppDbContext _context;
    public CartCountController(MvcAppDbContext context)
    {
        _context = context;
    }
    [Route("count")]
    [HttpGet]
    public async Task<IActionResult> Count()
    {
        int dbCount = 0;

        // Check if is user logged on or not
        if (int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            // Sum all quantity that is tied with user
            dbCount = await _context.CartItem
                .Where(ci => ci.Cart!.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }
        else
        {
            // Sum all of each cart items quantity
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            dbCount = cartItems?.Sum(x => x.Quantity) ?? 0;
        }
        return Ok(new { count = dbCount});
    }
}