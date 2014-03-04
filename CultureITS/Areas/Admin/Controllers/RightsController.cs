using CultureITS.Areas.Admin.Models;
using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Areas.Admin.Controllers
{
    [AuthorizeFilterAttribute]
    public class RightsController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Admin/Rights/
        public ActionResult Index()
        {
            return View(new RightsViewModel(db));
        }

        //
        // GET: /Admin/Rights/Change/15
        public ActionResult Change(int id)
        {
            var item = db.AccessRights.Find(id);
            if (item != null)
            {
                item.IsAllowed = !item.IsAllowed;
                db.Entry<AccessRight>(item).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Admin/Rights/Remove/15
        public ActionResult Remove(int id)
        {
            var item = db.AccessRights.Find(id);
            if (item != null)
            {
                db.AccessRights.Remove(item);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
