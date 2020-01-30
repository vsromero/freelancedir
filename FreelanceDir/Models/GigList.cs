using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class GigList : BaseModel
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }

        public ICollection<Gig> Gigs { get; set; }
    }
}
