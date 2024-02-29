﻿using System;
using System.Collections.Generic;

namespace YoublogProject.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? ContentId { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Content? ContentNavigation { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
