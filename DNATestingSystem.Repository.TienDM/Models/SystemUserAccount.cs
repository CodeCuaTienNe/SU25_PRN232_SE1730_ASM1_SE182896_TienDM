using System;
using System.Collections.Generic;

namespace DNATestingSystem.Repository.TienDM.Models;

public partial class SystemUserAccount
{
    public int UserAccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public int RoleId { get; set; }

    public string? RequestCode { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ApplicationCode { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AppointmentsTienDm> AppointmentsTienDms { get; set; } = new List<AppointmentsTienDm>();

    public virtual ICollection<BlogsHuyLhg> BlogsHuyLhgs { get; set; } = new List<BlogsHuyLhg>();

    public virtual ICollection<OrderGiapHd> OrderGiapHds { get; set; } = new List<OrderGiapHd>();

    public virtual ICollection<ProfileThinhLc> ProfileThinhLcs { get; set; } = new List<ProfileThinhLc>();

    public virtual ICollection<UserServiceNhanVt> UserServiceNhanVtCreatedByNavigations { get; set; } = new List<UserServiceNhanVt>();

    public virtual ICollection<UserServiceNhanVt> UserServiceNhanVtModifiedByNavigations { get; set; } = new List<UserServiceNhanVt>();

    public virtual ICollection<UserServiceNhanVt> UserServiceNhanVtUserAccounts { get; set; } = new List<UserServiceNhanVt>();
}
