using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using AutoMapper;
using Core.Domain;
using YANBE.Models;

namespace YANBE.Library.Mapping
{
    public class PostCreateModelMappingConverter : TypeConverter<PostModel, Post>
    {
        private readonly DbContext _context;
        
        public PostCreateModelMappingConverter(DbContext context)
        {
            _context = context;
        }

        protected override Post ConvertCore(PostModel source)
        {
            var tags = source.Tags.Split(',');
            for (var i = 0; i < tags.Count(); i++)
                tags[i] = tags[i].Replace("\"", "").Trim().ToLower();
            var post = new Post()
            {
                Title = source.Title,
                Body = source.Body,
                TitleSlug = Regex.Replace(source.Title, @"[^A-Za-z0-9_~]+", "-"),
                Tags = new List<Tag>()
            };

            var foundTags =
                _context.Set<Tag>()
                    .Where(x => tags.Contains(x.Name)).ToList();
            foundTags.ForEach(post.Tags.Add);
            foreach (var tag in tags.Where(c => post.Tags.All(t => t.Name != c)))
                post.Tags.Add(new Tag() {Name = tag});
            return post;
        }
    }
}