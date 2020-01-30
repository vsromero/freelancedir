using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        [InverseProperty("Sender")]
        public ICollection<Message> MessagesSent { get; set; }

        [InverseProperty("Receiver")]
        public ICollection<Message> MessagesReceived { get; set; }

        [InverseProperty("User")]
        public ICollection<Gig> Gigs { get; set; }

        public ICollection<GigList> GigLists { get; set; }

        [InverseProperty("Buyer")]
        public ICollection<Order> OrdersSent { get; set; }

        [InverseProperty("Seller")]
        public ICollection<Order> OrdersReceived { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
