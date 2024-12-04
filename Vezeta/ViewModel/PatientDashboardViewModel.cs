namespace Vezeta.ViewModel
{
    public class PatientDashboardViewModel
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public List<PendingRequestViewModel> PendingRequests { get; set; }
        public List<string> Specializations { get; set; } 
    }
}
