using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Core.Domain;
using EFConvention;
using WebGrease.Css.Extensions;
using YANBE.Library;
using YANBE.Models;

namespace YANBE.Controllers
{
    public class PostController : Controller
    {
        private DbContext _context;
        private Markdown _markdown;
        
        public PostController(DbContext context, Markdown markdown)
        {
            _context = context;
            _markdown = markdown;
        
        }

        public ActionResult Index()
        {
            return View();
        }

        public string Markdown(string source, bool external = true)
        {
            return _markdown.Transform(source, external);
        }

        public ActionResult View(int id, string slug = "")
        {
            var model = _context.Set<Post>().Include(x => x.Tags).FirstOrDefault(x => x.Id == id);
            if (model == null) return RedirectToAction("Index", "Home");
            
            model.Body = Markdown(model.Body, false);
            return View(model);
        }

        [Authorize]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(PostModel model)
        {
            if (ModelState.IsValid)
            {
                var post = Mapper.Map<Post>(model);
                _context.Set<Post>().Add(post);
                _context.SaveChanges();
                return RedirectToAction("View", new {id = post.Id, slug = post.TitleSlug});
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = _context.Set<Post>().Include(x => x.Tags).FirstOrDefault(x => x.Id == id);
            if (model == null) return RedirectToAction("Index", "Home");
            return View(Mapper.Map<PostEditModel>(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(PostEditModel model)
        {
            if (ModelState.IsValid)
            {
                var foundPost = _context.Set<Post>().Include(x => x.Tags).FirstOrDefault(x => x.Id == model.Id);
                if (foundPost == null) return RedirectToAction("Index", "Home");
                
                Mapper.Map(model, foundPost);
                _context.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return View(model);
        }
    }
}