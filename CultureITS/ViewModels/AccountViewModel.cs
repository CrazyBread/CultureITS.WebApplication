using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.ViewModels
{
    public class AccountViewModel
    {
        public User Item { set; get; }
        public Student ItemStudent { set; get; }
        public string TestData { set; get; }

        public Student RegisterItem { set; get; }

        [Display(Name = "Изображение профиля")]
        public HttpPostedFileBase Photo { set; get; }

        public AccountViewModel(User item)
        {
            Item = item;
            if (item is Student)
            {
                ItemStudent = item as Student;

                TestData = "Date";
                var tests = ItemStudent.TestSessions.GroupBy(i => i.Test.Id).Select(j => j.FirstOrDefault().Test).ToArray();
                foreach (var test in tests)
                    TestData += "," + test.Title;
                TestData += "\\n";

                var cntr = 1;
                foreach (var sessions in ItemStudent.TestSessions)
                {
                    TestData += cntr++;
                    for (int i = 0; i < tests.Count(); i++)
                    {
                        var strBuild = ",";

                        if (sessions.Test.Id == tests[i].Id)
                            strBuild += Convert.ToInt16(sessions.Percent * 100).ToString();
                        
                        TestData += strBuild;
                    }
                    TestData += "\\n";
                }
            }
        }

        public AccountViewModel(Student item)
        {
            RegisterItem = item;
        }
    }
}