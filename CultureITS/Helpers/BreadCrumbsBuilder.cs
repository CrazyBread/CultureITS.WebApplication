using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CultureITS.Helpers
{
    public class BreadCrumbsBuilder
    {
        private IList<BreadCrumb> _breadCrubsList = new List<BreadCrumb>();

        public BreadCrumbsBuilder()
        {
            _breadCrubsList.Add(new BreadCrumb() { Text = "Главная страница", Uri = new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Index", "Home", new { area = "" }) });
        }

        public BreadCrumbsBuilder Add(BreadCrumb crumb)
        {
            _breadCrubsList.Add(crumb);
            return this;
        }

        public string Generate(string page)
        {
            string result = "";
            foreach (var item in _breadCrubsList)
            {
                result += string.Format("<a href=\"{0}\">{1}</a><span> / </span>", item.Uri, item.Text);
            }
            result += string.Format("<span>{0}</span>", page);
            return result;
        }
    }

    public struct BreadCrumb
    {
        public string Text { set; get; }
        public string Uri { set; get; }
    }
}