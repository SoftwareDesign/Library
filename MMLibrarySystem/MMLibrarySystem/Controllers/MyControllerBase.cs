using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MMLibrarySystem.Controllers
{
    /// <summary>
    /// Provides some general operations for the controllers.
    /// </summary>
    public class MyControllerBase : Controller
    {
        protected ActionResult Alert(string format, params object[] args)
        {
            var sb = new StringBuilder();
            sb.Append("alert('");
            sb.AppendFormat(format, args);
            sb.Append("');");
            return JavaScript(sb.ToString());
        }
    }
}