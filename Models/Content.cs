using System;
using System.Collections.Generic;

namespace YoublogProject.Models;

public partial class Content
{
    public int ContentId { get; set; }

    public string? ContentUrl { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual Post? Post { get; set; }

    public virtual ICollection<Story> Stories { get; set; } = new List<Story>();
}
