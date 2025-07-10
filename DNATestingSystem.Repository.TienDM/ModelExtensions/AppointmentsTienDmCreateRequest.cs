using System;

namespace DNATestingSystem.Repository.TienDM.ModelExtensions
{
    /// <summary>
    /// Request DTO for creating/updating AppointmentsTienDm. Chỉ chứa các trường trực tiếp, không chứa navigation property.
    /// </summary>
    public class AppointmentsTienDmCreateRequest
    {
        public int? UserAccountId { get; set; }
        public int? ServicesNhanVtid { get; set; }
        public int? AppointmentStatusesTienDmid { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; } // ISO string: "09:00:00"
        public string? SamplingMethod { get; set; }
        public string? Address { get; set; }
        public string? ContactPhone { get; set; }
        public string? Notes { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsPaid { get; set; }
    }
}
