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
            /* Convert to list in order to use starting price and positive reviews unmapped properties */
            var gigs = _context.Gigs
                .Include(g => g.User)
                .Include(g => g.Category)
                .Include(g => g.Packages)
                .Where(g => g.Active)
                .AsNoTracking()
                .ToList(); 

            if (!string.IsNullOrEmpty(SearchString))
            {
                gigs = gigs.Where(g => g.Title.Contains(SearchString) || g.Description.Contains(SearchString)).ToList();
            }
            if (CategoryId.HasValue)
            {
                gigs = gigs.Where(g => g.CategoryId == CategoryId || g.Category.ParentId == CategoryId).ToList();
            }
            if (Descending == true)
            {
                gigs = gigs.OrderByDescending(g => g.Id).ToList();
            }
            if (SortBy.HasValue)
            {
                Comparison<Gig> comp;
                switch (SortBy)
                {
                    case SortEnum.Price:
                        comp = (x, y) => x.StartingPrice.CompareTo(y.StartingPrice);
                        break;
                    case SortEnum.Date:
                        comp = (x, y) => x.CreatedDate.CompareTo(y.CreatedDate);
                        break;
                    case SortEnum.Rating:
                        comp = (x, y) => x.PositivePercentage.CompareTo(y.PositivePercentage);
                        break;
                    case SortEnum.Views:
                        comp = (x, y) => x.TotalViewsCount.CompareTo(y.TotalViewsCount);
                        break;
                    case SortEnum.Delivered:
                        comp = (x, y) => x.JobsCompleted.CompareTo(y.JobsCompleted);
                        break;
                    default:
                        comp = (x, y) => x.JobsCompleted.CompareTo(y.JobsCompleted);
                        break;
                }

                gigs.Sort(comp);
                if (Descending == true)
                {
                    gigs.Reverse();
                }
            }

            Categories = await _context.Categories
                .Include(c => c.Children)
                .AsNoTracking()
                .ToListAsync();
            Gigs = gigs;
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
