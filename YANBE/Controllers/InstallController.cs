using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;
using Core.Domain;
using Core.Library;
using EFConvention;
using Microsoft.Ajax.Utilities;
using YANBE.Library.Filters;
using YANBE.Models;

namespace YANBE.Controllers
{
    [InstallationOnly]
    public class InstallController : Controller
    {
        public InstallController()
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Settings()
        {
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
            var model = new SettingsModel()
            {
                ConnectionString = section.ConnectionStrings["DefaultConnection"].ConnectionString
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsModel model)
        {
            if (ModelState.IsValid)
            {
                var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
                section.ConnectionStrings["DefaultConnection"].ConnectionString = model.ConnectionString;
                configuration.Save();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateAccount(AccountModels.RegisterModel model)
        {
            var context = DependencyResolver.Current.GetService<IContext>();
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Name = model.Username,
                    Password = Hash.CreateHash(model.Password),
                    Email = model.Email
                };
                context.Set<User>().Add(user);
                context.SaveChanges();
                FormsAuthentication.SetAuthCookie(model.Username, true);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}