using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoublogProject.Models;

public partial class UserDto
{
    [Required, MinLength(5)]
    public string Username { get; set; } = null!;
    [Required, MinLength(5)]
    public string Password { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    public string FullName { get; set; }

    [Display(Name ="Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public IFormFile? Avatar { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }


}
