using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages
{
    public class LoginFormModel : PageModel
    {
        private readonly YoublogContext context;
        public string errorMessage = "";
        public string successMessage = "";

        [BindProperty]
        public LoginDto LoginDto { get; set; } = new LoginDto();

        public LoginFormModel(YoublogContext context)
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

            var user = context.Users.FirstOrDefault(a => a.Username == LoginDto.Username && a.Password == LoginDto.Password);

            if (user == null)
            {
                errorMessage = "User not exist";
                return;
            }

            SessionUtil.SetObjectAsJson( HttpContext.Session, "user", user);

            ModelState.Clear();
            Response.Redirect("/Index");
        }
    }
}
