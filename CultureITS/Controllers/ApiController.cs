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

        #region Подсистема работы с экспонатами музея
        //
        // POST: /Api/markExhibit/5
        [HttpPost]
        public JsonResult markExhibit(int id)
        {
            var item = db.Exhibits.Find(id);
            if (item == null)
                return Json(new { success = false, message = "Игровой объект не найден в системе." });

            var sessionUser = System.Web.HttpContext.Current.Session.GetUser();
            if (sessionUser == null)
                return Json(new { success = false, message = "Доступ неавторизованным пользователям запрещён." });

            var user = db.Users.Find(sessionUser.Id);
            if (user is Student == false)
                return Json(new { success = false, message = "Пользователь не является студентом." });

            Student student = user as Student;
            if (student.Exhibits.Count(i => i.Id == item.Id) > 0)
                student.Exhibits.Remove(item);
            else
                student.Exhibits.Add(item);
            db.SaveChanges();

            return Json(new { success = true });
        }

        //
        // POST: /Api/checkExhibit/5
        [HttpPost]
        public JsonResult checkExhibit(int id)
        {
            var item = db.Exhibits.Find(id);
            if (item == null)
                return Json(new { success = false, message = "Игровой объект не найден в системе." });

            var sessionUser = System.Web.HttpContext.Current.Session.GetUser();
            if (sessionUser == null)
                return Json(new { success = false, message = "Доступ неавторизованным пользователям запрещён." });

            var user = db.Users.Find(sessionUser.Id);
            if (user is Student == false)
                return Json(new { success = false, message = "Пользователь не является студентом." });

            Student student = user as Student;
            if (student.Exhibits.Count(i => i.Id == item.Id) > 0)
                return Json(new { success = true, state = true });
            return Json(new { success = true, state = false });
        }

        //
        // POST: /Api/getExhibitInfo/5
        [HttpPost]
        public JsonResult getExhibitInfo(string code)
        {
            var sessionUser = System.Web.HttpContext.Current.Session.GetUser();
            if (sessionUser == null)
                return Json(new { success = false, message = "Доступ неавторизованным пользователям запрещён." });

            var item = db.Exhibits.SingleOrDefault(i => i.Code == code);
            if (item == null)
                return Json(new { success = false, message = "Экспонат не найден в системе." });
            
            return Json(new { success = true, id = item.Id, name = item.Name, description = item.Description });
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

                foreach (var rightItem in testQuestion.Answers.Where(i => i.Right))
                {
                    foreach (var item in dataIn.answersNumbers)
                    {
                        if (item == rightItem.Id)
                        {
                            testData.Queue[dataIn.questionNumber - 1].result++;
                            break;
                        }
                    }
                }
                testData.Queue[dataIn.questionNumber - 1].complete = true;

                testSession.Data = JsonConvert.SerializeObject(testData);
                testSession.QuestionsLeft--;
                if (testSession.QuestionsLeft == 0)
                    testSession.Complete = true;
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
    }
}
