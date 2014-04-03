using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
using CultureITS.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Areas.Admin.Models
{
    public class TestViewModel
    {
        public bool CanManage { set; get; }
        public bool CanShowTests { set; get; }

        public TestMain Item { set; get; }
        public IEnumerable<TestMain> List { set; get; }
        public Question Question { set; get; }
        public Answer Answer { set; get; }

        public TestViewModel(DataContext db)
        {
            List = db.TestMain;
            CanManage = (System.Web.HttpContext.Current.Session.GetUserRole() == AccountStatus.Admin);
            CanShowTests = (System.Web.HttpContext.Current.Session.GetUserRole() == AccountStatus.Teacher);
        }

        public TestViewModel(DataContext db, TestMain item)
            : this(db)
        {
            Item = item;
        }

        public TestViewModel(DataContext db, TestMain item, Question question)
            : this(db, item)
        {
            Question = question;
        }

        public TestViewModel(DataContext db, TestMain item, Question question, Answer answer)
            : this(db, item, question)
        {
            Answer = answer;
        }
    }
}