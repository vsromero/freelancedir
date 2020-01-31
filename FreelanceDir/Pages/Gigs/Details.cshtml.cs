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

namespace FreelanceDir.Pages.Gigs
{
    public class DetailsModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;

        public DetailsModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Gig Gig { get; set; }
        [BindProperty]
        public Review Review { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Gig = await _context.Gigs
                .Include(g => g.User)
                .Include(g => g.Category)
                    .ThenInclude(c => c.Parent)
                .Include(g => g.Packages)
                .Include(g => g.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Gig == null)
            {
                return NotFound();
            }

            var positiveReviews = Gig.Reviews.Where(r => r.Positive).Count();
            var totalReviews = Gig.Reviews.Count;
            Gig.PositivePercentage = Gig.Reviews.Count > 0 ? Decimal.Divide(positiveReviews, totalReviews) : 0;            
            Gig.TotalViewsCount++;
            return Page();
        }

        public async Task<IActionResult> OnPostReview()
        {
            if (!ModelState.IsValid)
            {
                if (Review.GigId>0)
                {
                    return RedirectToPage("Details", new { id = Review.GigId });
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }

            Review.User = await _userManager.GetUserAsync(User);
            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();
            return RedirectToPage("Details", new { id = Review.GigId });
        }
    }
}
