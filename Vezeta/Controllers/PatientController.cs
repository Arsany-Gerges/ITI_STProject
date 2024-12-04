using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vezeta.Data;
using Vezeta.ViewModel;

namespace Vezeta.Controllers
{
	public class PatientController : Controller
	{
        private VezetaDbContext context;
        public PatientController()
        {
            context = new VezetaDbContext();
        }
        public IActionResult Index(int? id)
		{
            var patient = context.Patients.SingleOrDefault(p => p.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            var specializations = context.Doctors
           .Select(d => d.Specialization)
           .Distinct()
           .ToList();

#pragma warning disable CS8601 // Possible null reference assignment.
            var pendingRequests = context.AppointmentRequests
            .Where(a => a.PatientId == id)
            .Select(a => new PendingRequestViewModel
            {
                AppointmentRequestId = a.RequestId,
                PatientName =patient.Name,
                DoctorName = context.Doctors
                    .Where(d => d.DoctorId == a.DoctorId)
                    .Select(d => d.Name)
                    .FirstOrDefault(),
                Specialization = context.Doctors
                    .Where(d => d.DoctorId == a.DoctorId)
                    .Select(d => d.Specialization)
                    .FirstOrDefault(),
                RequestDate = a.RequestedDate,
                Status = a.Status,
                Symptoms = a.Symptoms
            })
            .ToList();
#pragma warning restore CS8601 // Possible null reference assignment.

            var model = new PatientDashboardViewModel
            {
                PatientId = patient.PatientId,
                Name = patient.Name,
                PendingRequests = pendingRequests,
                Specializations = specializations
            };
            ViewData["ShowLogoutButton"] = true;
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            var request = context.AppointmentRequests
                .Where(a => a.RequestId == id)
                .Select(a => new PendingRequestViewModel
                {
                    AppointmentRequestId = a.RequestId,
                    DoctorName = context.Doctors
                        .Where(d => d.DoctorId == a.DoctorId)
                        .Select(d => d.Name)
                        .FirstOrDefault(),
                    Specialization = context.Doctors
                        .Where(d => d.DoctorId == a.DoctorId)
                        .Select(d => d.Specialization)
                        .FirstOrDefault(),
                    RequestDate = a.RequestedDate,
                    Status = a.Status,
                    Symptoms = a.Symptoms,
                    AppointmentDate = a.AppointmentDate
                })
                .FirstOrDefault();
#pragma warning restore CS8601 // Possible null reference assignment.

            if (request == null)
            {
                return NotFound();
            }
            var patientId = HttpContext.Session.GetInt32("UserId");
            ViewBag.PatientId = patientId;
            ViewData["ShowLogoutButton"] = true;
            return View(request);
        }
        [HttpGet]
        public IActionResult SelectSpecialization()
        {
            ViewData["Specializations"] = context.Doctors
                .Select(d => d.Specialization)
                .Distinct()
                .ToList();
            return View();
        }
        [HttpPost]
        public IActionResult SelectSpecialization(string specialization)
        {
            if (string.IsNullOrEmpty(specialization))
            {
                return RedirectToAction("SelectSpecialization");
            }

            var doctors = context.Doctors
                .Where(d => d.Specialization == specialization)
                .ToList();

            ViewData["Doctors"] = doctors;
            ViewData["SelectedSpecialization"] = specialization;
            ViewData["ShowLogoutButton"] = true;
            return View("SelectDoctor");
        }

        [HttpGet]
        public IActionResult SelectDoctor()
        {
            var specialization = ViewData["SelectedSpecialization"] as string;
            var doctors = context.Doctors
                .Where(d => d.Specialization == specialization)
                .ToList();

            ViewData["Doctors"] = doctors;
            ViewData["ShowLogoutButton"] = true;
            return View();
        }

        [HttpPost("patient/RequestAppointment")]
        public IActionResult RequestAppointment(int doctorId, string symptoms)
        {
            var patientId = HttpContext.Session.GetInt32("UserId");

            if (patientId == null)
            {
                return RedirectToAction("Index", "Home"); // Redirect if patientId is not found
            }

            if (string.IsNullOrEmpty(symptoms))
            {
                ViewBag.ErrorMessage = "Symptoms description is required.";
                return RedirectToAction("Index", new { id = patientId });
            }

            var newRequest = new AppointmentRequest
            {
                PatientId = (int)patientId,
                DoctorId = doctorId,
                RequestedDate = DateTime.Now,
                Symptoms = symptoms,
                Status = "Pending"
            };

            context.AppointmentRequests.Add(newRequest);
            context.SaveChanges();

            return RedirectToAction("Index", new { id = patientId });
        }
    }
}
