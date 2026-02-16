using Mazada.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mazada.Controllers;

public class LogoutController : Controller
{
    [Route("logout")]
    public IActionResult Logout()
    {
        // TODO: Delete all cookies
        foreach(var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }
        HttpContext.Session.Clear();
        return Ok(new { message = "Logout successfully!", redirectUrl = Url.Action("index", "home")});
    }   
}