using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DNATestingSystem.Repository.TienDM.Models;

public partial class AppointmentsTienDm
{
    public int AppointmentsTienDmid { get; set; }

    public int UserAccountId { get; set; }

    public int ServicesNhanVtid { get; set; }

    public int AppointmentStatusesTienDmid { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentTime { get; set; }

    public string SamplingMethod { get; set; } = null!;

    public string? Address { get; set; }

    public string ContactPhone { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }    public decimal TotalAmount { get; set; }

    public bool? IsPaid { get; set; }


    [JsonIgnore]
    public virtual AppointmentStatusesTienDm AppointmentStatusesTienDm { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<SampleThinhLc> SampleThinhLcs { get; set; } = new List<SampleThinhLc>();

    [JsonIgnore]
    public virtual ServicesNhanVt ServicesNhanVt { get; set; } = null!;

    [JsonIgnore]
    public virtual SystemUserAccount UserAccount { get; set; } = null!;
}
