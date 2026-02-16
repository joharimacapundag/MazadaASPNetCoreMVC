using Mazada.Data;
using Mazada.Extensions;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mazada.Controllers;

[Route("cart")]
public class CartRemoveController : Controller
{
    private readonly MvcAppDbContext _context;
    public CartRemoveController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("remove")]
    public async Task<IActionResult> Remove()
    {
        // Check if the cart id is null
        if (!int.TryParse(Request.Query["id"], out int cartItemId))
        {
            return BadRequest(new { message = "Cart id is invalid!" });
        }

        // Try parse the user id to int
        int.TryParse(Request.Cookies["user_id"], out int userId);

        // Check if user exists in db
        var user = await _context.User.FirstOrDefaultAsync(user => user.Id == userId);

        // Check if user is log on
        if (user == null)
        {
            // Delete if there's contains user id in the cookies left
            if (Request.Cookies.ContainsKey("user_id"))
            {
                Response.Cookies.Delete("user_id");
            }

            // Check if cart doesnt exist in the session
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            if (cartItems == null)
            {
                return BadRequest(new { message = "Cart is not existing!" });
            }

            // Check if cart item doesn't exist in the cart
            var cartItem = cartItems.FirstOrDefault(cartItem => cartItem.Id == cartItemId);
            if (cartItem == null)
            {
                return BadRequest(new { message = "Cart item is not existing!" });
            }

            // Remove the cart item from the session cart
            cartItems.Remove(cartItem);

            // Update the session cart
            HttpContext.Session.SetObjectAsJson("cart", cartItems);

            return Ok(new { message = "Remove successfully!" });
        }

        // Check if cart item id exists in user's cart
        var deletedCount = await _context.CartItem
            .Where(cartItem => cartItem.Id == cartItemId && cartItem.Cart!.UserId == userId)
            .ExecuteDeleteAsync();
        
        if (deletedCount == 0)
        {
            return NotFound(new { message = "Cart item does not exist!" });
        }

        // await _context.SaveChangesAsync();

        return Ok(new { message = "Remove successfully!" });
    }
}