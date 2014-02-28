using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CultureITS.Models;
using CultureITS.Models.Context;
using CultureITS.Areas.Admin.Models;

namespace CultureITS.Areas.Admin.Controllers
{
    [AuthorizeFilterAttribute]
    public class UserController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Admin/User/
        public ActionResult Index()
        {
            return View(new UserViewModel(db));
        }

        //
        // GET: /Admin/User/Details/5
        public ActionResult Details(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /Admin/User/Edit
        public ActionResult Edit(int? id)
        {
            User user = null;

            try
            {
                if (id.HasValue)
                {
                    user = db.Users.SingleOrDefault(i => i.Id == id);
                    if (user == null)
                        throw new ArgumentException("Пользователь не найден");
                }
                else
                {
                    user = new User();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(new UserViewModel(db, user));
        }

        //
        // POST: /Admin/User/Edit
        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id)
        {
            User user = null;

            try
            {
                if (id.HasValue)
                {
                    user = db.Users.SingleOrDefault(i => i.Id == id);
                    if (user == null)
                        throw new ArgumentException("Пользователь не найден");
                    TryUpdateModel(user, "Item", new[] { "Login", "Name", "UserRole" });
                }
                else
                {
                    user = new User();
                    TryUpdateModel(user, "Item", new[] { "Login", "Password", "Name", "UserRole" });
                }

                if (ModelState.IsValid)
                {
                    if (id == null)
                        db.Users.Add(user);
                    else
                        db.Entry<User>(user).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new UserViewModel(db, user));
        }

        //
        // GET: /Admin/User/Delete/5
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new UserViewModel(db) { Item = user });
        }

        //
        // POST: /Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}