using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelanceDir.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        public ICollection<Category> Children { get; set; }
    }
}
