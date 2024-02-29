using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoublogProject.Models;

namespace YoublogProject.Pages
{
    public class SignupFormModel : PageModel
    {

        private readonly YoublogContext context;
        public string errorMessage = "";
        public string successMessage = "";

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();


        public SignupFormModel(YoublogContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Please correct all errors";
                return;
            }

            var checkDup = context.Users.FirstOrDefault(a => a.Username == UserDto.Username || a.Email == UserDto.Email);

            if(checkDup != null)
            {
                errorMessage = "User already exist";
                return;
            }

            User user = new User()
            {
                Username = UserDto.Username,
                Email = UserDto.Email,
                Password = UserDto.Password,
                FullName = UserDto.FullName,
                PhoneNumber = UserDto.PhoneNumber,
                Gender = UserDto.Gender,
                DateOfBirth = UserDto.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
            };

            context.Users.Add(user);
            context.SaveChanges();

            ModelState.Clear();

            successMessage = "User created successfully";

            Response.Redirect("/Login");
        }
    }
}
