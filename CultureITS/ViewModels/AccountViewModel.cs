using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.ViewModels
{
    public class AccountViewModel
    {
        public User Item { set; get; }
        public Student RegisterItem { set; get; }

        public AccountViewModel(User item)
        {
            Item = item;
        }

        public AccountViewModel(Student item)
        {
            RegisterItem = item;
        }
    }
}