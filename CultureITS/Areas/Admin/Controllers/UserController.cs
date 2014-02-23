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
        public ActionResult Details(string id = null)
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
        public ActionResult Edit(string id)
        {
            var viewModel = new UserViewModel(db);
            if (id != null) viewModel.Item = db.Users.SingleOrDefault(i => i.UserName == id);
            return View(viewModel);
        }

        //
        // POST: /Admin/User/Edit
        [HttpPost, ActionName("Edit")]
        public ActionResult CreatePost(string id)
        {
            User user = (id == null) ? new User() : db.Users.SingleOrDefault(i => i.UserName == id);
            TryUpdateModel(user, "Item", new[] { "UserName", "Password", "Name", "UserRole" });

            if (ModelState.IsValid)
            {
                if (id == null)
                    db.Users.Add(user);
                else
                    db.Entry<User>(user).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(new UserViewModel(db) { Item = user });
        }

        //
        // GET: /Admin/User/Delete/5
        public ActionResult Delete(string id = null)
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
        public ActionResult DeletePost(string id)
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