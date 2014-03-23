using CultureITS.Models.Context;
using CultureITS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Controllers
{
    public class ExhibitController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Exhibit/
        public ActionResult Index(string find, int? page)
        {
            return View(new ExhibitViewModel(db.Exhibits, page));
        }

        //
        // GET: /Exhibit/Details/5
        public ActionResult Details(int id)
        {
            var item = db.Exhibits.Find(id);
            if (item == null)
                return RedirectToAction("Index");
            return View(new ExhibitViewModel(item));
        }

        //
        // GET: /Exhibit/GetImage/1
        public FileContentResult GetImage(int id)
        {
            var item = db.Exhibits.Find(id);
            if (item == null)
                return null;
            if (item.ApplicationType == null)
                return null;
            return File(item.ApplicationData, item.ApplicationType);
        }

        //
        // GET: /Exhibit/GetArticleImage/1
        public FileContentResult GetArticleImage(int id)
        {
            var item = db.Articles.Find(id);
            if (item == null)
                return null;
            if (item.ApplicationType == null)
                return null;
            return File(item.ApplicationData, item.ApplicationType);
        }
    }
}
