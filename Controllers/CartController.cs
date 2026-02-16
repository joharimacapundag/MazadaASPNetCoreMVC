using Mazada.Data;
using Mazada.Extensions;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mazada.Controllers;

public class CartController : Controller
{
    private readonly MvcAppDbContext _context;
    public CartController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("cart")]
    public async Task<IActionResult> Cart()
    {
        // Check if user is log on or not
        if (!int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            // Add temporary cart for user not log on and don't have temporary cart yet
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart") ?? [];
            HttpContext.Session.SetObjectAsJson("cart", cartItems);
            var cartItemViewModels = cartItems.Select(cartItem => new CartItemViewModel
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product?.Name,
                ProductShortDescription = cartItem.Product?.ShortDescription,
                ProductPrice = cartItem.Product?.Price ?? 0,
                ProductStocks = cartItem.Product?.Stocks ?? 0,
                Quantity = cartItem.Quantity
            });

            return View(cartItemViewModels.ToArray());
        }

        // Check if user is exist in db
        var user = await _context.User.FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            return NotFound(new { message = "User is not found!" });
        }

        // Get user's cart with cart items(products) (join)
        var cart = await _context.Cart
            .Include(c => c.CartItems!)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        // Create empty cart if doesn't exist
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Convert/Map the cart items to cart item view model
        var dbCartItemViewModels = cart.CartItems?
            .Select(ci => new CartItemViewModel
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name,
                ProductShortDescription = ci.Product?.ShortDescription,
                ProductPrice = ci.Product?.Price ?? 0,
                ProductStocks = ci.Product?.Stocks ?? 0,
                Quantity = ci.Quantity
            })
            .ToArray() ?? [];
        
        return View(dbCartItemViewModels);
    }
}