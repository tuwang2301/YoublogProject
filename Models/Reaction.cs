using System;
using System.Collections.Generic;

namespace YoublogProject.Models;

public partial class Reaction
{
    public int ReactionId { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public string? Type { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
