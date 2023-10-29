using System.ComponentModel.DataAnnotations;

namespace JobApplication.Models
{
    public class Software
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = "";
    }
}
