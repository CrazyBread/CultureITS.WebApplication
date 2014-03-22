using CultureITS.Areas.Admin.Models;
using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Areas.Admin.Controllers
{
    [AuthorizeFilterAttribute]
    public class ExhibitController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Admin/Exhibit/
        public ActionResult Index()
        {
            return View(new ExhibitViewModel(db));
        }

        //
        // GET: /Admin/Exhibit/Edit
        public ActionResult Edit(int? id)
        {
            Exhibit item = null;

            try
            {
                if (id.HasValue)
                {
                    item = db.Exhibits.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Экспонат не найден.");
                }
                else
                {
                    item = new Exhibit();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new ExhibitViewModel(db, item));
        }

        //
        // POST: /Admin/Exhibit/Edit
        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id)
        {
            Exhibit item = null;

            try
            {
                if (id.HasValue)
                {
                    item = db.Exhibits.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Экспонат не найден.");
                }
                else
                {
                    item = new Exhibit();
                }

                TryUpdateModel(item, "Item", new[] { "Name", "Description", "CanNotified", "FullDescription", "Code" });

                if (ModelState.IsValid)
                {
                    if (id == null)
                            db.Exhibits.Add(item);
                    else
                        db.Entry<Exhibit>(item).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new ExhibitViewModel(db, item));
        }

        //
        // GET: /Admin/Exhibit/Delete/5
        public ActionResult Delete(int id)
        {
            var item = db.Exhibits.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new ExhibitViewModel(db, item));
        }

        //
        // POST: /Admin/Exhibit/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var item = db.Exhibits.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            db.Exhibits.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
