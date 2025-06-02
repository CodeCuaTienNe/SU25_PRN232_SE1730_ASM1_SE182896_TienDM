using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DNATestingSystem.Repository.TienDM.Models;

public partial class ServicesNhanVt
{
    public int ServicesNhanVtid { get; set; }

    public int ServiceCategoryNhanVtid { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Duration { get; set; }

    public bool? IsSelfSampleAllowed { get; set; }

    public bool? IsHomeVisitAllowed { get; set; }

    public bool? IsClinicVisitAllowed { get; set; }

    public int? ProcessingTime { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsActive { get; set; }

    public int UserAccountId { get; set; }

    // Keep JsonIgnore to avoid circular reference when serializing appointments collection
    [JsonIgnore]
    public virtual ICollection<AppointmentsTienDm> AppointmentsTienDms { get; set; } = new List<AppointmentsTienDm>();

    // Allow serialization of navigation properties for display
    public virtual ServiceCategoriesNhanVt? ServiceCategoryNhanVt { get; set; }

    // Allow serialization of navigation properties for display
    public virtual SystemUserAccount? UserAccount { get; set; }
}
