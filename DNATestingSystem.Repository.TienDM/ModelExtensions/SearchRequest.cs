using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Repository.TienDM.ModelExtensions
{
    public class SearchRequest
    {
        public int? CurrentPage { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }

    public class SearchAppointmentsTienDm : SearchRequest
    {
        public int? AppointmentsTienDmid { get; set; }
        public string? ContactPhone { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
