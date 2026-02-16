using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;

public class FeaturedProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("product_id")]
    public int ProductId { get; set; }
    public Product? Product {get; set; }
    
    [Column("priority")]
    public int Priority { get; set; }

    [Required]
    [Column("section_id")]
    public int SectionId { get; set; }
    public Section? Section { get; set; }


}