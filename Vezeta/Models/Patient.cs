using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
	public class Patient : User
    {
        public int PatientId { get; set; } // Primary key
        public int Age { get; set; }

        // Navigation property
        public List<AppointmentRequest> AppointmentRequests { get; set; } = new List<AppointmentRequest>();
    }
}
