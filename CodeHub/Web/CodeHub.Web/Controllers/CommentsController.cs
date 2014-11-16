namespace CodeHub.Web.Controllers
{
    using AutoMapper;
    using CodeHub.Data.Contracts;
    using CodeHub.Data.Models;
    using CodeHub.Web.ViewModels.Comment;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class CommentsController : BaseController
    {
        private const int DefaultAdditionalPointsPerComment = 5;

        public CommentsController(ICodeHubData data)
            : base(data)
        {
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(AddCommentViewModel comment)
        {
            if (comment != null && ModelState.IsValid)
            {
                var dbComment = Mapper.DynamicMap<Comment>(comment);

                dbComment.AuthorId = this.CurrentUser.Id;

                this.Data.Comments.Add(dbComment);
                this.Data.SaveChanges();

                // each user receives points for a comment
                this.CurrentUser.Points += DefaultAdditionalPointsPerComment;
                this.Data.Users.Update(this.CurrentUser);

                this.Data.SaveChanges();

                var viewModel = Mapper.Map<CommentViewModel>(dbComment);

                return PartialView("_CommentPartial", viewModel);
            }

            throw new HttpException(400, "Invalid comment");
        }

        public ActionResult CommentOptions(int id)
        {
            if (this.CurrentUser != null)
            {
                var isCurrentUsersComment = this.CurrentUser.Id == this.Data.Comments.GetById(id).AuthorId;
                if (isCurrentUsersComment)
                {
                    return PartialView("_CommentOptionsPartial", id);
                }
            }

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var commentToDelete = this.Data.Comments.GetById(id);

            this.Data.Comments.Delete(commentToDelete);
            this.Data.SaveChanges();

            return RedirectToAction("Details", "Pastes", new { id = commentToDelete.PasteId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var comment = this.Data.Comments.GetById(id);
            var model = Mapper.Map<EditCommentViewModel>(comment);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCommentViewModel model, int id)
        {
            var dbComment = this.Data.Comments.GetById(id);

            Mapper.CreateMap<EditCommentViewModel, Comment>();
            Mapper.Map<EditCommentViewModel, Comment>(model, dbComment);

            this.Data.Comments.Update(dbComment);
            this.Data.SaveChanges();

            return RedirectToAction("Details", "Pastes", new { id = dbComment.PasteId });
        }
    }
}