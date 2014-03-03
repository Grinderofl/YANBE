using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AutoMapper;
using Core.Domain;
using YANBE.Models;

namespace YANBE.Library.Mapping
{
    public class PostEditModelMappingConverter : TypeConverter<PostEditModel, Post>
    {
        private readonly DbContext _context;
        
        public PostEditModelMappingConverter(DbContext context)
        {
            _context = context;
        }

        protected override Post ConvertCore(PostEditModel source)
        {
            var tags = source.Tags.Split(',');
            for (var i = 0; i < tags.Count(); i++)
                tags[i] = tags[i].Replace("\"", "").Trim().ToLower();

            var foundPost = _context.Set<Post>().Include(x => x.Tags).First(x => x.Id == source.Id);

            foreach (var tag in foundPost.Tags.ToArray())
                if (!tags.Contains(tag.Name))
                    foundPost.Tags.Remove(tag);

            var tagsInPost = foundPost.Tags.Select(x => x.Name).ToList();

            var foundTags =
                _context.Set<Tag>()
                    .Where(x => tags.Contains(x.Name) && !tagsInPost.Contains(x.Name))
                    .ToList();
            foundTags.ForEach(foundPost.Tags.Add);
            foreach (var tag in tags.Where(c => foundPost.Tags.All(t => t.Name != c)))
                foundPost.Tags.Add(new Tag() {Name = tag});

            foundPost.Title = source.Title;
            foundPost.Body = source.Body;

            return foundPost;
        }
    }
}