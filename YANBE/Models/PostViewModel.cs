using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domain;

namespace YANBE.Models
{
    public class PostViewModel
    {
        public List<Post> Posts { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
    }
}