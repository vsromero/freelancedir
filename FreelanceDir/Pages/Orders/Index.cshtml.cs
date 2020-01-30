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
using Microsoft.EntityFrameworkCore;

namespace FreelanceDir.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet(int pid)
        {
            Package = _context.Packages
                .Include(p => p.Gig)
                    .ThenInclude(g => g.User)
                .Include(p => p.Gig)
                    .ThenInclude(g => g.Category)
                .FirstOrDefault(p => p.Id == pid);

            return Page();
        }

        public Package Package;
        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public int DeliveryDays { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.Buyer = await _userManager.GetUserAsync(User);
            Order.Status = Status.Due;
            Order.DueDate = DateTime.UtcNow.AddDays(DeliveryDays);

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}