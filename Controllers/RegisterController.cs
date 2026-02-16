using System.Text.RegularExpressions;
using Mazada.Data;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mazada.Controllers;

public class RegisterController : Controller
{
    private readonly MvcAppDbContext _context;
    public RegisterController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("register")]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> RegisterPost()
    {
        // TODO: Add username, password inputs empty checker
        // TODO: Add user exist checker
        // TODO: Add username, password, confirm password validation
        var username = Request.Form["username"].ToString();
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest(new { field = "username", message = "Username field is empty!"});
        }
        var existingUser = await _context.User.FirstOrDefaultAsync(user => user.Username == username);
        if (existingUser != null)
        {
            return BadRequest(new { field = "username", message = "User exists!"});
        }
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]{3,20}$")) 
        {
            return BadRequest(new { field = "username", message = "Username must be 3-20 characters, letters/numbers/underscores only!" });
        }
        ///////////////////////////////////////////// PASSWORD /////////////////////////////////////////////////////////////////
        var password = Request.Form["password"].ToString();
        if (string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new { field = "password", message = "Password field is empty!"});
        }
        if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
        {
            return BadRequest(new { field = "password", message = "Password must be at least 8 characters, including uppercase, lowercase, number, and special character!" });
        }
        ///////////////////////////////////////////// CONFIRM PASSWORD /////////////////////////////////////////////////////////////////
        var confirmPassword = Request.Form["confirm-password"].ToString();
        if (confirmPassword != password)
        {
            return BadRequest(new { field = "confirm-password", message = "Password do not match!"});
        }

        // TODO: Add user to database
        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) // I use brcrypt hashing because more simple than the other one
        };
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();


        // TODO: Add user to cookies
        Response.Cookies.Append("user_id", user.Id.ToString());

        return Ok(new { message = "Registered Successfully!", redirectUrl = Url.Action("index", "home")});
    }
}