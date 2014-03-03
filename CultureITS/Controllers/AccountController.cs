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
            if (TryUpdateModel(model, new string[] { "Login", "Password" }))
            {
                var user = db.Users.SingleOrDefault(i => ((i.Login == model.Login) && (i.Password == model.Password)));
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
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost, ActionName("Register")]
        public ActionResult RegisterPost()
        {
            var item = new Student() { UserRole = AccountStatus.Student };

            try
            {
                TryUpdateModel(item, "RegisterItem", new string[] { "Login", "Name", "Password", "Group", "Course", "Age" });

                if (db.Users.Count(i => i.Login == item.Login) > 0)
                    throw new ArgumentException("Пользователь с таким e-mail уже существует.");

                if (ModelState.IsValid)
                {
                    db.Students.Add(item);
                    db.SaveChanges();
                    System.Web.HttpContext.Current.Session.Authorize(item);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(new AccountViewModel(item));
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
