using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Api
{
    public class ExhibitInfoIn
    {
        public string code { set; get; }
    }

    public class ExhibitInfoOut
    {
        public bool success { set; get; }

        public int id { set; get; }
        public string name { set; get; }
        public string location { set; get; }
        public string description { set; get; }
        public bool haveApplication { set; get; }
        public bool state { set; get; }
    }

    public class ExhibitMarkIn
    {
        public int id { set; get; }
    }

    public class ExhibitMarkOut
    {
        public bool success { set; get; }

        public bool state { set; get; }
    }

    public class ExhibitCheckIn
    {
        public int id { set; get; }
    }

    public class ExhibitCheckOut
    {
        public bool success { set; get; }

        public bool state { set; get; }
    }
}