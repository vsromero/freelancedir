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
    public class IndexModel : PageModel
    {
        private readonly RDSContext _context;

        public IndexModel(RDSContext context)
        {
            _context = context;
        }

        public IList<Gig> Gigs { get;set; }
        public IList<Category> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }
        [BindProperty(SupportsGet = true)]
        public SortEnum? SortBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? Descending { get; set; }

        public async Task OnGetAsync()
        {
            var gigs = _context.Gigs
                .Include(g => g.User)
                .Include(g => g.Category)
                .Include(g => g.Packages)
                .Where(g => g.Active)
                .Select(g => g);

            if (!string.IsNullOrEmpty(SearchString))
            {
                gigs = gigs.Where(g => g.Title.Contains(SearchString) || g.Description.Contains(SearchString));
            }
            if (CategoryId.HasValue)
            {
                gigs = gigs.Where(g => g.CategoryId == CategoryId || g.Category.ParentId == CategoryId);
            }
            if (Descending == true)
            {
                gigs = gigs.OrderByDescending(g => g.Id);
            }
            if (SortBy.HasValue)
            {
                System.Linq.Expressions.Expression<Func<Gig, object>> key;
                switch (SortBy)
                {
                    case SortEnum.Price:
                        key = g => g.StartingPrice;
                        break;
                    case SortEnum.Date:
                        key = g => g.CreatedDate;
                        break;
                    case SortEnum.Rating:
                        key = g => g.PositivePercentage;
                        break;
                    case SortEnum.Views:
                        key = g => g.TotalViewsCount;
                        break;
                    case SortEnum.Delivered:
                        key = g => g.JobsCompleted;
                        break;
                    default:
                        key = g => g.JobsCompleted;
                        break;
                }
                gigs = Descending == true ? gigs.OrderByDescending(key) : gigs.OrderBy(key);                
            }

            Categories = await _context.Categories
                .Include(c => c.Children)
                .ToListAsync();
            Gigs = await gigs.ToListAsync();
        }
    }

    public enum SortEnum
    {
        Price,
        Date,
        Rating,
        Views,
        Delivered,
    }
}
