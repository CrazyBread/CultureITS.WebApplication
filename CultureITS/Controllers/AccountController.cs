using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
using CultureITS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Controllers
{
    [AuthorizeFilterAttribute]
    public class AccountController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View(new AccountViewModel(System.Web.HttpContext.Current.Session.GetUser()));
        }

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost, ActionName("Login")]
        public ActionResult LoginPost()
        {
            var model = new UserLogin();
            if (TryUpdateModel(model, new string[] { "UserName", "Password" }))
            {
                var user = db.Users.SingleOrDefault(i => ((i.UserName == model.UserName) && (i.Password == model.Password)));
                if (user == null)
                    ModelState.AddModelError(string.Empty, "Пользователь с указанными учётными данными не существует");

                if (ModelState.IsValid)
                {
                    System.Web.HttpContext.Current.Session.Authorize(user as User);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/Logout
        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Session.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
