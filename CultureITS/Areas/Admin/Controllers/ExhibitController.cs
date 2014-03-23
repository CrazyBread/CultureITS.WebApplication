using CultureITS.Areas.Admin.Models;
using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
        public ActionResult EditPost(int? id, HttpPostedFileBase Photo)
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

                TryUpdateModel(item, "Item", new[] { "Name", "Description", "Location", "Code" });

                if (ModelState.IsValid)
                {
                    if (Photo != null && Photo.IsImage())
                    {
                        item.ApplicationType = Photo.ContentType;
                        item.ApplicationData = new WebImage(Photo.InputStream).Resize(300, 300).GetBytes(item.ApplicationType);
                        Photo.InputStream.Read(item.ApplicationData, 0, Photo.ContentLength);
                    }

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
        // POST: /Admin/Exhibit/ChangeImage
        [HttpPost]
        public ActionResult ChangeImage(int id, HttpPostedFileBase Photo)
        {
            var item = db.Exhibits.Find(id);
            if (item == null)
                return RedirectToAction("Index");

            if (Photo.IsImage())
            {
                if (Photo != null && Photo.IsImage())
                {
                    item.ApplicationType = Photo.ContentType;
                    item.ApplicationData = new WebImage(Photo.InputStream).Resize(300, 300).GetBytes(item.ApplicationType);
                    Photo.InputStream.Read(item.ApplicationData, 0, Photo.ContentLength);
                }
                db.Entry<Exhibit>(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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

        //
        // GET: /Admin/Exhibit/RemoveImage
        public ActionResult RemoveImage(int id)
        {
            var item = db.Exhibits.Find(id);
            if (item == null)
                return RedirectToAction("Index");
            if (item.ApplicationType == null)
                return RedirectToAction("Index");

            item.ApplicationData = null;
            item.ApplicationType = null;
            db.Entry(item).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /Admin/Exhibit/Articles/5
        public ActionResult Articles(int id)
        {
            var item = db.Exhibits.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new ExhibitViewModel(db, item));
        }

        //
        // GET: /Admin/Exhibit/EditArticle
        public ActionResult EditArticle(int exhibitId, int? id)
        {
            Article item = null;
            Exhibit exhibit = null;

            try
            {
                exhibit = db.Exhibits.Find(exhibitId);
                if (exhibit == null)
                    throw new ArgumentException("Экспонат не найден.");

                if (id.HasValue)
                {
                    item = db.Articles.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Материал не найден.");
                }
                else
                {
                    item = new Article() { Exhibit = exhibit };
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new ExhibitViewModel(db, exhibit, item));
        }

        //
        // POST: /Admin/Exhibit/EditArticle
        [HttpPost, ActionName("EditArticle")]
        public ActionResult EditArticlePost(int exhibitId, int? id)
        {
            Article item = null;
            Exhibit exhibit = null;

            try
            {
                exhibit = db.Exhibits.Find(exhibitId);
                if (exhibit == null)
                    throw new ArgumentException("Экспонат не найден.");

                if (id.HasValue)
                {
                    item = db.Articles.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Материал не найден.");
                }
                else
                {
                    item = new Article() { Exhibit = exhibit };
                }

                TryUpdateModel(item, "Article", new[] { "Title", "Text" });

                if (ModelState.IsValid)
                {
                    if (id == null)
                        db.Articles.Add(item);
                    else
                        db.Entry<Article>(item).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Articles", new { id = exhibit.Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new ExhibitViewModel(db, exhibit, item));
        }

        //
        // POST: /Admin/Exhibit/ChangeArticleImage
        [HttpPost]
        public ActionResult ChangeArticleImage(int id, HttpPostedFileBase Photo)
        {
            var item = db.Articles.Find(id);
#warning Вторая часть проверки бредовая, но без этого не работает. Почему-то.
            if (item == null || item.Exhibit.Id == 0) 
                return RedirectToAction("Index");

            if (Photo.IsImage())
            {
                if (Photo != null && Photo.IsImage())
                {
                    item.ApplicationType = Photo.ContentType;
                    item.ApplicationData = new WebImage(Photo.InputStream).Resize(300, 300).GetBytes(item.ApplicationType);
                    Photo.InputStream.Read(item.ApplicationData, 0, Photo.ContentLength);
                }
                db.Entry<Article>(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Articles", new { id = item.Exhibit.Id });
            }
            return RedirectToAction("Articles", new { id = item.Exhibit.Id });
        }

        //
        // GET: /Admin/Exhibit/DeleteArticle/5
        public ActionResult DeleteArticle(int id)
        {
            var item = db.Articles.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new ExhibitViewModel(db, item.Exhibit, item));
        }

        //
        // POST: /Admin/Exhibit/DeleteArticle/5
        [HttpPost, ActionName("DeleteArticle")]
        public ActionResult DeleteArticlePost(int id)
        {
            var item = db.Articles.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            var exhibitId = item.Exhibit.Id;
            db.Articles.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Articles", new { id = exhibitId });
        }

        //
        // GET: /Admin/Exhibit/RemoveArticleImage
        public ActionResult RemoveArticleImage(int id)
        {
            var item = db.Articles.Find(id);
            if (item == null || item.Exhibit.Id == 0)
                return RedirectToAction("Index");
            if (item.ApplicationType == null)
                return RedirectToAction("Index");

            item.ApplicationData = null;
            item.ApplicationType = null;
            db.Entry(item).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Articles", new { id = item.Exhibit.Id });
        }
    }
}
