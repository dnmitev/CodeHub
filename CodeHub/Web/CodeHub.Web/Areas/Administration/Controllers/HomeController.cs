namespace CodeHub.Web.Areas.Administration.Controllers
{
    using CodeHub.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    
    public class HomeController : AdminBaseController
    {
        public HomeController(ICodeHubData data) : base(data)
        {
        }

        // GET: Administration/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}