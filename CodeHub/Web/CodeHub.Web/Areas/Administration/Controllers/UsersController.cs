namespace CodeHub.Web.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CodeHub.Data.Contracts;

    using Kendo.Mvc.UI;

    using Model = CodeHub.Data.Models.Paste;
    using ViewModel = CodeHub.Web.Areas.Administration.ViewModels.UserViewModel;

    public class UsersController : KendoGridAdministrationController
    {
        public UsersController(ICodeHubData data) : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable GetData()
        {
            var data = this.Data.Users
                           .All()
                           .Project()
                           .To<ViewModel>();

            return data;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, ViewModel model)
        {
            var dbModel = base.Create<Model>(model);
            if (dbModel != null)
            {
                model.Id = dbModel.Id.ToString();
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            base.Update<Model, ViewModel>(model, model.Id);
            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var user = this.Data.Users.GetById(model.Id);

                var userPastes = this.Data.Pastes
                                     .All()
                                     .Where(p => p.AuthorId == user.Id);

                int counter = 0;
                foreach (var paste in
                    this.Data.Pastes
                        .All()
                        .Where(p => p.AuthorId == user.Id)
                        .ToList())
                {
                    counter++;
                    this.Data.Pastes.Delete(paste);
                    if (counter == 100)
                    {
                        this.Data.SaveChanges();
                    }
                }
                this.Data.SaveChanges();

                counter = 0;
                foreach (var comment in 
                    this.Data.Comments
                        .All()
                        .Where(c => c.AuthorId == user.Id)
                        .ToList())
                {
                    counter++;
                    this.Data.Comments.Delete(comment);
                    if (counter == 100)
                    {
                        this.Data.SaveChanges();
                    }
                }
                this.Data.SaveChanges();

                this.Data.Users.Delete(user);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }
    }
}