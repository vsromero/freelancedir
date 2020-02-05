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
using Microsoft.AspNetCore.Authorization;

namespace FreelanceDir.Pages.Packages
{
    public class DeleteModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public DeleteModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Package Package { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Package = await _context.Packages
                .Include(p => p.Gig).FirstOrDefaultAsync(m => m.Id == id);

            if (Package == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Package = await _context.Packages
                .Include(p => p.Gig).FirstOrDefaultAsync(m => m.Id == id);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, Package.Gig, "IsOwner");

            if (isAuthorized.Succeeded)
            {
                if (Package != null)
                {
                    _context.Packages.Remove(Package);
                    await _context.SaveChangesAsync();
                }
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
