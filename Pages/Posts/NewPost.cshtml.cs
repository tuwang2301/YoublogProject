using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages.Posts
{
    public class NewPostModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly YoublogContext context;

        [BindProperty]
        public PostDto PostDto { get; set; } = new PostDto();

        public NewPostModel(IWebHostEnvironment environment, YoublogContext context)
        {
            this.environment = environment;
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

        }

        public void OnPost()
        {
            var user = SessionUtil.GetObjectFromJson<User>(HttpContext.Session, "user");
            if (user == null)
            {
                Response.Redirect("/Login");
                return;
            }

            Content content = new Content();
            content.Description = PostDto.Description;

            if (PostDto.ContentMedia != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(PostDto.ContentMedia.FileName);
                string imageFullPath = environment.WebRootPath + "/media/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    PostDto.ContentMedia.CopyTo(stream);
                }

                content.ContentUrl = newFileName;
            }

            context.Contents.Add(content);
            context.SaveChanges();

            Post post = new Post()
            {
                PrivacyMode = PostDto.PrivacyMode,
                ContentId = content.ContentId,
                Content = content,
                UserId = user.UserId,
                CreatedAt = DateTime.Now,
            };

            context.Posts.Add(post);
            context.SaveChanges();

            Response.Redirect("/Index");
            return;
        }
    }
}
