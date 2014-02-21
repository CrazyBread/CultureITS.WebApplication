using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
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
            return View();
        }

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }
        
        //
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(i => ((i.UserName == model.UserName) && (i.Password == model.Password)));
                if (user == null)
                    ModelState.AddModelError(string.Empty, "Пользователь с указанными учётными данными не существует");

                if (ModelState.IsValid)
                {
                    Session.Authorize(user as User);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/Logout
        public ActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
