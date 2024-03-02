using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YoublogProject.Models;

namespace YoublogProject.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly YoublogProject.Models.YoublogContext _context;

        public IndexModel(YoublogProject.Models.YoublogContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                User = await _context.Users.ToListAsync();
            }
        }
    }
}
