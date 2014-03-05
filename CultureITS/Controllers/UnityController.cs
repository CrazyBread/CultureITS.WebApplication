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
    public class UnityController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Unity/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Unity/GameObjectMark/5
        [HttpPost]
        public JsonResult GameObjectMark(int id)
        {
            var item = db.GameObjects.Find(id);
            if (item == null)
                return Json(new { success = false, message = "Игровой объект не найден в системе." });

            var sessionUser = System.Web.HttpContext.Current.Session.GetUser();
            if (sessionUser == null)
                return Json(new { success = false, message = "Доступ неавторизованным пользователям запрещён." });

            var user = db.Users.Find(sessionUser.Id);
            if (user is Student == false)
                return Json(new { success = false, message = "Пользователь не является студентом." });

            Student student = user as Student;
            if (student.GameObjects.Count(i => i.Id == item.Id) > 0)
                student.GameObjects.Remove(item);
            else
                student.GameObjects.Add(item);
            db.SaveChanges();

            return Json(new { success = true });
        }
    }
}
