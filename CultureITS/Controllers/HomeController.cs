using CultureITS.Helpers;
using CultureITS.Models;
using CultureITS.ViewModels;
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
            var model = new HomeViewModel();
            var user = System.Web.HttpContext.Current.Session.GetUser();

            if (user == null)
                model.ShowPromo = true;
            if (user is Student)
                model.ShowGameButton = true;

            return View(model);
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
