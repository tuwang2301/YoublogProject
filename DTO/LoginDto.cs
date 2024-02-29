using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoublogProject.Models;

public partial class LoginDto
{
    [Required, MinLength(5)]
    public string Username { get; set; } = null!;
    [Required, MinLength(5)]
    public string Password { get; set; } = null!;

}
