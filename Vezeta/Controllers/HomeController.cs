using Final_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Numerics;
using Vezeta.Data;

namespace Final_Project.Controllers
{
	public class HomeController : Controller
	{
        private VezetaDbContext context;
        public HomeController()
        {
            context = new VezetaDbContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("home/login")]
        public IActionResult Login()
        {

            ViewData["ShowLogoutButton"] = false;
            return View();
        }

        [HttpPost("home/login")]
        public IActionResult Login(string usernameOrEmail, string password, string role)
        {
            if (role == "Doctor")
            {
                var doctor = context.Doctors
                    .FirstOrDefault(d => (d.Username == usernameOrEmail || d.Email == usernameOrEmail) && d.Password == password);

                if (doctor != null)
                {
                    HttpContext.Session.SetInt32("UserId", doctor.DoctorId);
                    return RedirectToAction("Index", "Doctor", new { id = doctor.DoctorId });
                }
            }
            else if (role == "Patient")
            {
                var patient = context.Patients
                    .FirstOrDefault(p => (p.Username == usernameOrEmail || p.Email == usernameOrEmail) && p.Password == password);

                if (patient != null)
                {
                    HttpContext.Session.SetInt32("UserId", patient.PatientId);
                    return RedirectToAction("Index", "Patient", new { id = patient.PatientId });
                }
            }

            ViewBag.ErrorMessage = "Invalid username/email or password.";
            ViewData["ShowLogoutButton"] = true;
            return View();
        }

        [HttpGet("home/register")]
        public IActionResult Register()
        {
            ViewData["ShowLogoutButton"] = false;
            return View();
        }

        [HttpPost("home/register")]
        public IActionResult Register(string name, string email, string phone, string username, string password, string role, int? age, string specialization)
        {
            if (role == "Doctor")
            {
                var doctor = new Doctor
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Username = username,
                    Password = password,
                    Specialization = specialization
                };

                context.Doctors.Add(doctor);
            }
            else if (role == "Patient")
            {
                var patient = new Patient
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Username = username,
                    Password = password,
                    Age = age.GetValueOrDefault()
                };

                context.Patients.Add(patient);
            }

            context.SaveChanges();

            TempData["SuccessMessage"] = "The account was created successfully.";
            ViewData["ShowLogoutButton"] = false;
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            // Clear authentication cookies or session
            HttpContext.SignOutAsync();

            // Redirect to login page
            return RedirectToAction("Index", "Home");
        }

    }
}
