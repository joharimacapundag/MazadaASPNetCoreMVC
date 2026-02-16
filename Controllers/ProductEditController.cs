using Mazada.Data;
using Mazada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mazada.Controllers;

[Route("product")]
public class ProductEditController : ProductController
{
    private readonly MvcAppDbContext _context;
    public ProductEditController(MvcAppDbContext context)
    {
        _context = context;
    }
    [Route("edit")]
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        // TODO: Add product id null checker
        var id = Request.Query["id"];
        if (!int.TryParse(id, out var productId))
        {
            return BadRequest(new { success = false, message = "Product id is empty!" });
        }

        // TODO: Add product exist checker
        var product = await _context.Product.Include(product => product.Seller).FirstOrDefaultAsync(product => product.Id == productId);
        if (product == null)
        {
            return NotFound(new { success = false, message = "Product is not found!" });
        }

        // Map product to shop product edit view model
        var shopProductViewModel = new ShopProductEditViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            ProductShortDescription = product.ShortDescription,
            ProductLongDescription = product.LongDescription,
            ProductImagePath = product.ImagePath,
            ProductPrice = product.Price,
            ProductStocks = product.Stocks,
            ShopName = product.Seller!.ShopName
        };

        return ProductView(shopProductViewModel);
    }

    [Route("edit")]
    [HttpPost]
    public async Task<IActionResult> EditPost()
    {

        // Check if the user is logged on
         if (!int.TryParse(Request.Cookies["user_id"], out int userId))
        {
            return BadRequest(new { success = false, message = "User not found!" });
        }

        // Check if the user is seller
        var seller = await _context.Seller.FirstOrDefaultAsync(seller => seller.UserId == userId);
        if (seller == null)
        {
            return BadRequest(new { success = false, message = "User is not a seller" });
        }

        // Check the if the product id is null
        if (!int.TryParse(Request.Form["id"], out int productId))
        {
            return BadRequest(new { message = "Product is invalid!" });
        }

        // Check the product if exists in the seller's database
        var product = await _context.Product.FirstOrDefaultAsync(product => product.Id == productId && product.SellerId == seller.Id);
        if (product == null)
        {
            return BadRequest(new { message = "Product is not exists!" });
        }

        // Check if the product name is null
        var productName = Request.Form["name"].ToString();
        if (string.IsNullOrWhiteSpace(productName))
        {
            return BadRequest(new { field = "name", message = "Product name field is empty!" });
        }

        // Validate the product name
        if (productName.Length < 3 || productName.Length > 100)
        {
            return BadRequest(new { field = "name", message = "Product name must be between 3 and 100 characters!" });
        }
        // Check if the product price is lesser than or equal to 0
        if (!decimal.TryParse(Request.Form["price"], out decimal price))
        {
            return BadRequest(new { field = "price",  message = "Price is not set!" });
        }
        if (price <= 0)
        {
            return BadRequest(new { field = "price", message = "Price must be greater than 0!" });
        }

        // Check if the stocks is valid or not
        if (!int.TryParse(Request.Form["stocks"], out int stocks))
        {
            return BadRequest(new { field = "stocks", message = "Stocks is not set!" });
        }
        if (stocks <= 0)
        {
            return BadRequest(new { field = "stocks", message = "Stocks must be greater than 0!" });
        }

        // Check if the product short description is null
        var shortDesc = Request.Form["short_description"].ToString();
        if (string.IsNullOrWhiteSpace(shortDesc))
        {
            return BadRequest(new { field = "short_description", message = "Short description field is empty!" });
        }
        // Validate the product short description
        if (shortDesc.Length < 5 || shortDesc.Length > 200)
        {
            return BadRequest(new { field = "short_description", message = "Short description must be between 5 and 200 characters!" });
        }

        // WIP: Check if the product long description is null
        var longDesc = Request.Form["long_description"].ToString();
        if (string.IsNullOrWhiteSpace(longDesc))
        {
            //return BadRequest(new { field = "long_description",  message = "Long description field is empty!" });
        }
        // WIP: Validate the product long description
        if (longDesc.Length < 10 || longDesc.Length > 2000)
        {
            //return BadRequest(new { field = "long_description", message = "Long description must be between 10 and 2000 characters!" });
        }

        // Validate the uploaded product image
        var imageFile = Request.Form.Files["image_path"];

        var imagePath = product.ImagePath;
        // Check if the image is not null and file size greater than 0
        if (imageFile != null && imageFile.Length > 0)
        {
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
            imagePath = "/uploads/" + imageFile.FileName;
        }

        // Update the product database
        product.Name = productName;
        product.Price = price;
        product.Stocks = stocks;
        product.ShortDescription = shortDesc;
        product.LongDescription = longDesc; // WIP: needs rich text editor
        product.ImagePath = imagePath;
        // TODO: Add Product Categories Edit


        _context.Product.Update(product);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Saved successfully!" });
    }
}