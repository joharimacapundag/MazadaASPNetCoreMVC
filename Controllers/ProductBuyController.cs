using Mazada.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mazada.Controllers;

[Route("product")]
public class ProductBuyController : ProductController
{
    [Route("buy")]
    [HttpGet]
    public IActionResult Buy()
    {
        return ProductView();
    }

    [Route("buy")]
    [HttpPost]
    public IActionResult BuyPost()
    {
        return Ok(new { message = "Checkout Successfully!"});
    }
}