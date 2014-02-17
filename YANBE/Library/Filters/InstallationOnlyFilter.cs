using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Core.Domain;
using EFConvention;

namespace YANBE.Library.Filters
{
    public class InstallationOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (DependencyResolver.Current.GetService<IContext>().Set<User>().Any())
            {
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Home", action = "Index"}));
            }
        }
    }
}