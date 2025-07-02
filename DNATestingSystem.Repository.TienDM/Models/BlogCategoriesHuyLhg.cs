using System;
using System.Collections.Generic;

namespace DNATestingSystem.Repository.TienDM.Models;

public partial class BlogCategoriesHuyLhg
{
    public int BlogCategoryHuyLhgid { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BlogsHuyLhg> BlogsHuyLhgs { get; set; } = new List<BlogsHuyLhg>();
}
