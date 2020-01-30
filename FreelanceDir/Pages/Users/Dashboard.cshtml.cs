using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FreelanceDir.Data;
using FreelanceDir.Models;
using Microsoft.AspNetCore.Identity;

namespace FreelanceDir.Pages.Users
{
    public class DashboardModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Gig> Gigs { get;set; }

        public async Task OnGetAsync()
        {
            Gigs = await _context.Gigs
                .Where(g => g.UserId == _userManager.GetUserId(User))
                .Include(g => g.User).ToListAsync();
        }
    }
}
