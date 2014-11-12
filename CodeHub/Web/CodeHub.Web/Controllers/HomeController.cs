using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeHub.Data.Common.Repositories;
using CodeHub.Data.Models;
using CodeHub.Data;

namespace CodeHub.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var data = new GenericRepository<Paste>(new CodeHubDbContext());

            var paste = data.All().FirstOrDefault();
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