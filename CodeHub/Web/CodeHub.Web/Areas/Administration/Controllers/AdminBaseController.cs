namespace CodeHub.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [Authorize(Roles="Admin")]
    public abstract class AdminBaseController : Controller
    {
        // TODO: Give Data abstraction to the admin area controllers
    }
}