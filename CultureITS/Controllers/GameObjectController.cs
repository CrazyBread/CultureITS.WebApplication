using CultureITS.Models.Context;
using CultureITS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Controllers
{
    public class GameObjectController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /GameObject/
        public ActionResult Index(string find, int? page)
        {
            return View(new GameObjectViewModel(db.GameObjects, page));
        }

        //
        // GET: /GameObject/Details/5
        public ActionResult Details(int id)
        {
            var item = db.GameObjects.Find(id);
            if (item == null)
                return RedirectToAction("Index");
            return View(new GameObjectViewModel(item));
        }
    }
}
