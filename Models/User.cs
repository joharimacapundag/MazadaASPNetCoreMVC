using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    public string? Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;
    

}