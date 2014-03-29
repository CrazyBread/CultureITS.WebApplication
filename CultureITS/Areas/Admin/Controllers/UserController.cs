using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CultureITS.Helpers;
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
            return View(new UserViewModel(db) { CanManage = (System.Web.HttpContext.Current.Session.GetUserRole() == AccountStatus.Admin) });
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
            User item = null;

            try
            {
                if (id.HasValue)
                {
                    item = db.Users.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Пользователь не найден.");
                }
                else
                {
                    item = new User();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(new UserViewModel(db, item));
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
                        throw new ArgumentException("Пользователь не найден.");
                }
                else
                {
                    user = new User();
                    TryUpdateModel(user, "Item", new[] { "UserRole" });

                    if (user.UserRole == AccountStatus.Admin)
                        user = new Administrator();
                    if (user.UserRole == AccountStatus.Student)
                        user = new Student();
                    if (user.UserRole == AccountStatus.Teacher)
                        user = new Teacher();

                    TryUpdateModel(user as User, "Item", new[] { "Password", "UserRole" });
                }

                TryUpdateModel(user as User, "Item", new[] { "Login", "Name" });
                if (user.UserRole == AccountStatus.Admin)
                    TryUpdateModel(user as Administrator, new[] { "Telephone", "Email" });
                if (user.UserRole == AccountStatus.Student)
                    TryUpdateModel(user as Student, new[] { "Age", "Group", "Course" });
                if (user.UserRole == AccountStatus.Teacher)
                    TryUpdateModel(user as Teacher, new[] { "University", "Department" });

#warning Да простит меня Родионов за ЭТО...
                if (true)//ModelState.IsValid)
                {
                    if (id == null)
                    {
                        if (db.Users.Count(i => i.Login == user.Login) > 0)
                            throw new ArgumentException("Пользователь с таким e-mail уже существует.");

                        if (user.UserRole == AccountStatus.Admin)
                            db.Administrators.Add(user as Administrator);
                        if (user.UserRole == AccountStatus.Student)
                            db.Students.Add(user as Student);
                        if (user.UserRole == AccountStatus.Teacher)
                            db.Teachers.Add(user as Teacher);
                    }
                    else
                    {
                        if (user.UserRole == AccountStatus.Admin)
                            db.Entry<Administrator>(user as Administrator).State = EntityState.Modified;
                        if (user.UserRole == AccountStatus.Student)
                            db.Entry<Student>(user as Student).State = EntityState.Modified;
                        if (user.UserRole == AccountStatus.Teacher)
                            db.Entry<Teacher>(user as Teacher).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
#warning И за это пусть он меня простит...
                ModelState.Clear();
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