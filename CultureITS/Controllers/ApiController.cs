using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Api;
using CultureITS.Models.Context;
using CultureITS.Models.Test;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CultureITS.Controllers
{
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

        #region Подсистема работы с экспонатами музея
        //
        // POST: /Api/markExhibit/5
        [HttpPost]
        public JsonResult markExhibit(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<ExhibitMarkIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var exhibit = db.Exhibits.Find(dataIn.id);
                if (exhibit == null)
                    throw new Exception("Экспонат с таким идентификатором не найден.");

                ExhibitMarkOut dataOut = new ExhibitMarkOut();
                dataOut.success = true;

                if (user.Exhibits.Count(i => i.Id == exhibit.Id) > 0)
                {
                    user.Exhibits.Remove(exhibit);
                    dataOut.state = false;
                }
                else
                {
                    user.Exhibits.Add(exhibit);
                    dataOut.state = true;
                }
                db.SaveChanges();

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        //
        // POST: /Api/checkExhibit/5
        [HttpPost]
        public JsonResult checkExhibit(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<ExhibitCheckIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var exhibit = db.Exhibits.Find(dataIn.id);
                if (exhibit == null)
                    throw new Exception("Экспонат с таким идентификатором не найден.");

                ExhibitCheckOut dataOut = new ExhibitCheckOut();
                dataOut.success = true;

                dataOut.state = (user.Exhibits.Count(i => i.Id == exhibit.Id) > 0);

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        //
        // POST: /Api/getExhibitInfo/5
        [HttpPost]
        public JsonResult getExhibitInfo(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<ExhibitInfoIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var exhibit = db.Exhibits.SingleOrDefault(i => i.Code == dataIn.code);
                if (exhibit == null)
                    throw new Exception("Экспонат с таким идентификатором не найден.");

                ExhibitInfoOut dataOut = new ExhibitInfoOut();
                dataOut.success = true;

                dataOut.id = exhibit.Id;
                dataOut.name = exhibit.Name;
                dataOut.description = exhibit.Description;
                dataOut.location = exhibit.Location;
                dataOut.haveApplication = (!string.IsNullOrEmpty(exhibit.ApplicationType));
                dataOut.haveArticles = (exhibit.Article.Count() > 0);
                dataOut.state = (user.Exhibits.Count(i => i.Id == exhibit.Id) > 0);

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        #endregion

        #region Подсистема работы с тестированием
        //
        // POST: /Api/getTestInfo
        [HttpPost]
        public JsonResult getTestInfo(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<TestInfoIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var test = db.TestMain.SingleOrDefault(i => i.Code == dataIn.code);
                if (test == null)
                    throw new Exception("Тест с таким идентификатором не найден.");

                TestInfoOut dataOut = new TestInfoOut();
                dataOut.success = true;

                dataOut.author = test.Author;
                dataOut.questionsCount = test.Questions.Count(i => i.Answers.Count(j => j.Right) > 0);
                dataOut.title = test.Title;
                dataOut.topic = test.Topic;
                dataOut.id = test.Id;

                dataOut.resultsCount = test.Sessions.Count(i => i.Student.Id == user.Id);
                dataOut.isOpened = (dataOut.resultsCount == 0) ? false : (test.Sessions.Count(i => ((i.Student.Id == user.Id) && (!i.Complete))) > 0);
                var dateLastResult = (dataOut.resultsCount == 0) ? DateTime.Now : test.Sessions.Where(i => i.Student.Id == user.Id).OrderByDescending(i => i.Date).FirstOrDefault().Date;
                dataOut.dateLastResult = dateLastResult.ToString("dd.MM.yyyy");

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        //
        // POST: /Api/startTest
        [HttpPost]
        public JsonResult startTest(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<TestStartIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var test = db.TestMain.Find(dataIn.id);
                if (test == null)
                    throw new Exception("Тест с таким идентификатором не найден.");

                TestStartOut dataOut = new TestStartOut();
                dataOut.success = true;

                var openedTestSession = test.Sessions.SingleOrDefault(i => ((i.Student.Id == user.Id) && (!i.Complete)));
                if (openedTestSession == null)
                {
                    var testSession = new Session() { Complete = false, Date = DateTime.Now, QuestionsLeft = test.Questions.Count(i => i.Answers.Count(j => j.Right) > 0), Student = user, Test = test, Percent = 0 };

                    TestData testData = new TestData();
                    testData.Queue = new TestDataItem[testSession.QuestionsLeft];
#warning Добавить перемешивание вопросов
                    int k = 0;
                    foreach (var item in test.Questions.Where(i => i.Answers.Count(j => j.Right) > 0))
                    {
                        testData.Queue[k].id = item.Id;
                        testData.Queue[k].maxResult = item.Answers.Count(j => j.Right);
                        testData.Queue[k].result = 0;
                        testData.Queue[k].complete = false;
                        k++;
                    }
                    testSession.Data = JsonConvert.SerializeObject(testData);

                    db.TestSessions.Add(testSession);
                    db.SaveChanges();
                    dataOut.testSessionId = testSession.Id;
                    dataOut.questionLeft = testSession.QuestionsLeft;
                }
                else
                {
                    dataOut.testSessionId = openedTestSession.Id;
                    dataOut.questionLeft = openedTestSession.QuestionsLeft;
                }

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        //
        // POST: /Api/getTestQuestion
        [HttpPost]
        public JsonResult getTestQuestion(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<TestQuestionIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var testSession = db.TestSessions.SingleOrDefault(i => ((i.Student.Id == user.Id) && (i.Id == dataIn.testSessionId)));
                if (testSession == null)
                    throw new Exception("Сеанс тестирования не найден.");

                TestData testData = JsonConvert.DeserializeObject<TestData>(testSession.Data);
                if ((dataIn.questionNumber > testData.Queue.Count()) || (dataIn.questionNumber < 1))
                    throw new Exception("Неправильный номер вопроса.");

                TestDataItem testDataItem = testData.Queue[dataIn.questionNumber - 1];
                if (testDataItem.complete)
                    throw new Exception("На вопрос уже был дан ответ.");

                var testQuestion = db.TestQuestion.Find(testDataItem.id);
                if (testQuestion == null)
                    throw new Exception("Ошибка при выборе вопроса из БД.");

                TestQuestionOut dataOut = new TestQuestionOut();
                dataOut.success = true;

                dataOut.allowMultiChoise = (testQuestion.Answers.Count(i => i.Right) > 1);
                dataOut.hasImage = !String.IsNullOrEmpty(testQuestion.ApplicationType);
                dataOut.text = testQuestion.Text;
                dataOut.number = dataIn.questionNumber;

                dataOut.answers = new TestAnswers[testQuestion.Answers.Count];
                int j = 0;
                foreach (var item in testQuestion.Answers)
                {
                    dataOut.answers[j].id = item.Id;
                    dataOut.answers[j].text = item.Text;
                    dataOut.answers[j].hasImage = !String.IsNullOrEmpty(item.ApplicationType);
                    j++;
                }

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        //
        // POST: /Api/setTestAnswer
        [HttpPost]
        public JsonResult setTestAnswer(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<TestAnswerIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var testSession = db.TestSessions.SingleOrDefault(i => ((i.Student.Id == user.Id) && (i.Id == dataIn.testSessionId)));
                if (testSession == null)
                    throw new Exception("Сеанс тестирования не найден.");

                TestData testData = JsonConvert.DeserializeObject<TestData>(testSession.Data);
                if ((dataIn.questionNumber > testSession.Test.Questions.Count(i => i.Answers.Count(j => j.Right) > 0)) || (dataIn.questionNumber < 1))
                    throw new Exception("Неправильный номер вопроса.");

                TestDataItem testDataItem = testData.Queue[dataIn.questionNumber - 1];
                if (testDataItem.complete)
                    throw new Exception("На вопрос уже был дан ответ.");

                var testQuestion = db.TestQuestion.Find(testDataItem.id);
                if (testQuestion == null)
                    throw new Exception("Ошибка при выборе вопроса из БД.");

                var markedAnswers = new bool[dataIn.answersNumbers.Count()];
                foreach (var rightItem in testQuestion.Answers.Where(i => i.Right))
                {
                    for(int i = 0; i < dataIn.answersNumbers.Count(); i++)
                    {
                        if (markedAnswers[i]) continue;

                        var item = dataIn.answersNumbers[i];
                        if (item == rightItem.Id)
                        {
                            testData.Queue[dataIn.questionNumber - 1].result++;
                            markedAnswers[i] = true;
                            break;
                        }
                    }
                }
                testData.Queue[dataIn.questionNumber - 1].result -= markedAnswers.Count(i => !i);
                if (testData.Queue[dataIn.questionNumber - 1].result < 0) testData.Queue[dataIn.questionNumber - 1].result = 0;
                testData.Queue[dataIn.questionNumber - 1].complete = true;

                testSession.Data = JsonConvert.SerializeObject(testData);
                testSession.QuestionsLeft--;
                if (testSession.QuestionsLeft == 0)
                {
                    testSession.Complete = true;
                    testSession.Percent = Convert.ToDouble(testData.Queue.Sum(i => i.result)) / testData.Queue.Sum(i => i.maxResult);
                }
                db.Entry<Session>(testSession).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                TestAnswerOut dataOut = new TestAnswerOut();
                dataOut.success = true;

                if (testSession.QuestionsLeft > 0)
                {
                    dataOut.result = 1;
                    dataOut.questionNext = dataIn.questionNumber + 1;
                }
                else
                {
                    dataOut.result = 0;
                }

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        //
        // POST: /Api/getTestResult
        [HttpPost]
        public JsonResult getTestResult(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<TestResultIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var testSession = db.TestSessions.SingleOrDefault(i => ((i.Student.Id == user.Id) && (i.Id == dataIn.testSessionId)));
                if (testSession == null)
                    throw new Exception("Сеанс тестирования не найден.");

                TestData testData = JsonConvert.DeserializeObject<TestData>(testSession.Data);

                TestResultOut dataOut = new TestResultOut();
                dataOut.success = true;

                dataOut.date = testSession.Date;
                dataOut.percent = testSession.Percent;
                dataOut.questionsResults = new TestResults[testData.Queue.Count()];
                int j = 0;
                foreach (var item in testData.Queue)
                {
                    dataOut.questionsResults[j].result = item.result;
                    dataOut.questionsResults[j].maxResult = item.maxResult;
                    dataOut.questionsResults[j].number = j + 1;
                    j++;
                }

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        #endregion

        #region Подсистема работы с игровыми объектами
        //
        // POST: /Api/putItemInBag/5
        [HttpPost]
        public JsonResult putItemInBag(string data)
        {
            try
            {
                var dataIn = JsonConvert.DeserializeObject<ItemPutIn>(data);

                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                var gameItem = db.GameItems.SingleOrDefault(i => i.Name == dataIn.name);
                if (gameItem == null)
                {
                    gameItem = new GameItem() { Name = dataIn.name };
                    db.GameItems.Add(gameItem);
                }

                ItemPutOut dataOut = new ItemPutOut();
                dataOut.success = true;

                user.GameItems.Add(gameItem);
                db.SaveChanges();

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        //
        // POST: /Api/getItemsInBag/5
        [HttpPost]
        public JsonResult getItemsInBag(string data)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session.GetUserRole() != AccountStatus.Student)
                    throw new Exception("Доступ разрешен только студентам.");
                var user = db.Students.Find(System.Web.HttpContext.Current.Session.GetUser().Id);

                ItemGetOut dataOut = new ItemGetOut();
                dataOut.success = true;

                dataOut.items = string.Empty;
                foreach (var item in user.GameItems.Select(i => i.Name))
                    dataOut.items += item + "|";
                if (dataOut.items.Length > 0)
                    dataOut.items = dataOut.items.Substring(0, dataOut.items.Length - 1);

                return Json(dataOut);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        #endregion
    }
}
