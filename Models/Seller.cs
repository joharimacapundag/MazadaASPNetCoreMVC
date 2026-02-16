using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;

public class Seller
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    [Column("email")]
    public string? Email { get; set; }
    
    [Required]
    [DataType(DataType.PhoneNumber)]
    [Column("contact_number")]
    public string? ContactNumber { get; set; }

    [Required]
    [Column("shop_name")]
    public string? ShopName { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; } // FK user id
    public User? User { get; set; }
    public List<Product>? Products { get; set; }
}