namespace Vezeta.ViewModel
{
    public class DoctorDashboardViewModel
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public List<PendingRequestViewModel> PendingRequests { get; set; }

    }
}
