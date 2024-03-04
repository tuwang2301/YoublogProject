using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages.Friends
{
    public class IndexModel : PageModel
    {
        private readonly YoublogContext context;

        public IndexModel(YoublogContext context)
        {
            this.context = context;
        }

        public IActionResult OnPostGetAjax(string name)
        {
            return new JsonResult("Duoc roi " + name);
        }
    }
}
