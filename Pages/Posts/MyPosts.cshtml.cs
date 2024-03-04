using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages.Posts
{
    public class MyPostsModel : PageModel
    {
        private readonly YoublogContext context;

        public List<Post> Posts { get; set; } = new List<Post>();

        public MyPostsModel(YoublogContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            var user = SessionUtil.GetObjectFromJson<User>(HttpContext.Session, "user");

            if (user == null)
            {
                Response.Redirect("/Login");
                return;
            }

            Posts = context.Posts
                .Where(p => p.UserId == user.UserId)
                .Include(p => p.Content)
                .Include(p => p.User)
                .ToList();


        }
    }
}
