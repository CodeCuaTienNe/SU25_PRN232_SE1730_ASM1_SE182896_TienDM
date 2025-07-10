using System;
using System.Collections.Generic;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Models
{
    public class ApiAppointmentDTO
    {
        public int appointmentsTienDmid { get; set; }
        public int userAccountId { get; set; }
        public string username { get; set; }
        public int servicesNhanVtid { get; set; }
        public string serviceName { get; set; }
        public int appointmentStatusesTienDmid { get; set; }
        public string statusName { get; set; }
        public string appointmentDate { get; set; }
        public string appointmentTime { get; set; }
        public string samplingMethod { get; set; }
        public string address { get; set; }
        public string contactPhone { get; set; }
        public string notes { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? modifiedDate { get; set; }
        public decimal totalAmount { get; set; }
        public bool? isPaid { get; set; }
    }

    public class ApiPaginationResult<T>
    {
        public int totalItems { get; set; }
        public int totalPages { get; set; }
        public int currentPages { get; set; }
        public int pageSize { get; set; }
        public T items { get; set; }
    }
}
