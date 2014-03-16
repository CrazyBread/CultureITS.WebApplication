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

        public Student RegisterItem { set; get; }

        [Display(Name = "Изображение профиля")]
        public HttpPostedFileBase Photo { set; get; }

        public AccountViewModel(User item)
        {
            Item = item;
            if (item is Student)
                ItemStudent = item as Student;
        }

        public AccountViewModel(Student item)
        {
            RegisterItem = item;
        }
    }
}