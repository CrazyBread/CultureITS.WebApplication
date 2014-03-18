using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Api;
using CultureITS.Models.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
        // GET: /Api/Test
        public ActionResult Test()
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

        //
        // POST: /Api/getTestInfo
        [HttpPost]
        public JsonResult getTestInfo(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<TestInfoIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    return Json(new { success = false, message = "Доступ разрешен только студентам." });
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var test = db.TestMain.Find(dataIn.id);
                if (test == null)
                    return Json(new { success = false, message = "Тест с таким идентификатором не найден." });

                TestInfoOut dataOut = new TestInfoOut();
                dataOut.author = test.Author;
                dataOut.questionsCount = test.Questions.Count();
                dataOut.title = test.Title;
                dataOut.topic = test.Topic;

                dataOut.resultsCount = test.Results.Count(i => i.Student.Id == user.Id);
                dataOut.isOpened = (dataOut.resultsCount == 0) ? false : (test.Results.Count(i => ((i.Student.Id == user.Id) && (!i.Complete))) > 0);
                dataOut.dateLastResult = (dataOut.resultsCount == 0) ? DateTime.Now : test.Results.Where(i => i.Student.Id == user.Id).OrderByDescending(i => i.Date).FirstOrDefault().Date;

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
    }
}
