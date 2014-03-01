using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Core.Domain;
using EFConvention;
using WebGrease.Css.Extensions;
using YANBE.Models;

namespace YANBE.Library.Mapping
{
    public class PostMapping : Profile
    {
        private IContext _context;

        public PostMapping(IContext context)
        {
            _context = context;
        }

        protected override void Configure()
        {
           CreateMap<Post, PostEditModel>()
                .ForMember(x => x.Tags, x => x.MapFrom(c => string.Join(", ", c.Tags.Select(t => t.Name))))
                .ReverseMap()
                .ForMember(x => x.Tags, x => x.ResolveUsing(c => MapTags(c.Tags, c.Id)))
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.TitleSlug, x => x.Ignore());

            CreateMap<PostModel, Post>()
                .ForMember(x => x.Tags, x => x.ResolveUsing(c => MapTags(c.Tags)))
                .ForMember(x => x.TitleSlug, x => x.MapFrom(c => Regex.Replace(c.Title, @"[^A-Za-z0-9_~]+", "-")));
        }

        private List<Tag> MapTags(string tagList, int id = 0)
        {
            var tags = tagList.Split(',');
            for (var i = 0; i < tags.Count(); i++)
                tags[i] = tags[i].Replace("\"", "").Trim().ToLower();

            var foundTags = _context.Set<Tag>().Where(x => tags.Contains(x.Name)).ToList();
            if (id != 0)
            {
                var post = _context.Set<Post>().Include(x => x.Tags).First(x => x.Id == id);
                foreach (var tag in post.Tags.Where(tag => tags.All(x => tag.Name != x)))
                    post.Tags.Remove(tag);
            }
            return
                tags.Select(
                    tag =>
                        foundTags
                            .FirstOrDefault(f => tags.Contains(f.Name)) ?? new Tag() {Name = tag})
                    .ToList();
        }
    }
}
