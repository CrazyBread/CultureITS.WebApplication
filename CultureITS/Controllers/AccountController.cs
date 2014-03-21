using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
using CultureITS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace CultureITS.Controllers
{
    [AuthorizeFilterAttribute]
    public class AccountController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Account/Profile
        public new ActionResult Profile()
        {
            var user = db.Users.Find(System.Web.HttpContext.Current.Session.GetUser().Id);
            return View(new AccountViewModel(user));
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
                    ModelState.AddModelError(string.Empty, "Пользователь с указанными учётными данными не существует.");

                if (ModelState.IsValid)
                {
                    System.Web.HttpContext.Current.Session.Authorize(user as User);
                    return RedirectToAction("Index", "Home");
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
        public ActionResult RegisterPost(HttpPostedFileBase Photo)
        {
            var item = new Student() { UserRole = AccountStatus.Student };

            try
            {
                TryUpdateModel(item, "RegisterItem", new string[] { "Login", "Name", "Password", "Group", "Course", "Age" });

                if (db.Users.Count(i => i.Login == item.Login) > 0)
                    throw new ArgumentException("Пользователь с таким e-mail уже существует.");

                if (ModelState.IsValid)
                {
                    if (Photo != null && Photo.IsImage())
                    {
                        item.PhotoMime = Photo.ContentType;
                        item.Photo = new WebImage(Photo.InputStream).Resize(300, 300).GetBytes(item.PhotoMime);
                        Photo.InputStream.Read(item.Photo, 0, Photo.ContentLength);
                    }

                    db.Students.Add(item);
                    db.SaveChanges();
                    System.Web.HttpContext.Current.Session.Authorize(item);
                    return RedirectToAction("Profile");
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

        //
        // GET: /Account/Users/1
        public FileContentResult GetImage(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
                return null;
            if (user.PhotoMime == null)
                return null;
            return File(user.Photo, user.PhotoMime);
        }

        //
        // GET: /Account/RemoveImage
        public ActionResult RemoveImage()
        {
            var user = db.Users.Find(System.Web.HttpContext.Current.Session.GetUser().Id);
            if (user.PhotoMime != null)
            {
                user.PhotoMime = null;
                user.Photo = null;
                db.Entry(user).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Profile");
        }

        //
        // POST: /Account/ChangeImage
        [HttpPost]
        public ActionResult ChangeImage(HttpPostedFileBase Photo)
        {
            var user = db.Users.Find(System.Web.HttpContext.Current.Session.GetUser().Id);
            if (Photo.IsImage())
            {
                user.PhotoMime = Photo.ContentType;
                user.Photo = new WebImage(Photo.InputStream).Resize(300, 300).GetBytes(user.PhotoMime);
                Photo.InputStream.Read(user.Photo, 0, Photo.ContentLength);
                db.Entry(user).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Profile");
        }

        //
        // GET: /Account/AutoLogOn
        public ActionResult AutoLogOn(string login)
        {
            var user = db.Users.FirstOrDefault(i => i.Login == login);
            if (user != null)
                System.Web.HttpContext.Current.Session.Authorize(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
