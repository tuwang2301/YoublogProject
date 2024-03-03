using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoublogProject.Models;

public partial class PostDto
{
    [Required]
    public string? Description { get; set; }

    public IFormFile? ContentMedia { get; set; }

    public string? PrivacyMode { get; set; }

}
