using CultureITS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Helpers
{
    public static class RightsManager
    {
        public static bool IsAllow(string controller, string action, AccountStatus status)
        {
            if (controller == "Home")
                return true;

            if (status == AccountStatus.None)
                return ((controller == "Account") && (action == "Login"));

            return !((controller == "Account") && (action == "Login"));
        }
    }
}