using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
	public class AppointmentRequest
	{
		[Key]
		public int RequestId { get; set; }

		[ForeignKey("Patient")]
		public int PatientId { get; set; }
		public Patient Patient { get; set; } // Navigation property

		[ForeignKey("Doctor")]
		public int DoctorId { get; set; }
		public Doctor Doctor { get; set; } // Navigation property

		[Required]
		public string Symptoms { get; set; }

		[Required]
		public DateTime RequestedDate { get; set; }

		[Required]
		public string Status { get; set; } // "Pending", "Accepted", "Rejected"

		public DateTime? AppointmentDate { get; set; } // Nullable, set by the doctor
	}
}
