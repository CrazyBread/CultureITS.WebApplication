using CultureITS.Models;
using CultureITS.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Areas.Admin.Models
{
    public class RightsViewModel
    {
        public IEnumerable<AccessRight> List { set; get; }

        public RightsViewModel(DataContext db)
        {
            List = db.AccessRights.OrderBy(i => i.Area).ThenBy(i => i.Controller).ThenBy(i => i.Action).ThenBy(i => i.Role);
        }
    }
}