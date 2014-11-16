namespace CodeHub.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using CodeHub.Data.Contracts;
    using CodeHub.Web.Infrastructure.Populators;
    using CodeHub.Web.ViewModels.HomePage;
    using CodeHub.Web.ViewModels.Other;
    using CodeHub.Web.ViewModels.Paste;
    using CodeHub.Data.Models;

    public class PastesController : BaseController
    {
        private const int DefaultPastesCountBySyntax = 3;
        private const int DefaultAdditionalPointsPerPaste = 10;

        private readonly IDropDownListPopulator populator;

        public PastesController(ICodeHubData data, IDropDownListPopulator populator)
            : base(data)
        {
            this.populator = populator;
        }

        [HttpGet]
        public ActionResult All(FilterViewModel filter)
        {
            // Filter options shouldn't be accessed through the query string of the URL
            if (!Request.IsAuthenticated)
            {
                filter = new FilterViewModel()
                {
                    Syntax = null,
                    OnlyMine = false,
                    WithBugs = false
                };
            }

            return View(filter);
        }

        [HttpGet]
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

            // each time a paste is viewed, its hits should grow up by 1
            var dbPaste = this.Data.Pastes.GetById(idAsGuid);
            dbPaste.Hits++;

            this.Data.Pastes.Update(dbPaste);
            this.Data.SaveChanges();

            return View(paste);
        }

        [HttpPost]
        public ActionResult ReadPastes([DataSourceRequest]DataSourceRequest request, int? syntax, bool onlyMine, bool withBugs)
        {
            var pastesQuery = this.Data.Pastes.All();

            if (syntax != null)
            {
                pastesQuery = pastesQuery
                       .Where(p => p.SyntaxId == syntax);
            }

            if (onlyMine)
            {
                pastesQuery = pastesQuery
                         .Where(p => p.AuthorId == this.CurrentUser.Id);
            }

            if (withBugs)
            {
                pastesQuery = pastesQuery
                        .Where(p => p.HasBug);
            }

            var pastes = pastesQuery
                    .Project()
                    .To<BasePasteViewModel>();

            return Json(pastes.ToDataSourceResult(request));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            var addPasteViewModel = new AddPasteViewModel()
            {
                Syntaxes = this.populator.GetSyntaxes()
            };

            return View(addPasteViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddPasteViewModel paste)
        {
            if (paste != null && ModelState.IsValid)
            {
                var dbPaste = Mapper.DynamicMap<Paste>(paste);

                dbPaste.AuthorId = this.CurrentUser.Id;

                this.Data.Pastes.Add(dbPaste);
                this.Data.SaveChanges();

                // Each User receive points for a posted source code
                this.CurrentUser.Points += DefaultAdditionalPointsPerPaste;
                this.Data.Users.Update(this.CurrentUser);
                this.Data.SaveChanges();

                return RedirectToAction("All");
            }

            paste.Syntaxes = this.populator.GetSyntaxes();

            return View(paste);
        }

        [ChildActionOnly]
        public ActionResult UserOptions(string pasteId)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
            var options = new PasteUserOptionsViewModel()
            {
                Id = pasteId,
                HasBug = currentPaste.HasBug,
                IsPrivate = currentPaste.IsPrivate,
                HasCurrentUserAsAuthor = currentPaste.AuthorId == this.CurrentUser.Id
            };

            return PartialView("_UserOptions", options);
        }

        [Authorize]
        [HttpGet]
        public ActionResult FlagBug(string pasteId)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
            currentPaste.HasBug = true;

            this.Data.Pastes.Update(currentPaste);
            this.Data.SaveChanges();

            var pasteAsViewModel = Mapper.Map<PasteDetailsViewModel>(currentPaste);

            return View("Details", pasteAsViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult MakePrivate(string pasteId)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
            currentPaste.IsPrivate = true;

            this.Data.Pastes.Update(currentPaste);
            this.Data.SaveChanges();

            var pasteAsViewModel = Mapper.Map<PasteDetailsViewModel>(currentPaste);

            return View("Details", pasteAsViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(string pasteId)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
            var editModel = Mapper.Map<EditPasteViewModel>(currentPaste);

            editModel.Id = pasteId;
            editModel.Syntaxes = this.populator.GetSyntaxes();

            return View(editModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPasteViewModel paste, string id)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(id));

            Mapper.CreateMap<EditPasteViewModel, Paste>();
            Mapper.Map<EditPasteViewModel, Paste>(paste, currentPaste);

            this.Data.Pastes.Update(currentPaste);
            this.Data.SaveChanges();

            return RedirectToAction("Details", new { id = id });
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