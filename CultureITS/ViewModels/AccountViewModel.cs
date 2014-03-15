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
        public Student Item { set; get; }
        public Student RegisterItem { set; get; }

        public AccountViewModel(Student item)
        {
            Item = item;
            RegisterItem = item;
        }
    }
}