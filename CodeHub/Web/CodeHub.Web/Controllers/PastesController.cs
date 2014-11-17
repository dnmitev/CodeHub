namespace CodeHub.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CodeHub.Data.Contracts;
    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Populators;
    using CodeHub.Web.Infrastructure.Sanitizing;
    using CodeHub.Web.ViewModels.Comment;
    using CodeHub.Web.ViewModels.HomePage;
    using CodeHub.Web.ViewModels.Other;
    using CodeHub.Web.ViewModels.Paste;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    public class PastesController : BaseController
    {
        private const int DefaultPastesCountBySyntax = 3;
        private const int DefaultAdditionalPointsPerPaste = 10;

        private readonly IDropDownListPopulator populator;
        private readonly ISanitizer sanitizer;

        public PastesController(ICodeHubData data, IDropDownListPopulator populator, ISanitizer sanitizer) : base(data)
        {
            this.populator = populator;
            this.sanitizer = sanitizer;
        }

        [HttpGet]
        public ActionResult All(FilterViewModel filter)
        {
            // Filter options shouldn't be accessed through the query string of the URL
            if (!this.Request.IsAuthenticated)
            {
                filter = new FilterViewModel()
                {
                    Syntax = null,
                    OnlyMine = false,
                    WithBugs = false
                };
            }

            return this.View(filter);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return this.RedirectToAction("Index", "Home", null);
            }

            var idAsGuid = new Guid(id);

            PasteDetailsViewModel paste = this.Data.Pastes
                                              .All()
                                              .Where(p => p.Id == idAsGuid)
                                              .Project()
                                              .To<PasteDetailsViewModel>()
                                              .FirstOrDefault();

            // each time a paste is viewed, its hits should grow up by 1
            Paste pasteToDb = this.Data.Pastes.GetById(idAsGuid);
            pasteToDb.Hits++;

            this.Data.Pastes.Update(pasteToDb);
            this.Data.SaveChanges();

            // Get paste's comments
            paste.Comments = this.Data.Comments
                                 .All()
                                 .OrderByDescending(c => c.Id)
                                 .Where(c => c.PasteId == id)
                                 .Project()
                                 .To<CommentViewModel>()
                                 .ToList();

            return this.View(paste);
        }

        [HttpPost]
        public ActionResult ReadPastes([DataSourceRequest]
                                       DataSourceRequest request, int? syntax, bool onlyMine, bool withBugs)
        {
            IQueryable<Paste> pastesQuery = this.Data.Pastes.All();

            if (syntax != null)
            {
                pastesQuery = 
                    pastesQuery
                        .Where(p => p.SyntaxId == syntax);
            }

            if (onlyMine)
            {
                pastesQuery = 
                    pastesQuery
                        .Where(p => p.AuthorId == this.CurrentUser.Id);
            }
            else
            {
                pastesQuery = 
                    pastesQuery
                        .Where(p => !p.IsPrivate);
            }

            if (withBugs)
            {
                pastesQuery = 
                    pastesQuery
                        .Where(p => p.HasBug);
            }

            IQueryable<BasePasteViewModel> pastes = pastesQuery
                                                       .Project()
                                                       .To<BasePasteViewModel>();

            return this.Json(pastes.ToDataSourceResult(request));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            var addPasteViewModel = new EditPasteViewModel()
            {
                Syntaxes = this.populator.GetSyntaxes()
            };

            return this.View(addPasteViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddPasteViewModel paste)
        {
            if (paste != null && this.ModelState.IsValid)
            {
                var pasteToDb = Mapper.DynamicMap<Paste>(paste);

                pasteToDb.AuthorId = this.CurrentUser.Id;
                pasteToDb.Description = this.sanitizer.Sanitize(pasteToDb.Description);

                this.Data.Pastes.Add(pasteToDb);
                this.Data.SaveChanges();

                // Each User receive points for a posted source code
                this.CurrentUser.Points += DefaultAdditionalPointsPerPaste;
                this.Data.Users.Update(this.CurrentUser);
                this.Data.SaveChanges();

                return this.RedirectToAction("All");
            }

            paste.Syntaxes = this.populator.GetSyntaxes();

            return this.View(paste);
        }

        [ChildActionOnly]
        public ActionResult UserOptions(string pasteId)
        {
            if (this.CurrentUser != null)
            {
                var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
                var options = new PasteUserOptionsViewModel()
                {
                    Id = pasteId,
                    HasBug = currentPaste.HasBug,
                    IsPrivate = currentPaste.IsPrivate,
                    HasCurrentUserAsAuthor = currentPaste.AuthorId == this.CurrentUser.Id
                };

                return this.PartialView("_UserOptions", options);
            }

            return new EmptyResult();
        }

        [Authorize]
        [HttpGet]
        public ActionResult FlagBug(string pasteId)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
            currentPaste.HasBug = true;

            this.Data.Pastes.Update(currentPaste);
            this.Data.SaveChanges();

            PasteDetailsViewModel pasteAsViewModel = Mapper.Map<PasteDetailsViewModel>(currentPaste);

            return this.View("Details", pasteAsViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult MakePrivate(string pasteId)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
            currentPaste.IsPrivate = true;

            this.Data.Pastes.Update(currentPaste);
            this.Data.SaveChanges();

            PasteDetailsViewModel pasteAsViewModel = Mapper.Map<PasteDetailsViewModel>(currentPaste);

            return this.View("Details", pasteAsViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(string pasteId)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(pasteId));
            var editModel = Mapper.Map<EditPasteViewModel>(currentPaste);

            editModel.Id = pasteId;
            editModel.Syntaxes = this.populator.GetSyntaxes();

            return this.View(editModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPasteViewModel paste, string id)
        {
            var currentPaste = this.Data.Pastes.GetById(new Guid(id));

            Mapper.CreateMap<EditPasteViewModel, Paste>();
            Mapper.Map<EditPasteViewModel, Paste>(paste, currentPaste);

            currentPaste.Description = this.sanitizer.Sanitize(currentPaste.Description);

            this.Data.Pastes.Update(currentPaste);
            this.Data.SaveChanges();

            return this.RedirectToAction("Details", new { id = id });
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

            return this.PartialView("_GetLatestPastesPartial", currentPastesInSyntax);
        }
    }
}