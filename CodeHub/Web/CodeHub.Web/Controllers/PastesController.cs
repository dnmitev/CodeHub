namespace CodeHub.Web.Controllers
{
    using CodeHub.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CodeHub.Web.ViewModels.HomePage;
    using CodeHub.Web.ViewModels.Paste;
    using CodeHub.Data.Models;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using CodeHub.Web.Infrastructure.Populators;

    public class PastesController : BaseController
    {
        private const int DefaultPastesCountBySyntax = 3;

        private readonly IDropDownListPopulator populator;

        public PastesController(ICodeHubData data, IDropDownListPopulator populator)
            : base(data)
        {
            this.populator = populator;
        }

        public ActionResult All(int? syntax, bool? onlyMine)
        {
            return View(syntax);
        }

        [ValidateInput(false)]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home", null);
            }

            var idAsGuid = new Guid(id);

            var paste = this.Data.Pastes
                            .All()
                            .Where(p => p.Id == idAsGuid)
                            .Project()
                            .To<PasteDetailsViewModel>()
                            .FirstOrDefault();

            var dbPaste = this.Data.Pastes.GetById(idAsGuid);

            // each time a paste is viewed, its hits should grow up by 1
            dbPaste.Hits++;
            this.Data.SaveChanges();

            return View(paste);
        }

        [HttpPost]
        public ActionResult ReadPastes([DataSourceRequest]DataSourceRequest request, int? syntax)
        {
            var pastesQuery = this.Data.Pastes.All();

            if (syntax != null)
            {
                pastesQuery = pastesQuery.Where(p => p.SyntaxId == syntax);
            }

            var pastes = pastesQuery
                                    .Project()
                                    .To<BasePasteViewModel>();

            return Json(pastes.ToDataSourceResult(request));
        }

        [ChildActionOnly]
        [OutputCache(Duration = 5 * 60)]
        public ActionResult GetLatestPastes(int syntaxId)
        {
            var currentPastesInSyntax = this.Data.Pastes
                                            .All()
                                            .Where(p => p.SyntaxId == syntaxId && !p.IsPrivate)
                                            .OrderByDescending(p => p.CreatedOn)
                                            .Take(DefaultPastesCountBySyntax)
                                            .Project()
                                            .To<PasteHomePageViewModel>()
                                            .ToList();

            return PartialView("_GetLatestPastesPartial", currentPastesInSyntax);
        }
    }
}