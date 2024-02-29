using System;
using System.Collections.Generic;

namespace YoublogProject.Models;

public partial class Story
{
    public int StoryId { get; set; }

    public int? UserId { get; set; }

    public int? ContentId { get; set; }

    public string? PrivacyMode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Content? Content { get; set; }

    public virtual User? User { get; set; }
}
