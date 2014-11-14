namespace CodeHub.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;
    
    using CodeHub.Data.Contracts;
    using CodeHub.Web.ViewModels.HomePage;

    public class HomeController : BaseController
    {
        public HomeController(ICodeHubData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var syntaxes = this.Data.Syntaxes.All()
                               .OrderBy(s => s.Name)
                               .Project()
                               .To<SyntaxHomePageViewModel>()
                               .ToList();

            return View(syntaxes);
        }
    }
}