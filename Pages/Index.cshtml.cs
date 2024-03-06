using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly YoublogContext context;
        public List<Post> Posts { get; set; } = new List<Post>();

        public List<Reaction> Reactions { get; set; } = new List<Reaction>();

        public IndexModel(ILogger<IndexModel> logger, YoublogContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public void OnGet()
        {
            var user = SessionUtil.GetObjectFromJson<User>(HttpContext.Session, "user");

            Reactions = context.Reactions.ToList();

            var postBase = context.Posts
                      .Include(p => p.Content)
                      .Include(p => p.User)
                      .Include(p => p.Reactions)
                      .Include(p => p.Comments)
                      .OrderByDescending(p => p.CreatedAt);
                      

            if (user == null)
            {
                Posts = postBase.Where(p => p.PrivacyMode == "public").ToList();
            }
            else
            {
                Posts = postBase
                    .Where(p =>
                    (p.PrivacyMode == "public") ||
                    (p.PrivacyMode == "friends" && context.Friends.Any(f =>
                        (f.UserId1 == user.UserId && f.UserId2 == p.UserId) ||
                        (f.UserId2 == user.UserId && f.UserId1 == p.UserId)
                    ) || (p.UserId == user.UserId)) ||
                    (p.PrivacyMode == "private" && p.UserId == user.UserId)
                    )
                    .ToList();
            }
        }

        public IActionResult OnPostGetAjax(string name)
        {
            return new JsonResult("Hello " + name);
        }

    }
}
