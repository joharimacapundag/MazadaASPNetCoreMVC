using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;

public class CartItem
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("cart_id")]
    public int CartId { get; set; } // FK Cart Id
    public Cart? Cart { get; set; } 

    [Required]
    [Column("product_id")]
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }


    
}