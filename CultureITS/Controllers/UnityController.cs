using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Controllers
{
    [AuthorizeFilterAttribute]
    public class UnityController : Controller
    {
        //
        // GET: /Unity/
        public ActionResult Index()
        {
            return View();
        }
    }
}
