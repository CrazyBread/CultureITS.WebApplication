﻿using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CultureITS.Areas.Admin.Models
{
    public class ExhibitViewModel
    {
        public bool CanManage { set; get; }

        public Exhibit Item { set; get; }
        public IEnumerable<Exhibit> List { set; get; }
        public Article Article { set; get; }

        [Display(Name = "Иллюстрация")]
        public HttpPostedFileBase Photo { set; get; }

        public ExhibitViewModel(DataContext db)
        {
            List = db.Exhibits;
            CanManage = (System.Web.HttpContext.Current.Session.GetUserRole() == AccountStatus.Admin);
        }

        public ExhibitViewModel(DataContext db, Exhibit item)
            : this(db)
        {
            Item = item;
        }

        public ExhibitViewModel(DataContext db, Exhibit item, Article article)
            : this(db, item)
        {
            Article = article;
        }
    }
}