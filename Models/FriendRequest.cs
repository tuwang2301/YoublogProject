using System;
using System.Collections.Generic;

namespace YoublogProject.Models;

public partial class FriendRequest
{
    public int FriendRequestId { get; set; }

    public int? FromUserId { get; set; }

    public int? ToUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? FromUser { get; set; }

    public virtual User? ToUser { get; set; }
}
