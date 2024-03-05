using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly YoublogContext context;

        public List<Post> Posts { get; set; } = new List<Post>();

        public List<Reaction> Reactions { get; set; } = new List<Reaction>();

        public IndexModel(YoublogContext context)
        {
            this.context = context;
        }

        public void OnGet(int? id)
        {
            var user = SessionUtil.GetObjectFromJson<User>(HttpContext.Session, "user");

            if (user == null)
            {
                Response.Redirect("/Login");
                return;
            }

            Reactions = context.Reactions.ToList();

            var posts = context.Posts
                .Where(p => p.UserId == id)
                .Include(p => p.Content)
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            if (user.UserId != id)
            {
                Posts = posts.Where(p =>
                    (p.PrivacyMode == "public") ||
                    (p.PrivacyMode == "friends" && context.Friends.Any(f =>
                        (f.UserId1 == user.UserId && f.UserId2 == p.UserId) ||
                        (f.UserId2 == user.UserId && f.UserId1 == p.UserId)
                    ))).ToList();
            }else
            {
                Posts = posts.ToList();
            }




        }

        public IActionResult OnPostGetAjax(int userId, int postId)
        {
            var react = context.Reactions.FirstOrDefault(r => r.UserId == userId && r.PostId == postId);

            if (react == null)
            {
                context.Reactions.Add(new Reaction()
                {
                    UserId = userId,
                    PostId = postId
                });
                context.SaveChanges();
                return new JsonResult("like");
            }
            else
            {
                context.Reactions.Remove(react);
                context.SaveChanges();
                return new JsonResult("unlike");
            }


        }
    }
}
