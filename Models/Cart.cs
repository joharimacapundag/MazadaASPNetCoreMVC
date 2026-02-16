using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;

public class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [ForeignKey("user_id")]
    public int UserId { get; set; } // FK User id
    public User? User { get; set; } 
    public List<CartItem>? CartItems { get; set; } 
    
}