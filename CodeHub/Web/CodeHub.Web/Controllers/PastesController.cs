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

    public class PastesController : BaseController
    {
        private const int DefaultPastesCountBySyntax = 3;

        public PastesController(ICodeHubData data)
            : base(data)
        {
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