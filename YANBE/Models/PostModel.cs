using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YANBE.Models
{
    public class PostModel
    {
        public string Title { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string Tags { get; set; }
    }
}