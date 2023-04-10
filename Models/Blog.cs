using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A10BlogsPosts.Models
{
    public class Blog
    {
        public int BlogId { get; set; }

        [StringLength(100)] //Will limit length of Name field to 100
        public string Name { get; set; }

        //navigation property
        public virtual List<Post> Posts { get; set; }
    }
}
