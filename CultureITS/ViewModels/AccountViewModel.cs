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
        //private DataContext db = new DataContext();

        public User Item { set; get; }

        public AccountViewModel(User item)
        {
            Item = item;
        }
    }
}