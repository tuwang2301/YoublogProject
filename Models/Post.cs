using System;
using System.Collections.Generic;

namespace YoublogProject.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? ContentId { get; set; }

    public int? UserId { get; set; }

    public string? PrivacyMode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Content? Content { get; set; }

    public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();

    public virtual User? User { get; set; }
}
