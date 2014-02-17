using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        private IContext _context;

        public InstallController(IContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateAccount(AccountModels.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Name = model.Username,
                    Password = Hash.CreateHash(model.Password),
                    Email = model.Email
                };
                _context.Set<User>().Add(user);
                _context.SaveChanges();
                FormsAuthentication.SetAuthCookie(model.Username, true);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}