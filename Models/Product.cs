using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("name")]
    public string? Name { get; set; }
    [Column("short_description")]
    public string? ShortDescription { get; set; }
    [Column("long_description")]
    public string? LongDescription { get; set; }
    [Required]
    [Column("price")]
    public decimal Price { get; set; }
    [Column("stocks")]
    public int Stocks { get; set; }
    [Column("image_path")]
    public string? ImagePath { get; set; }
    [Column("seller_id")]
    public int SellerId { get; set; } // FK Seller Id
    public Seller? Seller { get; set; }
    public List<ProductCategory>? ProductCategories { get; set; }
    
}