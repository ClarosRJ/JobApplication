using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplication.Models
{
    public class Applicant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        [DisplayName("Nombre")]
        public string Name { get; set; } = "";
        [Required]
        [StringLength(10)]
        [DisplayName("Genero")]
        public string Gender { get; set; } = "";
        [Required]
        [Range(25, 55, ErrorMessage = "Currently, We don´t have on positions vacant for your age.")]
        [DisplayName("Edad en Años")]
        public int Age { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Habilidades")]
        public string Qualification { get; set; } = "";
        [Required]
        [Range(1, 55, ErrorMessage = "Currently, We have on positions vacant for your Experience.")]
        [DisplayName("Experiencia en Años")]
        public int TotalExperience { get; set; }

        public virtual List<Experience> Experiences { get; set; } = new List<Experience>(); 
        public virtual List<SoftwareExperience> SoftwareExperiences { get; set; } = new List<SoftwareExperience>(); 

        public string PhotoUrl { get; set; }
        [Display(Name = "Foto de perfil")]
        [NotMapped]
        public IFormFile profilephoto { get; set; }
    }
}
