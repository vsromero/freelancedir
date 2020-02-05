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

namespace FreelanceDir.Pages.Users
{
    public class DashboardModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Gig> Gigs { get;set; }
        public IList<Order> LatestOrders { get; set; }        
        public IList<Message> LatestMessages { get; set; }
        public IList<Review> LatestReviews { get; set; }

        public async Task OnGetAsync()
        {
            Gigs = await _context.Gigs
                .Where(g => g.UserId == _userManager.GetUserId(User))
                .Include(g => g.User)
                .Include(g => g.Category)
                .Include(g => g.Packages)
                .Include(g => g.Reviews)
                .ToListAsync();

            LatestOrders = await _context.Orders
                .Where(o => o.SellerId == _userManager.GetUserId(User) || o.BuyerId == _userManager.GetUserId(User))
                .Include(o => o.Buyer)
                .Include(o => o.Gig)
                .Include(o => o.Package)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();

            LatestMessages = await _context.Messages
                .Where(m => m.ReceiverId == _userManager.GetUserId(User))
                .Include(m => m.Sender)
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();

            LatestReviews = await _context.Reviews
                .Where(r => r.Gig.UserId == _userManager.GetUserId(User))
                .Include(r => r.Gig)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }
    }
}
