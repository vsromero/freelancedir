using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FreelanceDir.Data;
using FreelanceDir.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FreelanceDir.Pages.Gigs
{
    public class CreateModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            ViewData["topCategories"] = _context.Categories.Include(c => c.Children).Where(c => c.ParentId == null).ToList();
            return Page();
        }

        [BindProperty]
        public Gig Gig { get; set; }    

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("OnGet");
            }
            Gig.User = await _userManager.GetUserAsync(User);
            var basicPackage = new Package()
            {
                Name = "Basic",
                Details = "This is an example package. You should edit this and all the fields below.",
                Gig = Gig,
            };
            
            Gig.Packages = new List<Package>() { basicPackage };            
            Gig.Reviews = new List<Review>();
            _context.Gigs.Add(Gig);            
            await _context.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = Gig.Id });
        }
    }
}