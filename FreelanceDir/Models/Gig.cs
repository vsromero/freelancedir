﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class Gig : BaseModel
    {
        public bool Active { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int JobsCompleted { get; set; }

        public int WeekViewsCount { get; set; }

        public int TotalViewsCount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Package> Packages { get; set; }

        public ICollection<Review> Reviews { get; set; }

        [NotMapped]
        public decimal PositivePercentage {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                {
                    return 0;
                }
                var positiveReviews = Reviews.Where(r => r.Positive).Count();
                var totalReviews = Reviews.Count;
                return Decimal.Divide(positiveReviews, totalReviews);
            }
        }

        [NotMapped]
        [DataType(DataType.Currency)]
        public decimal StartingPrice
        {
            get
            {
                if (Packages == null || Packages.Count == 0)
                {
                    return 0;
                }
                return Packages.Min(p => p.Price);
            }
        }
    }
}
