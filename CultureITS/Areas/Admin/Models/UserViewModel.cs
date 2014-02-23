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
        public User Item { set; get; }
        public IEnumerable<User> List { set; get; }
        public SelectList Roles { set; get; }

        public UserViewModel(DataContext db)
        {
            Roles = new SelectList(new SelectListItem[] {
                new SelectListItem() { Text = "Студент", Value = ((int)AccountStatus.Student).ToString() },
                new SelectListItem() { Text = "Преподаватель", Value = ((int)AccountStatus.Teacher).ToString() },
                new SelectListItem() { Text = "Администратор", Value = ((int)AccountStatus.Admin).ToString() }
            }, "Value", "Text");
            List = db.Users;
        }
    }
}