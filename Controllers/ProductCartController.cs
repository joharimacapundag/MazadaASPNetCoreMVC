using Mazada.Data;
using Mazada.Extensions;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mazada.Controllers;


[Route("product")]
public class ProductCartController : Controller
{
    private readonly MvcAppDbContext _context;
    public ProductCartController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("cart")]
    public async Task<IActionResult> Cart()
    {
        // TODO: Add product id null checker
        if (!int.TryParse(Request.Query["id"], out int productId))
        {
            return BadRequest(new { message = "Product id is invalid!" });
        }
        // TODO: Add quantity validation
        if (!int.TryParse(Request.Query["quantity"], out int quantity))
        {
            return BadRequest(new { message = "Quantity is invalid!" });
        }

        // TODO: Add product exist checker
        var product = await _context.Product.FirstOrDefaultAsync(product => product.Id == productId);
        if (product == null)
        {
            return BadRequest(new { message = "Product is not existing!" });
        }

        // TODO: Add user log on checker
        if (!int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            // Add temporary cart when not log on
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart") ?? new List<CartItem>();

            // Check if product exist in temporary cart checker
            var cartItem = cartItems.FirstOrDefault(cartItem => cartItem.ProductId == productId);

            // Get the highest existing Id or 0 if cart items is empty 
            var nextId = cartItems.Any() ? cartItems.Max(item => item.Id) + 1 : 1;

            // Add product to temporary cart
            if (cartItem == null)
            {
                cartItems.Add(new CartItem
                {
                    Id = nextId,
                    ProductId = productId,
                    Product = new Product
                    {
                        Name = product.Name,
                        ShortDescription = product.ShortDescription,
                        LongDescription = product.LongDescription,
                        Price = product.Price,
                        Stocks = product.Stocks,
                        ImagePath = product.ImagePath,
                        SellerId = product.SellerId
                    },
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity = quantity;
            }
            HttpContext.Session.SetObjectAsJson("cart", cartItems);

            return Ok(new { message = "Item added to the cart successfully!" });
        }
        // TODO: Add item to user's cart(db) when log on

        // Check user id if exists
        var user = await _context.User.FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            return NotFound(new { message = "User is not found!" });
        }

        // Add user's cart if not exists
        var dbCart = await _context.Cart.FirstOrDefaultAsync(cart => cart.UserId == userId);
        if (dbCart == null)
        {
            dbCart = new Cart { UserId = userId };
            _context.Cart.Add(dbCart);
            await _context.SaveChangesAsync();

        }

        // Check the cart item exists in user's cart
        var cartId = dbCart.Id;
        var dbCartItem = await _context.CartItem.FirstOrDefaultAsync(cartItem => cartItem.CartId == cartId && cartItem.ProductId == productId);
        if (dbCartItem == null)
        {
            // Check the product exist in db
            var cartItem = new CartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };
            _context.CartItem.Add(cartItem);
        }
        else
        {
            // Set the cart item quantity to input quantity
            dbCartItem.Quantity = quantity;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Item added to the cart successfully!" });
    }
}