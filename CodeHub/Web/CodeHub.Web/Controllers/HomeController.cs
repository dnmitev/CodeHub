using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeHub.Data.Models;
using CodeHub.Data;
using CodeHub.Data.Contracts;

namespace CodeHub.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ICodeHubData data)
            : base(data)
        {

        }

        public ActionResult Index()
        {
            var paste = this.Data.Pastes.All().FirstOrDefault();
            return View(paste);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}