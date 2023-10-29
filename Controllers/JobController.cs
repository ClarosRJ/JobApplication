using JobApplication.Data;
using JobApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Controllers
{
    public class JobController : Controller
    {
        private readonly JobApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public JobController(JobApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public IActionResult Index()
        {
            List<Applicant> applicants;
            applicants = _context.Applicants.ToList();
            return View(applicants);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.Experiences.Add(new Experience() { ExperienceId = 1 });
            ViewBag.Gender = GetGender();

            applicant.SoftwareExperiences.Add(new SoftwareExperience() { Id = 1 });
            ViewBag.Rating = GetRating();
            ViewBag.Softwares = GetSoftware();

            return View(applicant);
        }
        [HttpPost]
        public IActionResult Create(Applicant applicant)
        {
            if (applicant.SoftwareExperiences != null)
            {
                // Recorre todas las experiencias en la colección
                foreach (var softwareExperience in applicant.SoftwareExperiences)
                {
                    // Verifica si Notes es nulo o vacío
                    if (string.IsNullOrEmpty(softwareExperience.Notes))
                    {
                        // Establece el valor predeterminado
                        softwareExperience.Notes = "Vacio";
                    }
                }
            }

            applicant.Experiences.RemoveAll(x => x.YearsWorked == 0);
            applicant.Experiences.RemoveAll(n => n.IsDeleted == true);

            string uniqueFileName = GetUploadedFileName(applicant);
            applicant.PhotoUrl = uniqueFileName;

            _context.Add(applicant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Applicant applicant = _context.Applicants
                .Include(e => e.Experiences)
                .Include(s => s.SoftwareExperiences)
                .Where(a => a.Id == id).FirstOrDefault()!;

            ViewBag.Gender = GetGender();

            ViewBag.Rating = GetRating();
            ViewBag.Softwares = GetSoftware();

            return View(applicant);

        }
        [HttpPost]
        public IActionResult Edit(Applicant applicant)
        {
            if (applicant.SoftwareExperiences.Count > 0 && string.IsNullOrEmpty(applicant.SoftwareExperiences[0].Notes))
            {
                applicant.SoftwareExperiences[0].Notes = "Valor predeterminado";
            }

            List<Experience> experiencesDet = _context.Experiences.Where(d => d.ApplicantId == applicant.Id).ToList();
            _context.Experiences.RemoveRange(experiencesDet);
            _context.SaveChanges();

            List<SoftwareExperience> softExpDetail = _context.SoftwareExperiences.Where(d => d.ApplicantId == applicant.Id).ToList();
            _context.SoftwareExperiences.RemoveRange(softExpDetail);
            _context.SaveChanges();

            applicant.Experiences.RemoveAll(n => n.YearsWorked == 0);
            applicant.Experiences.RemoveAll(n => n.IsDeleted == true);

            applicant.SoftwareExperiences.RemoveAll(s => s.IsHidden == true);

            if (applicant.profilephoto != null)
            {
                string uniqueFileName = GetUploadedFileName(applicant);
                applicant.PhotoUrl = uniqueFileName;
            }

            _context.Attach(applicant);
            _context.Entry(applicant).State = EntityState.Modified;

            _context.Experiences.AddRange(applicant.Experiences);
            _context.SoftwareExperiences.AddRange(applicant.SoftwareExperiences);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        private string GetUploadedFileName(Applicant applicant)
        {
            string uniqueFileName = null;

            if (applicant.profilephoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + applicant.profilephoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    applicant.profilephoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult Details(int id)
        {
            Applicant applicant = _context.Applicants
                .Include(e => e.Experiences)
                .Include(s => s.SoftwareExperiences)
                .Where(a => a.Id == id).FirstOrDefault()!;

            ViewBag.Rating = GetRating();
            ViewBag.Softwares = GetSoftware();

            return View(applicant);

        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Applicant applicant = _context.Applicants
               .Include(e => e.Experiences)
               .Include(s => s.SoftwareExperiences)
               .Where(a => a.Id == id).FirstOrDefault()!;

            ViewBag.Rating = GetRating();
            ViewBag.Softwares = GetSoftware();

            return View(applicant);

        }
        [HttpPost]
        public IActionResult Delete(Applicant applicant)
        {
            _context.Attach(applicant);
            _context.Entry(applicant).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetGender()
        {
            List<SelectListItem> selectGenero = new List<SelectListItem>();
            var selItem = new SelectListItem()
            {
                Value = "",
                Text = "Selección Genero"
            };
            selectGenero.Insert(0, selItem);
            selItem = new SelectListItem()
            {
                Value = "Masculino",
                Text = "Masculino"
            };
            selectGenero.Add(selItem);

            selItem = new SelectListItem()
            {
                Value = "Femenino",
                Text = "Femenino"
            };
            selectGenero.Add(selItem);
            selItem = new SelectListItem()
            {
                Value = "Otros",
                Text = "Otros"
            };
            selectGenero.Add(selItem);

            return selectGenero;
        }

        private List<SelectListItem> GetRating()
        {
            List<SelectListItem> selRating = new List<SelectListItem>();
            var selItem = new SelectListItem()
            {
                Value = "0",
                Text = "Sel. Calificación"
            };
            selRating.Insert(0, selItem);

            for (int i = 1; i < 11; i++)
            {
                selItem = new SelectListItem()
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                };
                selRating.Add(selItem);
            }
            return selRating;
        }

        private List<SelectListItem> GetSoftware()
        {
            List<SelectListItem> selSoftware = _context.Softwares.OrderBy(s => s.Name)
                                                                 .Select(s => new SelectListItem
                                                                 {
                                                                     Value = s.Id.ToString(),
                                                                     Text = s.Name
                                                                 }).ToList();
            var selItem = new SelectListItem()
            {
                Value = null,
                Text = "Sel. Software"
            };

            selSoftware.Insert(0, selItem);
            return selSoftware;
        }
    }
}
