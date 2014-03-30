using CultureITS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.ViewModels
{
    public class ExhibitViewModel
    {
        public Exhibit Item { set; get; }
        public IEnumerable<Exhibit> List { set; get; }

        public Article Article { set; get; }

        public int? Page { set; get; }

        public ExhibitViewModel(Exhibit item)
        {
            Item = item;
        }

        public ExhibitViewModel(Exhibit item, Article article) : this(item)
        {
            Item = item;
            Article = article;
        }

        public ExhibitViewModel(IEnumerable<Exhibit> list, int? page)
        {
            List = list;
            Page = page;
        }
    }
}