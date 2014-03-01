using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Core.Domain;
using Core.Library;
using EFConvention;
using YANBE.Models;

namespace YANBE.Controllers
{
    public class AccountController : Controller
    {
        private IContext _context;

        public AccountController(IContext context)
        {
            _context = context;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountModels.LoginModel model)
        {
            var found = _context.Set<User>().FirstOrDefault(x => x.Name == model.Username);
            if (found == null)
                ModelState.AddModelError("Name", "Invalid user or password");

            if (ModelState.IsValid)
            {
                if (!Hash.ValidatePassword(model.Password, found.Password))
                    ModelState.AddModelError("Name", "Invalid user or password");

                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}