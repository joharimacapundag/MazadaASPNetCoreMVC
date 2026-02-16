using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mazada.Models;

public class Category{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id {get; set;}
    [Required]
    [Column("name")]
    public string? Name { get; set; }

}