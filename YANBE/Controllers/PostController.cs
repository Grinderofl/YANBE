using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Core.Domain;
using EFConvention;
using MarkdownSharp;
using Pygments;
using YANBE.Library;
using YANBE.Models;

namespace YANBE.Controllers
{
    public class PostController : Controller
    {
        private IContext _context;
        private Highlighter _highlighter;

        public PostController(IContext context, Highlighter highlighter)
        {
            _context = context;
            _highlighter = highlighter;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult View(int id, string slug = "")
        {
            var model = _context.Set<Post>().FirstOrDefault(x => x.Id == id);
            if (model == null) return RedirectToAction("Index", "Home");

            var md = new Markdown();
            var transformed = md.Transform(model.Body);
            
            var start = "<p>```c#";
            var end = "```</p>";

            while (transformed.IndexOf(start, StringComparison.Ordinal) > -1)
            {
                var startIndex = transformed.IndexOf(start, StringComparison.Ordinal);
                var endIndex = transformed.IndexOf(end, startIndex, StringComparison.Ordinal);
                var firstHalf = transformed.Substring(0, startIndex);
                var secondHalf = transformed.Substring(endIndex + end.Length);
                var text = transformed.Substring(startIndex + start.Length, endIndex - startIndex - start.Length).Replace("&gt;", ">").Replace("&lt;", "<");
                transformed = firstHalf + _highlighter.HighlightToHtml(HtmlRemoval.StripTagsRegexCompiled(text), "c#", "vs", lineNumberStyle:LineNumberStyle.table, fragment:false) + secondHalf;
            }

            model.Body = transformed;
            return View(model);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PostModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new Post()
                {
                    Body = model.Body,
                    Title = model.Title,
                    TitleSlug = Regex.Replace(model.Title, @"[^A-Za-z0-9_~]+", "-")
                };
                _context.Set<Post>().Add(post);
                _context.SaveChanges();
                return RedirectToAction("View", new {id = post.Id, slug = post.TitleSlug});
            }
            return View(model);
        }
    }
}