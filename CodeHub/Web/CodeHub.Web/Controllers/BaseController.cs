namespace CodeHub.Web.Controllers
{
    using System.Web.Mvc;

    using CodeHub.Data.Contracts;
    using CodeHub.Data.Models;

    public abstract class BaseController : Controller
    {
        public BaseController(ICodeHubData data)
        {
            this.Data = data;
        }

        protected ICodeHubData Data { get; set; }

        protected User CurrentUser { get; set; }
    }
}