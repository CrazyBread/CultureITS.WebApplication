using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Helpers
{
    public static class RightsManager
    {
        public static DataContext db = new DataContext();

        public static bool IsAllow(string area, string controller, string action, AccountStatus status)
        {
            var result = db.AccessRights.SingleOrDefault(i => ((controller == i.Controller) && (action == i.Action) && (status == i.Role) && 
                ((area == i.Area) || ((area == null) && (i.Area == null)))));

            if (result != null)
                return result.IsAllowed;

            db.AccessRights.Add(new AccessRight() { Area = area, Controller = controller, Action = action, Role = status, IsAllowed = true });
            db.SaveChanges();

            return true;
            /*
            if (controller == "Home")
                return true;

            if (status == AccountStatus.None)
                return ((controller == "Account") && (action == "Login"));

            return !((controller == "Account") && (action == "Login"));*/
        }
    }
}