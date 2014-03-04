using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Areas.Admin.Models
{
    public class GameObjectViewModel
    {
        public GameObject Item { set; get; }
        public IEnumerable<GameObject> List { set; get; }

        public GameObjectViewModel(DataContext db)
        {
            List = db.GameObjects;
        }

        public GameObjectViewModel(DataContext db, GameObject item) : this(db)
        {
            Item = item;
        }
    }
}