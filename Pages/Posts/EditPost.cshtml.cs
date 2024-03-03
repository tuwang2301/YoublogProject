using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages.Posts
{
    public class EditPostModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly YoublogContext context;

        [BindProperty]
        public PostDto PostDto { get; set; } = new PostDto();

        public Post Post { get; set; }

        public EditPostModel(IWebHostEnvironment environment, YoublogContext context)
        {
            this.environment = environment;
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

            if (id == null)
            {
                Response.Redirect("/Login");
                return;

            }

            var updatePost = context.Posts.Find(id);


            if (updatePost == null)
            {
                Response.Redirect("/Login");
                return;
            }

            updatePost.Content = context.Contents.Find(updatePost.ContentId);
            PostDto.PrivacyMode = updatePost.PrivacyMode;
            PostDto.Description = updatePost?.Content?.Description;

            Post = updatePost;

        }

        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Index");
                return;
            }

            var updatePost = context.Posts.Find(id);
            if (updatePost == null)
            {
                Response.Redirect("/Index");
                return;
            }

            var updateContent = context.Contents.Find(updatePost.ContentId);
            if (updateContent == null)
            {
                Response.Redirect("/Index");
                return;
            }

            string newFileName = updateContent?.ContentUrl;
            if (PostDto.ContentMedia != null)
            {

                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(PostDto.ContentMedia.FileName);
                string imageFullPath = environment.WebRootPath + "/media/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    PostDto.ContentMedia.CopyTo(stream);
                }
            }

            updatePost.PrivacyMode = PostDto.PrivacyMode;
            updateContent.Description = PostDto.Description;
            updateContent.ContentUrl = newFileName;

            context.SaveChanges();

            Post = updatePost;

            ModelState.Clear();

        }
    }
}
