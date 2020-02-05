using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FreelanceDir.Data;
using FreelanceDir.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FreelanceDir.Pages.Packages
{
    public class CreateModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync(int? gigid)
        {
            if (gigid == null)
            {
                return NotFound();
            }

            Gig = await _context.Gigs
                .Include(g => g.User)
                .Include(g => g.Packages)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == gigid);

            if (Gig == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, Gig, "IsOWner");

            if (isAuthorized.Succeeded)
            {
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

        public Gig Gig { get; set; }
        [BindProperty]
        public Package Package { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingGig = await _context.Gigs
                .Include(g => g.Packages)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == Package.GigId);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, existingGig, "IsOwner");

            if (isAuthorized.Succeeded)
            {
                _context.Packages.Add(Package);
                //existingGig.StartingPrice = existingGig.Packages.Append(Package).Min(p => p.Price);
                _context.Update(existingGig);
                await _context.SaveChangesAsync();

                return RedirectToPage("../Edit", new { id = Package.GigId });
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
    }
}