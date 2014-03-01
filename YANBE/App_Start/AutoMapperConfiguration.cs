using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Core.Domain;
using YANBE.Controllers;

namespace YANBE
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x => GetConfiguration(Mapper.Configuration));
        }

        private static void GetConfiguration(IConfiguration configuration)
        {
            var profiles =
                typeof (PostController).Assembly.GetTypes()
                    .Where(x => typeof (Profile).IsAssignableFrom(x))
                    .Union(typeof (Post).Assembly.GetTypes().Where(x => typeof (Profile).IsAssignableFrom(x)));
            foreach (var profile in profiles)
                configuration.AddProfile(DependencyResolver.Current.GetService(profile) as Profile);

        }
    }
}