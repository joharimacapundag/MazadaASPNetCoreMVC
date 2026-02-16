using Mazada.Data;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mazada.Controllers;

public class SellController : Controller
{
    private readonly MvcAppDbContext _context;
    public SellController(MvcAppDbContext context)
    {
        _context = context;
    }

    [Route("sell")]
    [HttpGet]
    public async Task<IActionResult> Sell()
    {
        var categoryViewModels = await _context.Category.Select(category => new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name
        }).ToArrayAsync();
        return View(categoryViewModels);
    }

    [Route("sell/product")]
    [HttpPost]
    public async Task<IActionResult> Product()
    {
        // TODO: Add user id null checker
        if (!int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            return Unauthorized(new { message = "User not logged in!" });
        }

        // TODO: Add user is already seller checker and null checker
        var seller = await _context.Seller.FirstOrDefaultAsync(seller => seller.UserId == userId);
        if (seller == null)
        {
            return NotFound(new { success = false, message = "User is not a seller" });
        }

        // TODO: Add product name, short desc, long desc, price, stocks, image path null checker
        // TODO: Add product name, short desc, long desc, price, stocks validation
        var productName = Request.Form["name"].ToString();
        if (string.IsNullOrWhiteSpace(productName))
        {
            return BadRequest(new { field = "name", message = "Product name field is empty!" });
        }
        if (productName.Length < 3 || productName.Length > 100)
        {
            return BadRequest(new { field = "name", message = "Product name must be between 3 and 100 characters!" });
        }
        ///////////////////////////////////////////// SHORT DESCRIPTION /////////////////////////////////////////////////////////////////
        var shortDesc = Request.Form["short_description"].ToString();
        if (string.IsNullOrWhiteSpace(shortDesc))
        {
            return BadRequest(new { field = "short_description", message = "Short description field is empty!" });
        }
        if (shortDesc.Length < 5 || shortDesc.Length > 200)
        {
            return BadRequest(new { field = "short_description", message = "Short description must be between 5 and 200 characters!" });
        }
        ////////////////////////////////////////////////// LONG DESCRIPTION ////////////////////////////////////////////////////////////
        var longDesc = Request.Form["long_description"].ToString();
        if (string.IsNullOrWhiteSpace(longDesc))
        {
            return BadRequest(new { field = "long_description", message = "Long description field is empty!" });
        }
        if (longDesc.Length < 10 || longDesc.Length > 2000)
        {
            return BadRequest(new { field = "long_description", message = "Long description must be between 10 and 2000 characters!" });
        }

        ///////////////////////////////////////////// PRICE /////////////////////////////////////////////////////////////////
        if (!decimal.TryParse(Request.Form["price"], out decimal price))
        {
            return BadRequest(new { field = "price", message = "Price is not set!" });
        }
        if (price <= 0)
        {
            return BadRequest(new { field = "price", message = "Price must be greater than 0!" });
        }
        /////////////////////////////////////////// STOCKS ///////////////////////////////////////////////////////////////////
        if (!int.TryParse(Request.Form["stocks"], out int stocks))
        {
            return BadRequest(new { field = "stocks", message = "Stocks is not set!" });
        }
        if (stocks <= 0)
        {
            return BadRequest(new { field = "stocks", message = "Stocks must be greater than 0!" });
        }
        ////////////////////////////////////////////////// CATEGORY ////////////////////////////////////////////////////////////

        // Get the categories from user's selected categories
        var categories = Request.Form["categories"].ToArray();

        // Convert the request form array elements to integer
        var categoryIds = new List<int>();
        foreach (var category in categories)
        {
            if (int.TryParse(category, out int id))
            {
                categoryIds.Add(id);
            }
        }

        // Check if the categories is empty or not
        if (categoryIds.IsNullOrEmpty())
        {
            return BadRequest(new { field = "category", message = "Please select at least one category!" });
        }

        // Get the count of the elements that is in category db from the list 
        var dbCategoryCount = await _context.Category
            .CountAsync(category => categoryIds.Contains(category.Id));

        // Check if it same length/count between the categoryIds and this dbcategorycount
        if (dbCategoryCount != categoryIds.Count)
        {
            return BadRequest(new { field = "category", message = "Some are invalid categories!" });
        }

        //////////////////////////////////////// IMAGE PATH //////////////////////////////////////////////////////////////////////
        var imageFile = Request.Form.Files["image_path"];
        if (imageFile == null)
        {
            return BadRequest(new { field = "image_path", message = "No file uploaded!" });
        }
        // Create folder directory if not exist
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Check the file extension if it is an image file
        var fileName = Path.GetFileName(imageFile.FileName);
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        if (!allowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
        {
            return BadRequest(new { field = "image_path", message = "Invalid file type!" });
        }

        // Create file path where to upload
        var filePath = Path.Combine(uploadsFolder, imageFile.FileName);
        // Dispose stream after copying the image to the file path
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        // Add the image actual path after uploaded
        var imagePath = "/uploads/" + imageFile.FileName;

        // TODO: Add product to database
        var product = new Product
        {
            Name = productName,
            ShortDescription = shortDesc,
            LongDescription = longDesc,
            Price = price,
            Stocks = stocks,
            ImagePath = imagePath,
            SellerId = seller.Id,
        };


        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();

        // Map product categories to categoryIds
        var productCategories = categoryIds
            .Select(categoryId => new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = categoryId
            }).ToList();

        // Add the list to the database
        await _context.ProductCategory.AddRangeAsync(productCategories);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Product Added Successfully!", redirectUrl = Url.Action("index", "home") });
    }
}