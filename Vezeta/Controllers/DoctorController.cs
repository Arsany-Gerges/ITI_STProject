using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vezeta.Data;
using Vezeta.ViewModel;

namespace Vezeta.Controllers
{
	public class DoctorController : Controller
	{
		private VezetaDbContext context;
		public DoctorController()
		{
			context = new VezetaDbContext();
		}
		public IActionResult Index(int? id)
		{
            var doctor = context.Doctors.SingleOrDefault(d => d.DoctorId == id);

            if (doctor == null)
            {
                return NotFound();
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            var pendingRequests = context.AppointmentRequests
                .Where(a => a.DoctorId == id )
                .Select(a => new PendingRequestViewModel
                {
                    AppointmentRequestId = a.RequestId,
                    DoctorName = doctor.Name,
                    PatientName = context.Patients
                        .Where(d => d.PatientId == a.PatientId)
                        .Select(d => d.Name)
                        .FirstOrDefault(),
                    Specialization = doctor.Specialization,
                    RequestDate = a.RequestedDate,
                    Status = a.Status,
                    Symptoms = a.Symptoms
                })
                .ToList();
#pragma warning restore CS8601 // Possible null reference assignment.

            var model = new DoctorDashboardViewModel
            {
                DoctorId = doctor.DoctorId,
                Name = doctor.Name,
                PendingRequests = pendingRequests,
            };
            ViewData["ShowLogoutButton"] = true;
            return View(model);
        }
        public IActionResult RequestDetails(int id)
        {
            var request = context.AppointmentRequests
                .Include(r => r.Doctor)
                .Include(r => r.Patient)
                .FirstOrDefault(r => r.RequestId == id);

            if (request == null)
            {
                return NotFound();
            }

            var viewModel = new PendingRequestViewModel
            {
                AppointmentRequestId = request.RequestId,
                DoctorName = request.Doctor.Name,
                PatientName=request.Patient.Name,
                Specialization = request.Doctor.Specialization,
                RequestDate = request.RequestedDate,
                Status = request.Status,
                Symptoms = request.Symptoms,
                AppointmentDate = request.AppointmentDate
            };
            var doctorId = HttpContext.Session.GetInt32("UserId");
            ViewBag.DoctorId = doctorId;
            ViewData["ShowLogoutButton"] = true;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AcceptRequest(int requestId, DateTime appointmentDate)
        {
            var request = context.AppointmentRequests
                .FirstOrDefault(r => r.RequestId == requestId);

            if (request != null)
            {
                request.AppointmentDate = appointmentDate;
                request.Status = "Accepted";
                context.SaveChanges();
            }

            return RedirectToAction("Index", new { id = request.DoctorId });
        }

        [HttpPost]
        public IActionResult RejectRequest(int requestId)
        {
            var request = context.AppointmentRequests
                .FirstOrDefault(r => r.RequestId == requestId);

            if (request != null)
            {
                request.Status = "Rejected";
                context.SaveChanges();
            }

            return RedirectToAction("Index", new { id = request.DoctorId });
        }
    }
}
