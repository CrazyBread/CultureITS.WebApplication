using CultureITS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.ViewModels
{
    public class GameObjectViewModel
    {
        public GameObject Item { set; get; }
        public IEnumerable<GameObject> List { set; get; }

        public int? Page { set; get; }

        public GameObjectViewModel(GameObject item)
        {
            Item = item;
        }

        public GameObjectViewModel(IEnumerable<GameObject> list, int? page)
        {
            List = list;
            Page = page;
        }
    }
}