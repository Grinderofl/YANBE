using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Domain;
using EFConvention;
using YANBE.Library;
using YANBE.Models;

namespace YANBE.Controllers
{
    public class HomeController : Controller
    {
        private DbContext _context;
        private Markdown _markdown;

        public HomeController(DbContext context, Markdown markdown)
        {
            _context = context;
            _markdown = markdown;
        }

        
        public ActionResult Index(int page = 1)
        {
            var query = _context.Set<Post>().OrderByDescending(x => x.PublishedDate);

            var model = new IndexViewModel()
            {
                Count = query.Count(),
                Page = page,
                Posts = query.Skip((page-1)*GlobalConfiguration.ItemsPerPage).Take(GlobalConfiguration.ItemsPerPage).Include(x => x.Tags).ToList(),
                Pages = _context.Set<Page>().OrderByDescending(x => x.PublishedDate).ToList(),
                Tags = _context.Set<Tag>().OrderByDescending(x => x.Posts.Count).Take(5).ToList()
            };
            model.Posts.ForEach(x => x.Body = _markdown.Transform(x.Body, false));
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}