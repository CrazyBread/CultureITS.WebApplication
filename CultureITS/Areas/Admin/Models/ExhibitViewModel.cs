﻿using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Areas.Admin.Models
{
    public class ExhibitViewModel
    {
        public Exhibit Item { set; get; }
        public IEnumerable<Exhibit> List { set; get; }

        public ExhibitViewModel(DataContext db)
        {
            List = db.Exhibits;
        }

        public ExhibitViewModel(DataContext db, Exhibit item) : this(db)
        {
            Item = item;
        }
    }
}