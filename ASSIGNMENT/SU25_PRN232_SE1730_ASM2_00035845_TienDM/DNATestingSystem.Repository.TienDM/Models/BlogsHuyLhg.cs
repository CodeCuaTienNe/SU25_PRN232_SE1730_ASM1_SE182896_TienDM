using System;
using System.Collections.Generic;

namespace DNATestingSystem.Repository.TienDM.Models;

public partial class BlogsHuyLhg
{
    public int BlogsHuyLhgid { get; set; }

    public int BlogCategoryHuyLhgid { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Summary { get; set; }

    public string? CoverImage { get; set; }

    public int ViewCount { get; set; }

    public DateTime? PublishDate { get; set; }

    public bool IsPublished { get; set; }

    public int Priority { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual BlogCategoriesHuyLhg BlogCategoryHuyLhg { get; set; } = null!;

    public virtual SystemUserAccount User { get; set; } = null!;
}
