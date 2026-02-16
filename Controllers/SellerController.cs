using System.Text.RegularExpressions;
using Mazada.Data;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mazada.Controllers;

public class SellerController : Controller
{
    private readonly MvcAppDbContext _context;

    public SellerController(MvcAppDbContext context)
    {
        _context = context;
    }
    [Route("seller")]
    [HttpGet]
    public IActionResult Seller()
    {
        return View();
    }

    [Route("/seller/register")]
    [HttpPost]
    public async Task<IActionResult> Register()
    {
        // TODO: Add Null Check for User Id
        var id = Request.Cookies["user_id"];
        if (!int.TryParse(id, out int userId))
        {
            return Unauthorized(new {  message = "User not logged in!" });
        }

        // TODO: Add email, contact number, shop name null checker
        // TODO: Add email, contact-number, shop-name validation
        var email = Request.Form["email"].ToString();
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { field = "email", message = "Email field is empty!" });
        }
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return BadRequest(new { field = "email", message = "Invalid email format!" });
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        var contactNumber = Request.Form["contact-number"].ToString();
        if (string.IsNullOrWhiteSpace(contactNumber))
        {
            return BadRequest(new { field = "contact-number", message = "Contact number field is empty!" });
        }
        if (!Regex.IsMatch(contactNumber, @"^\+639\d{9}$")) // Philippine Contact Number
        {
            return BadRequest(new { field = "contact-number", message = "Invalid ph contact number!" });
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        var shopName = Request.Form["shop-name"].ToString();
        if (string.IsNullOrWhiteSpace(shopName))
        {
            return BadRequest(new { field = "shop-name", message = "Shop name field is empty!" });
        }
        if (shopName.Length < 3 || shopName.Length > 50)
        {
            return BadRequest(new { field = "shop-name", message = "Shop name must be between 3 and 50 characters!" });
        }

        // TODO: Add user is already seller checker
        var existingSeller = _context.Seller.FirstOrDefault(seller => seller.UserId == userId);
        if (existingSeller != null)
        {
            return BadRequest(new { message = "User is already a seller!" });
        }

        // TODO: Add new Seller to database
        var seller = new Seller
        {
            UserId = userId,
            Email = email,
            ContactNumber = contactNumber,
            ShopName = shopName
        };
        await _context.Seller.AddAsync(seller);
        await _context.SaveChangesAsync();

        // TODO: Add seller to cookies
        Response.Cookies.Append("seller_id", seller.Id.ToString());

        return Ok(new { message = "Registered Complete!", redirectUrl = Url.Action("index", "home") });
    }

}