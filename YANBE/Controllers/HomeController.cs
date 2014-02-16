using System;
using System.Collections.Generic;
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
        private IContext _context;
        private Markdown _markdown;

        public HomeController(IContext context, Markdown markdown)
        {
            _context = context;
            _markdown = markdown;
        }

        public ActionResult Index(int page = 1)
        {
            var query = _context.Set<Post>().OrderByDescending(x => x.PublishedDate);

            var model = new PostViewModel()
            {
                Count = query.Count(),
                Page = 1,
                Posts = query.Skip((page-1)*10).Take(10).ToList()
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