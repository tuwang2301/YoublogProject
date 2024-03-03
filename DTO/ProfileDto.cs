using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YoublogProject.Models;

public partial class ProfileDto
{
    public string FullName { get; set; }

    [Display(Name ="Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    [DataType(DataType.Upload)]
    [FileExtensions(Extensions = "png,jpg,jpeg")]
    [BindProperty]
    public IFormFile? Avatar { get; set; }

    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
}
