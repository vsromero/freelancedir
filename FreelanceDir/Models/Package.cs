using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class Package : BaseModel
    {
        public int GigId { get; set; }
        public Gig Gig { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Details { get; set; }

        [Display(Name = "Days for Delivery")]
        public int DeliveryDays { get; set; }

        [Display(Name = "Number of Revision")]
        public int Revisions { get; set; }
    }
}
