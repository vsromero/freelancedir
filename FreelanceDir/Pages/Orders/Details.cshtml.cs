using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FreelanceDir.Data;
using FreelanceDir.Models;

namespace FreelanceDir.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;

        public DetailsModel(FreelanceDir.Data.RDSContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Gig)
                .Include(o => o.Package)
                .Include(o => o.Seller).FirstOrDefaultAsync(m => m.Id == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
