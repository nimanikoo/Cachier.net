using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCaching.Models;

public class Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(3), MaxLength(250)]
    public string CustomerName { get; set; }
    [Required] public int CustomerNo { get; set; }
}