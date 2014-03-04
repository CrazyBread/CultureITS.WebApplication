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
    public class GameObjectController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Admin/GameObject/
        public ActionResult Index()
        {
            return View(new GameObjectViewModel(db));
        }

        //
        // GET: /Admin/GameObject/Edit
        public ActionResult Edit(int? id)
        {
            GameObject item = null;

            try
            {
                if (id.HasValue)
                {
                    item = db.GameObjects.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Объект не найден");
                }
                else
                {
                    item = new GameObject();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(new GameObjectViewModel(db, item));
        }

        //
        // POST: /Admin/GameObject/Edit
        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id)
        {
            GameObject item = null;

            try
            {
                if (id.HasValue)
                {
                    item = db.GameObjects.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Объект не найден");
                }
                else
                {
                    item = new GameObject();
                }

                TryUpdateModel(item, "Item", new[] { "Name", "Description", "CanNotified", "FullDescription" });

                if (ModelState.IsValid)
                {
                    if (id == null)
                            db.GameObjects.Add(item);
                    else
                        db.Entry<GameObject>(item).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new GameObjectViewModel(db, item));
        }
    }
}
