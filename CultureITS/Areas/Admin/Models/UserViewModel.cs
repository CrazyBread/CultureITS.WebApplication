using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Areas.Admin.Models
{
    public class UserViewModel
    {
        public bool CanManage { set; get; }

        public User Item { set; get; }
        public IEnumerable<User> List { set; get; }

        public UserViewModel(DataContext db)
        {
            List = db.Users;
            CanManage = (System.Web.HttpContext.Current.Session.GetUserRole() == AccountStatus.Admin);
        }

        public UserViewModel(DataContext db, User item) : this(db)
        {
            Item = item;
        }
    }
}