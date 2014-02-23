using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Controllers
{
    [AuthorizeFilterAttribute]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/Styles
        public ActionResult Styles()
        {
            return View();
        }

        //
        // GET: /Home/About
        public ActionResult About()
        {
            return View();
        }
    }
}
