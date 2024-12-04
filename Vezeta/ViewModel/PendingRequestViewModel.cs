namespace Vezeta.ViewModel
{
    public class PendingRequestViewModel
    {
        public int AppointmentRequestId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string Specialization { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Symptoms { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}
