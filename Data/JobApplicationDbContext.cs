using JobApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Data
{
	public class JobApplicationDbContext:DbContext
	{
		public JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options) : base(options)
		{
		
		}
		public virtual DbSet<Applicant> Applicants { get; set; }
		public virtual DbSet<Experience> Experiences { get; set; }
		public virtual DbSet<Software> Softwares { get; set; }
		public virtual DbSet<SoftwareExperience> SoftwareExperiences { get; set; }
	}
}
