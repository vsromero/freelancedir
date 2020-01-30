using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class Review : BaseModel
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int GigId { get; set; }
        public Gig Gig { get; set; }

        public string Text { get; set; }

        public bool Positive { get; set; }        
    }
}
