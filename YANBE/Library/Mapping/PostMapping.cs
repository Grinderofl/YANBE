using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Core.Domain;
using EFConvention;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;
using YANBE.Models;

namespace YANBE.Library.Mapping
{
    public class PostMapping : Profile
    {
        protected override void Configure()
        {
           CreateMap<Post, PostEditModel>()
                .ForMember(x => x.Tags, x => x.MapFrom(c => string.Join(", ", c.Tags.Select(t => t.Name))))
                .ReverseMap()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.TitleSlug, x => x.Ignore())
                .ConvertUsing<PostEditModelMappingConverter>();

            CreateMap<PostModel, Post>().ConvertUsing<PostCreateModelMappingConverter>();
                //.ForMember(x => x.TitleSlug, x => x.MapFrom(c => Regex.Replace(c.Title, @"[^A-Za-z0-9_~]+", "-")));
        }
    }
}
