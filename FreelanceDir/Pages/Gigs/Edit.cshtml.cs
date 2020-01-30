using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreelanceDir.Data;
using FreelanceDir.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace FreelanceDir.Pages.Gigs
{
    public class EditModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Gig Gig { get; set; }

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
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Gig == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, Gig, "IsOWner");

            if (isAuthorized.Succeeded)
            {
                ViewData["topCategories"] = _context.Categories
                    .Include(c => c.Children)
                    .Where(c => c.ParentId == null)
                    .AsNoTracking()
                    .ToList();
                return Page();
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                //return Page();
                return RedirectToPage("Edit", new { id = Gig.Id });
            }

            var existingGig = await _context.Gigs
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == Gig.Id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, existingGig, "IsOwner");

            if (isAuthorized.Succeeded)
            {
                return await UpdateGig();                
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        private bool GigExists(int id)
        {
            return _context.Gigs.Any(e => e.Id == id);
        }

        private async Task<IActionResult> UpdateGig()
        {
            Gig.User = await _userManager.GetUserAsync(User);
            _context.Attach(Gig).State = EntityState.Modified;
            foreach (var package in Gig.Packages)
            {
                _context.Attach(package).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GigExists(Gig.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("Edit", new { id = Gig.Id });
        }
    }
}
