using System;
using System.Collections.Generic;

namespace YoublogProject.Models;

public partial class Share
{
    public int ShareId { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
