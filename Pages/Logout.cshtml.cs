using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoublogProject.Utils;

namespace YoublogProject.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            SessionUtil.SetObjectAsJson(HttpContext.Session, "user", null);
            TempData["SuccessMessage"] = "Lougout successfully";
            Response.Redirect("/");
        }
    }
}
