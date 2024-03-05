using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YoublogProject.Models;

namespace YoublogProject.Pages.Friends
{
    public class MyFriendsModel : PageModel
    {
        private readonly YoublogContext context;

        public List<User> Users { get; set; } = new List<User>();

        public MyFriendsModel(YoublogContext context)
        {
            this.context = context;
        }

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/");
                return;
            }

            Users = context.Friends
            .Where(f => f.UserId1 == id || f.UserId2 == id)
            .Select(f => f.UserId1 == id ? f.UserId2Navigation : f.UserId1Navigation)
            .ToList();

        }
    }
}
