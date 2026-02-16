using Mazada.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Mazada.Controllers;

public class LoginController : Controller
{
    private readonly MvcAppDbContext _context;
    public LoginController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("login")]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> LoginPost()
    {
        // TODO: Add username and password inputs empty checker
        var username = Request.Form["username"].ToString();
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest(new {field = "username", message = "Username field is empty!"});
        }
        // TODO: Add user checker if exist
        var user = await _context.User.FirstOrDefaultAsync(user => user.Username == username);
        if (user == null)
        {
            return BadRequest(new { field = "username", message = "User is not found!"});
        }
        // TODO: Add password verification
        var password = Request.Form["password"].ToString();
        if (string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new { field = "password", message = "Password field is empty!"});
        }
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return BadRequest(new { field = "password", message = "Wrong Password!"});
        }
        // Check if user is a seller or not
        var seller = await _context.Seller.FirstOrDefaultAsync(seller => seller.UserId == user.Id);
        if (seller != null)
        {
            Response.Cookies.Append("seller_id", seller.Id.ToString());
        }

        // TODO: Add user to cookies
        Response.Cookies.Append("user_id", user.Id.ToString());

        return Ok(new { message = "Login Successfully", redirectUrl = Url.Action("index", "home")});
    }
}