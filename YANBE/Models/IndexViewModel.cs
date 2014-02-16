using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domain;

namespace YANBE.Models
{
    public class IndexViewModel
    {
        public List<Post> Posts { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public IEnumerable<Page> Pages { get; set; }
        public List<Tag> Tags { get; set; }
    }
}