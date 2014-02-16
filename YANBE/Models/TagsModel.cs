using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domain;

namespace YANBE.Models
{
    public class TagsModel
    {
        public string Tag { get; set; }
        public List<Post> Posts { get; set; }
    }
}