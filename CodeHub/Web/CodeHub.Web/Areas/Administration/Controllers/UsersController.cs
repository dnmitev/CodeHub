namespace CodeHub.Web.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using CodeHub.Data.Contracts;
    using CodeHub.Data.Models;
    using CodeHub.Web.Areas.Administration.ViewModels;

    using Kendo.Mvc.UI;

    using Model = CodeHub.Data.Models.Paste;
    using ViewModel = CodeHub.Web.Areas.Administration.ViewModels.UserViewModel;

    public class UsersController : KendoGridAdministrationController
    {
        public UsersController(ICodeHubData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            Model modelToDb = base.Create<Model>(model);
            if (modelToDb != null)
            {
                model.Id = modelToDb.Id.ToString();
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
            if (model != null && this.ModelState.IsValid)
            {
                User user = this.Data.Users.GetById(model.Id);

                IQueryable<Paste> userPastes = this.Data.Pastes
                                                   .All()
                                                   .Where(p => p.AuthorId == user.Id);

                int counter = 0;
                foreach (Model paste in
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
                foreach (Comment comment in
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

        protected override IEnumerable GetData()
        {
            IQueryable<UserViewModel> data = this.Data.Users
                                                 .All()
                                                 .Project()
                                                 .To<ViewModel>();

            return data;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }
    }
}