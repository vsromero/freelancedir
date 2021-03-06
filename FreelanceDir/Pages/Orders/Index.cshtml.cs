﻿using System;
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
    public class IndexModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;

        public IndexModel(FreelanceDir.Data.RDSContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Gig)
                .Include(o => o.Package)
                .Include(o => o.Seller).ToListAsync();
        }
    }
}
