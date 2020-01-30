using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreelanceDir.Data;
using FreelanceDir.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreelanceDir.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RDSContext _context;

        public IndexModel(RDSContext context)
        {
            _context = context;
        }
        
        public IList<Category> Categories { get; set; }
        public IList<Gig> Popular { get; set; }
        public IList<Gig> Suggested { get; set; }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {               
                return RedirectToPage("LandingPage");
            }

            Categories = _context.Categories.ToList();
            return Page();
        }
    }
}
