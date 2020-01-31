using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreelanceDir.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FreelanceDir.Pages.Users
{
    public class ProfileModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;

        public ProfileModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public new User User { get; set; }
        public List<Gig> Gigs { get; set; }
        public List<Review> ReviewList { get; set; }

        public async Task<IActionResult> OnGet(string userName)
        {
            if (userName == null)
            {
                return NotFound();
            }

            User = await _userManager.FindByNameAsync(userName);

            if (User == null)
            {
                return NotFound();
            }

            Gigs = _context.Gigs
                .Where(g => g.Active && g.UserId == User.Id)
                .Include(g => g.User)                
                .OrderByDescending(g => g.LastModifiedDate > g.CreatedDate ? g.LastModifiedDate : g.CreatedDate)
                .Take(10).ToList();
            ReviewList = _context.Gigs
                .SelectMany(g => g.Reviews)                
                .Where(r => r.Positive && r.Gig.UserId == User.Id)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedDate)
                .Take(10).ToList();

            return Page();
        }
    }
}