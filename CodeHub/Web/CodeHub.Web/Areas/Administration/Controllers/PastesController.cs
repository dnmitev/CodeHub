namespace CodeHub.Web.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.UI;

    using CodeHub.Data.Contracts;

    using Model = CodeHub.Data.Models.Paste;
    using ViewModel = CodeHub.Web.Areas.Administration.ViewModels.PasteViewModel;

    public class PastesController : KendoGridAdministrationController
    {
        public PastesController(ICodeHubData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var syntaxex =
                ViewData["syntaxes"] = this.Data.Syntaxes
                .All()
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return View();
        }

        protected override IEnumerable GetData()
        {
            var data = this.Data.Pastes
                           .All()
                           .Project()
                           .To<ViewModel>();

            return data;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Pastes.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]
                                   DataSourceRequest request, ViewModel model)
        {
            var dbModel = base.Create<Model>(model);
            if (dbModel != null)
            {
                model.Id = dbModel.Id;
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]
                                   DataSourceRequest request, ViewModel model)
        {
            base.Update<Model, ViewModel>(model, model.Id);
            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]
                                    DataSourceRequest request, ViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var paste = this.Data.Pastes.GetById(model.Id);

                this.Data.Pastes.Delete(paste);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }
    }
}