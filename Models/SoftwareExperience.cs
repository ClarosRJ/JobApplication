using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplication.Models
{
    public class SoftwareExperience
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        public virtual Applicant Applicant { get; private set; }
       
        [ForeignKey("Softwares")]
        public int SoftwareId { get; set; }
        public virtual Software Softwares { get; private set; }
        
        [Range(1, 10, ErrorMessage = "Select Your Raiting out of 10, 10 is the best. 1 is Poor")]
        public int Rating { get; set; }
        [StringLength(50)]
        public string Notes { get; set; }
        [NotMapped]
        public bool IsHidden{ get; set; } = false;
    }
}
