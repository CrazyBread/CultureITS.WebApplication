using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace CultureITS.Helpers
{
    public static class MenuHelper
    {
        // Добавляет метод, показывающий меню
        public static IEnumerable<MenuItem> GetMenu(this HttpSessionState session)
        {
            DataContext db = new DataContext();
            int accessiblemask = (1 << (int)session.GetUserRole());
            return db.MenuItems.Where(i => (i.AccessMask & accessiblemask) > 0).OrderBy(i => i.Order);
        }
    }
}