using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class Order : BaseModel
    {
        public int GigId { get; set; }
        public Gig Gig { get; set; }

        public string SellerId { get; set; }
        public User Seller { get; set; }

        public string BuyerId { get; set; }
        public User Buyer { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Note { get; set; }

        public Status Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DeliveredDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal AmountPaidByBuyer { get; set; }

        [DataType(DataType.Currency)]
        public decimal AmountPaidToSeller { get; set; }
    }

    public enum Status
    {
        Completed,
        Cancelled,
        Due,
    }
}
