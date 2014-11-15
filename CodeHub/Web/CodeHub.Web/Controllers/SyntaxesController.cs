namespace CodeHub.Web.Controllers
{
    using CodeHub.Data.Contracts;
    using CodeHub.Web.Infrastructure.Populators;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class SyntaxesController : BaseController
    {
        private readonly IDropDownListPopulator populator;

        public SyntaxesController(ICodeHubData data, IDropDownListPopulator populator)
            : base(data)
        {
            this.populator = populator;
        }

        public ActionResult GetSyntaxes()
        {
            return Json(this.populator.GetSyntaxes(), JsonRequestBehavior.AllowGet);
        }
    }
}