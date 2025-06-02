using DNATestingSystem.Repository.TienDM.Models;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Models
{
    public class PaginatedAppointmentsViewModel
    {
        public List<AppointmentsTienDm> Appointments { get; set; } = new List<AppointmentsTienDm>();
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        // Search parameters
        public int SearchId { get; set; }
        public string SearchContactPhone { get; set; } = "";
        public decimal SearchTotalAmount { get; set; }

        // Helper properties for pagination
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int PreviousPage => HasPreviousPage ? CurrentPage - 1 : 1;
        public int NextPage => HasNextPage ? CurrentPage + 1 : TotalPages;
    }
}
