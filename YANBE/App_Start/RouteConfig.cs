using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YANBE
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Empty", "", new {controller = "Home", action = "Index", page = 1});
            routes.MapRoute("PostEdit", "Post/Edit/{id}", new {controller = "Post", action = "Edit"});
            routes.MapRoute("Post", "Post/{id}/{slug}", new {controller = "Post", action = "View"});
            routes.MapRoute("Page", "Page/{id}/{slug}", new { controller = "Page", action = "View" });
            routes.MapRoute("Tag", "Tags/{id}", new { controller = "Tags", action = "Details" });
            routes.MapRoute("Index", "page/{page}",
                new { controller = "Home", action = "Index", page = UrlParameter.Optional }, new { page = @"^[0-9]+$" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
