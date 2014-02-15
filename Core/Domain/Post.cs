using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Post : EntityWithCreatedDate
    {
        //public User Author { get; set; }
        public string TitleSlug { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
