namespace CodeHub.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using CodeHub.Data.Contracts;
    using CodeHub.Web.Infrastructure.Populators;

    public class SyntaxesController : BaseController
    {
        private readonly IDropDownListPopulator populator;

        public SyntaxesController(ICodeHubData data, IDropDownListPopulator populator) : base(data)
        {
            this.populator = populator;
        }

        public ActionResult GetSyntaxes()
        {
            return this.Json(this.populator.GetSyntaxes(), JsonRequestBehavior.AllowGet);
        }
    }
}