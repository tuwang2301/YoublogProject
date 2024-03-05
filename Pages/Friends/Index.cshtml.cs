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

        public void OnGet(int? id, string? type)
        {
            var user = SessionUtil.GetObjectFromJson<User>(HttpContext.Session, "user");

            if (user == null)
            {
                Response.Redirect("/Login");
                return;
            }

            if(id == null) {
                Response.Redirect("/Index");
                return;
            }

            if (user.UserId == id)
            {
                Response.Redirect("/Profile?id=" + id);
                return;
            }

            var friendRequest = context.FriendRequests
                .FirstOrDefault(fr =>
                (fr.FromUserId == user.UserId && fr.ToUserId == id) ||
                (fr.ToUserId == user.UserId && fr.FromUserId == id));

            if(type== null)
            {
                if (friendRequest == null)
                {
                    FriendRequest newFriendRq = new FriendRequest()
                    {
                        FromUserId = user.UserId,
                        ToUserId = id,
                    };

                    context.FriendRequests.Add(newFriendRq);
                    context.SaveChanges();

                    Response.Redirect("/Profile?id=" + id);
                    return;
                }
            }

            if (type == "add")
            {
                context.Friends.Add(new Friend()
                {
                    CreatedAt = DateTime.Now,
                    UserId1 = user.UserId,
                    UserId2 = id
                });
                context.SaveChanges();

                context.FriendRequests.Remove(friendRequest);
                context.SaveChanges();

                Response.Redirect("/Profile?id=" + id);
                return;
            }else if (type == "delete")
            {
                var deleteFriend = context.Friends
                    .FirstOrDefault(fr =>
                    (fr.UserId1 == user.UserId && fr.UserId2 == id) ||
                    (fr.UserId2 == user.UserId && fr.UserId1 == id));

                context.Friends.Remove(deleteFriend);
                context.SaveChanges();

                Response.Redirect("/Profile?id=" + id);
                return;
            }else if (type == "delete-fr")
            {
                context.FriendRequests.Remove(friendRequest);
                context.SaveChanges();
                Response.Redirect("/Profile?id=" + id);
                return;
            }

        }

        public IActionResult OnPostGetAjax(string name)
        {
            return new JsonResult("Duoc roi " + name);
        }
    }
}
