using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;
public class ProductCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("product_id")]
    public int ProductId { get; set; }
    public Product? product { get; set; }

    [Required]
    [Column("category_id")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}