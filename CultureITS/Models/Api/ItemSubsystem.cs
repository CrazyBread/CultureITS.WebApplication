using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Api
{
    public struct ItemPutIn
    {
        public string name { set; get; }
    }

    public struct ItemPutOut
    {
        public bool success { set; get; }
    }

    /*
    public struct ItemGetIn
    {
    }
    */

    public struct ItemGetOut
    {
        public bool success { set; get; }

        public string items { set; get; }
    }
}