using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages.Posts
{
    public class PostDetailModel : PageModel
    {
        private readonly YoublogContext context;

        public Post Post { get; set; } = new Post();
        public List<Reaction> Reactions { get; set; } = new List<Reaction>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public PostDetailModel(YoublogContext context)
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

            Reactions = context.Reactions.ToList();

            Comments = context.Comments
                .Where(c => c.PostId == id)
                .Include(c => c.User)
                .ToList();

            var post = context.Posts
                .Include(p=> p.Content)
                .Include(p => p.User)
                .Include(p => p.Reactions) 
                .Include(p => p.Comments)
                .FirstOrDefault(p=>p.PostId == id);

            if (post == null)
            {
                Response.Redirect("/Index");
                return;
            }

            Post = post;

        }

        public void OnPost(int? id)
        {
            var user = SessionUtil.GetObjectFromJson<User>(HttpContext.Session, "user");

            if (user == null)
            {
                Response.Redirect("/Posts/PostDetail?id=" + id);
                return;
            }

            string comment = Request?.Form["comment"];

            if (comment.IsNullOrEmpty())
            {
                Response.Redirect("/Posts/PostDetail?id=" + id);
                return;
            }

            Comment cmt = new Comment()
            {
                PostId = id,
                UserId = user.UserId,
                Content = comment,
                CreatedAt = DateTime.Now,
            };

            context.Comments.Add(cmt);
            context.SaveChanges();

            Response.Redirect("/Posts/PostDetail?id=" + id);
            return;

        }
    }
}
