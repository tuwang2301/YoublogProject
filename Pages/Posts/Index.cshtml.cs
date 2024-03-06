using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YoublogProject.DTO;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly YoublogContext context;

        public List<Post> Posts { get; set; } = new List<Post>();

        public List<Reaction> Reactions { get; set; } = new List<Reaction>();

        [BindProperty]
        public PostFilterDto PostFilterDto { get; set; } = new PostFilterDto();

        public int? UserId { get; private set; }

        public IndexModel(YoublogContext context)
        {
            this.context = context;
        }

        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Index");
                return;
            }

            var user = SessionUtil.GetObjectFromJson<User>(HttpContext.Session, "user");

            UserId = id;

            Reactions = context.Reactions.ToList();

            var posts = context.Posts
                .Where(p => p.UserId == id)
                .Include(p => p.Content)
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            if (user != null)
            {
                if (user.UserId == id)
                {
                    Posts = posts.ToList();
                }
                else
                {
                    var checkFriend = context.Friends.Any(f =>
                           (f.UserId1 == user.UserId && f.UserId2 == id) ||
                           (f.UserId2 == user.UserId && f.UserId1 == id));

                    ViewData["checkFriend"] = checkFriend;

                    Posts = posts.Where(p =>
                       (p.PrivacyMode == "public") ||
                       checkFriend).ToList();
                }

            }

            Posts = posts.Where(p => p.PrivacyMode == "public").ToList();


        }

        public void OnPost(int? id)
        {
            var posts = context.Posts
                .Where(p => p.UserId == id && (p.PrivacyMode == PostFilterDto.PrivacyMode || PostFilterDto.PrivacyMode == "all"))
                .Include(p => p.Content)
                .Include(p => p.Comments)
                .Include(p => p.User)
                .Include(p => p.Reactions);


            if (PostFilterDto.SortDirection == "asc")
            {
                Posts = posts
                    .OrderBy(p => PostFilterDto.SortField == "like" ? p.Reactions.Count : p.Comments.Count)
                   .ToList();
            }
            else
            {
                Posts = posts
                     .OrderByDescending(p => PostFilterDto.SortField == "like" ? p.Reactions.Count : p.Comments.Count)
                    .ToList();
            }

            if (!PostFilterDto.SearchContent.IsNullOrEmpty())
            {
                Posts = Posts
                    .Where(p => p.Content.Description.ToUpper().Contains(PostFilterDto.SearchContent.ToUpper().Trim()))
                    .ToList();
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
