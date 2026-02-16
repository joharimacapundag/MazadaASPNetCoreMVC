using Microsoft.AspNetCore.Mvc;
using Mazada.Data;
using Mazada.Extensions;
using Mazada.Models;
using Microsoft.EntityFrameworkCore;

namespace Mazada.ViewComponents;

public class CartBadgeViewComponent : ViewComponent
{
    private readonly MvcAppDbContext _context;

    public CartBadgeViewComponent(MvcAppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        int count = 0;

        // Check if is user logged on or not
        if (int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            // Sum all quantity that is tied with user
            count = await _context.CartItem
                .Where(ci => ci.Cart!.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }
        else
        {
            // Sum all of each cart items quantity
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            count = cartItems?.Sum(x => x.Quantity) ?? 0;
        }

        return View("~/Views/Shared/_CartBadge.cshtml", count); 
    }
}