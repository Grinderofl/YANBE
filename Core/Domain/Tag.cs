using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Tag : Entity
    {
        public ICollection<Post> Posts { get; set; } 
        public string Name { get; set; }
    }
}
