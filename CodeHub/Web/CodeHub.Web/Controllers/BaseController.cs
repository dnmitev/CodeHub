namespace CodeHub.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

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

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.CurrentUser = this.Data.Users
                                   .All()
                                   .FirstOrDefault(u => u.UserName == requestContext.HttpContext.User.Identity.Name);

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}