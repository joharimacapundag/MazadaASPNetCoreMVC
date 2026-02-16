using Microsoft.AspNetCore.Mvc;

namespace Mazada.Controllers;

public class ProductController : Controller
{
    protected IActionResult ProductView(object? model = null)
    {
        // Get the action name what is the descriptor of the current Just redirecting to the folder 
        var actionName = ControllerContext.ActionDescriptor.ActionName;
        return View($"~/Views/Product/{actionName}.cshtml", model);
    }
}