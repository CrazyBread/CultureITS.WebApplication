using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Api
{
    /*
    public struct UserGetRoleIn
    {
    }
    */

    public struct UserGetRoleOut
    {
        public bool success { set; get; }

        public int role { set; get; }
    }
}