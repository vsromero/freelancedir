using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class Message : BaseModel
    {
        public string SenderId { get; set; }
        public User Sender { get; set; }

        public string ReceiverId { get; set; }
        public User Receiver { get; set; }

        public string MessageText { get; set; }

        public bool Seen { get; set; }

        [DataType(DataType.Date)]
        public DateTime SeenDate { get; set; }
    }
}
