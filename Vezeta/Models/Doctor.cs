using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
	public class Doctor : User
    {
        public int DoctorId { get; set; } // Primary key
        public string Specialization { get; set; }

        // Navigation property
        public List<AppointmentRequest> AppointmentRequests { get; set; } = new List<AppointmentRequest>();
    }
}
