﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Core.Domain;
using EFConvention;
using WebGrease.Css.Extensions;
using YANBE.Library;
using YANBE.Models;

namespace YANBE.Controllers
{
    public class PostController : Controller
    {
        private IContext _context;
        private Markdown _markdown;

        public PostController(IContext context, Markdown markdown)
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
            var model = _context.Set<Post>().FirstOrDefault(x => x.Id == id);
            if (model == null) return RedirectToAction("Index", "Home");
            
            model.Body = Markdown(model.Body, false);
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
                var tags = model.Tags.Split(',');
                for (int i = 0; i < tags.Count(); i++)
                {
                    tags[i] = tags[i].Replace("\"", "").Trim().ToLower();
                }
                
                var post = new Post()
                {
                    Body = model.Body,
                    Title = model.Title,
                    TitleSlug = Regex.Replace(model.Title, @"[^A-Za-z0-9_~]+", "-"),
                    Tags = new Collection<Tag>()
                };

                var foundTags = _context.Set<Tag>().Where(x => tags.Contains(x.Name)).ToList();
                foreach (var tag in tags)
                {
                    if (foundTags.Any(x => x.Name == tag))
                    {
                        post.Tags.Add(foundTags.First(x => x.Name == tag));
                        continue;
                    }
                    var t = new Tag()
                    {
                        Name = tag
                    };
                    post.Tags.Add(t);
                }
                _context.Set<Post>().Add(post);
                _context.SaveChanges();
                return RedirectToAction("View", new {id = post.Id, slug = post.TitleSlug});
            }
            return View(model);
        }

        public ViewResult Edit(int id)
        {
            return null;
        }
    }
}