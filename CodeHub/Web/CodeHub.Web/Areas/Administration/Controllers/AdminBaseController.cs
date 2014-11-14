namespace CodeHub.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using CodeHub.Common;
    using CodeHub.Data.Contracts;
    using CodeHub.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdminRole)]
    public abstract class AdminBaseController : BaseController
    {
        public AdminBaseController(ICodeHubData data)
            : base(data)
        {
        }
    }
}