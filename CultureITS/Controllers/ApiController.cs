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
    public class ApiController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Api/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Api/markGameObject/5
        [HttpPost]
        public JsonResult markGameObject(int id)
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

        //
        // POST: /Api/checkGameObject/5
        [HttpPost]
        public JsonResult checkGameObject(int id)
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
                return Json(new { success = true, state = true });
            return Json(new { success = true, state = false });
        }

        //
        // POST: /Api/getGameObjectInfo/5
        [HttpPost]
        public JsonResult getGameObjectInfo(int id, bool full)
        {
            var item = db.GameObjects.Find(id);
            if (item == null)
                return Json(new { success = false, message = "Игровой объект не найден в системе." });

            var sessionUser = System.Web.HttpContext.Current.Session.GetUser();
            if (sessionUser == null)
                return Json(new { success = false, message = "Доступ неавторизованным пользователям запрещён." });

            if (full)
                return Json(new { success = true, name = item.Name, description = item.Description, canNotified = item.CanNotified, fullDescription = item.FullDescription });
            return Json(new { success = true, name = item.Name, description = item.Description, canNotified = item.CanNotified }); 
        }
    }
}
