using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiCashing.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3),MaxLength(250)]
        public string CustomerName { get; set; }

        [Required]
        public int CustomerNo { get; set; }

    }
}
