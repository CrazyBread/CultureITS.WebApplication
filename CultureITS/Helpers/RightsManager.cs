using CultureITS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Helpers
{
    public class RightsManager
    {
        public static bool IsAllow(string controller, string action, AccountStatus status)
        {
            if ((controller == "Account") && (action == "Login"))
                return true;
            return false;
        }
    }
}