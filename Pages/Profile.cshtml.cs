using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using YoublogProject.Models;
using YoublogProject.Utils;

namespace YoublogProject.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly YoublogContext context;
        private readonly IWebHostEnvironment environment;

        [BindProperty]
        public ProfileDto Profile { get; set; } = new ProfileDto();

        public User User { get; set; } = new User();

        public ProfileModel(YoublogContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public void OnGet(int? id)
        {

            if (id == null)
            {
                Response.Redirect("/Index");
                return;
            }
            var updateUser = context.Users.Find(id);
            if (updateUser == null)
            {
                Response.Redirect("/Index");
                return;
            }

            Profile.FullName = updateUser.FullName;
            Profile.PhoneNumber = updateUser.PhoneNumber;
            Profile.Address = updateUser.Address;
            Profile.Gender = updateUser.Gender;
            Profile.DateOfBirth = updateUser.DateOfBirth;

            User = updateUser;

        }

        public void OnPost(int? id)
        {

            if (id == null)
            {
                Response.Redirect("/Index");
                return;
            }

            var updateUser = context.Users.Find(id);
            if (updateUser == null)
            {
                Response.Redirect("/Index");
                return;
            }
            string newFileName = updateUser.Avatar;
            if (Profile.Avatar != null)
            {

                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(Profile.Avatar.FileName);
                string imageFullPath = environment.WebRootPath + "/media/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    Profile.Avatar.CopyTo(stream);
                }
            }
            updateUser.FullName = Profile.FullName;
            updateUser.PhoneNumber = Profile.PhoneNumber;
            updateUser.DateOfBirth = Profile.DateOfBirth;
            updateUser.Gender = Profile.Gender;
            updateUser.PhoneNumber = Profile.PhoneNumber;
            updateUser.Address = Profile.Address;
            updateUser.Avatar = newFileName;
            context.SaveChanges();

            User = updateUser;

            SessionUtil.SetObjectAsJson(HttpContext.Session, "user", User);

            ModelState.Clear();
            Response.Redirect("/Profile?id=" + User.UserId);



        }
        public IActionResult OnPostGetAjax(string name)
        {
            return new JsonResult("Bye " + name);
        }
    }

    
}
