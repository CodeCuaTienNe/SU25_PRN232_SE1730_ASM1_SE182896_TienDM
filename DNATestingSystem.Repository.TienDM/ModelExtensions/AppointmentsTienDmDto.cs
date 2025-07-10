using System;

namespace DNATestingSystem.Repository.TienDM.ModelExtensions
{
    public class AppointmentsTienDmDto
    {
        public int AppointmentsTienDmid { get; set; }
        public int UserAccountId { get; set; }
        public string? Username { get; set; }
        public int ServicesNhanVtid { get; set; }
        public string? ServiceName { get; set; }
        public int AppointmentStatusesTienDmid { get; set; }
        public string? StatusName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string SamplingMethod { get; set; } = null!;
        public string? Address { get; set; }
        public string ContactPhone { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsPaid { get; set; }
    }
}
