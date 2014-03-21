using CultureITS.Areas.Admin.Models;
using CultureITS.Models.Context;
using CultureITS.Models.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Areas.Admin.Controllers
{
    public class TestController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Admin/Test/
        public ActionResult Index()
        {
            return View(new TestViewModel(db));
        }

        //
        // GET: /Admin/Test/Edit
        public ActionResult Edit(int? id)
        {
            TestMain item = null;

            try
            {
                if (id.HasValue)
                {
                    item = db.TestMain.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Тест не найден.");
                }
                else
                {
                    item = new TestMain();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new TestViewModel(db, item));
        }

        //
        // POST: /Admin/Test/Edit
        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id)
        {
            TestMain item = null;

            try
            {
                if (id.HasValue)
                {
                    item = db.TestMain.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Тест не найден.");
                }
                else
                {
                    item = new TestMain();
                }

                TryUpdateModel(item, "Item", new[] { "Title", "Topic", "Author" });

                if (ModelState.IsValid)
                {
                    if (id == null)
                        db.TestMain.Add(item);
                    else
                        db.Entry<TestMain>(item).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new TestViewModel(db, item));
        }

        //
        // GET: /Admin/Test/Delete/5
        public ActionResult Delete(int id)
        {
            var item = db.TestMain.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new TestViewModel(db, item));
        }

        //
        // POST: /Admin/Test/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var item = db.TestMain.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            db.TestMain.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /Admin/Test/Questions/5
        public ActionResult Questions(int id)
        {
            var item = db.TestMain.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new TestViewModel(db, item));
        }

        //
        // GET: /Admin/Test/EditQuestion
        public ActionResult EditQuestion(int testId, int? id)
        {
            Question item = null;
            TestMain test = null;

            try
            {
                test = db.TestMain.Find(testId);
                if (test == null)
                    throw new ArgumentException("Тест не найден.");

                if (id.HasValue)
                {
                    item = db.TestQuestion.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Вопрос не найден.");
                }
                else
                {
                    item = new Question() { Test = test };
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new TestViewModel(db, test, item));
        }

        //
        // POST: /Admin/Test/EditQuestion
        [HttpPost, ActionName("EditQuestion")]
        public ActionResult EditQuestionPost(int testId, int? id)
        {
            Question item = null;
            TestMain test = null;

            try
            {
                test = db.TestMain.Find(testId);
                if (test == null)
                    throw new ArgumentException("Тест не найден.");

                if (id.HasValue)
                {
                    item = db.TestQuestion.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Вопрос не найден.");
                }
                else
                {
                    item = new Question() { Test = test };
                }

                TryUpdateModel(item, "Question", new[] { "Text" });

                if (ModelState.IsValid)
                {
                    if (id == null)
                        db.TestQuestion.Add(item);
                    else
                        db.Entry<Question>(item).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Questions", new { id = test.Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new TestViewModel(db, test, item));
        }

        //
        // GET: /Admin/Test/DeleteQuestion/5
        public ActionResult DeleteQuestion(int id)
        {
            var item = db.TestQuestion.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new TestViewModel(db, item.Test, item));
        }

        //
        // POST: /Admin/Test/DeleteQuestion/5
        [HttpPost, ActionName("DeleteQuestion")]
        public ActionResult DeleteQuestionPost(int id)
        {
            var item = db.TestQuestion.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            var testId = item.Test.Id;
            db.TestQuestion.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Questions", new { id = testId });
        }

        //
        // GET: /Admin/Test/Answers/5
        public ActionResult Answers(int id)
        {
            var item = db.TestQuestion.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new TestViewModel(db, item.Test, item));
        }

        //
        // GET: /Admin/Test/EditAnswer
        public ActionResult EditAnswer(int questionId, int? id)
        {
            Answer item = null;
            Question question = null;

            try
            {
                question = db.TestQuestion.Find(questionId);
                if (question == null)
                    throw new ArgumentException("Вопрос не найден.");

                if (id.HasValue)
                {
                    item = db.TestAnswer.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Ответ не найден.");
                }
                else
                {
                    item = new Answer() { Question = question };
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new TestViewModel(db, question.Test, question, item));
        }

        //
        // POST: /Admin/Test/EditAnswer
        [HttpPost, ActionName("EditAnswer")]
        public ActionResult EditAnswerPost(int questionId, int? id)
        {
            Answer item = null;
            Question question = null;

            try
            {
                question = db.TestQuestion.Find(questionId);
                if (question == null)
                    throw new ArgumentException("Вопрос не найден.");

                if (id.HasValue)
                {
                    item = db.TestAnswer.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                        throw new ArgumentException("Ответ не найден.");
                }
                else
                {
                    item = new Answer() { Question = question };
                }

                TryUpdateModel(item, "Answer", new[] { "Text", "Right" });

                if (ModelState.IsValid)
                {
                    if (id == null)
                        db.TestAnswer.Add(item);
                    else
                        db.Entry<Answer>(item).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Answers", new { id = question.Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(new TestViewModel(db, question.Test, question, item));
        }

        //
        // GET: /Admin/Test/DeleteAnswer/5
        public ActionResult DeleteAnswer(int id)
        {
            var item = db.TestAnswer.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            return View(new TestViewModel(db, item.Question.Test, item.Question, item));
        }

        //
        // POST: /Admin/Test/DeleteAnswer/5
        [HttpPost, ActionName("DeleteAnswer")]
        public ActionResult DeleteAnswerPost(int id)
        {
            var item = db.TestAnswer.Find(id);

            if (item == null)
                return RedirectToAction("Index");

            var questionId = item.Question.Id;
            db.TestAnswer.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Answers", new { id = questionId });
        }

    }
}
