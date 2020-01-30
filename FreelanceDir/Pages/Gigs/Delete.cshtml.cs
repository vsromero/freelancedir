using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FreelanceDir.Data;
using FreelanceDir.Models;

namespace FreelanceDir.Pages.Gigs
{
    public class DeleteModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;

        public DeleteModel(FreelanceDir.Data.RDSContext context)
        {
            _context = context;
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
                .Include(g => g.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Gig == null)
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

            Gig = await _context.Gigs.FindAsync(id);

            if (Gig != null)
            {
                _context.Gigs.Remove(Gig);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Users/Dashboard");
        }
    }
}
