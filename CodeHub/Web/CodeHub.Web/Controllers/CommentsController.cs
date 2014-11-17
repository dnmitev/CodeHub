namespace CodeHub.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CodeHub.Data.Contracts;
    using CodeHub.Web.Infrastructure.Filters;
    using CodeHub.Data.Models;
    using CodeHub.Web.ViewModels.Comment;

    public class CommentsController : BaseController
    {
        private const int DefaultAdditionalPointsPerComment = 5;

        public CommentsController(ICodeHubData data) : base(data)
        {
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddComment(AddCommentViewModel comment)
        {
            if (comment != null && this.ModelState.IsValid)
            {
                Comment commentToDb = Mapper.DynamicMap<Comment>(comment);

                commentToDb.AuthorId = this.CurrentUser.Id;

                this.Data.Comments.Add(commentToDb);
                this.Data.SaveChanges();

                // each user receives points for a comment
                this.CurrentUser.Points += DefaultAdditionalPointsPerComment;
                this.Data.Users.Update(this.CurrentUser);

                this.Data.SaveChanges();

                Comment commentFromDb = this.Data.Comments
                                            .All()
                                            .OrderByDescending(c => c.CreatedOn)
                                            .FirstOrDefault();

                CommentViewModel viewModel = Mapper.Map<CommentViewModel>(commentFromDb);

                return this.PartialView("_CommentPartial", viewModel);
            }

            return this.PartialView("_CommentPartial", comment);
        }
        
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CommentOptions(int id)
        {
            if (this.CurrentUser != null)
            {
                bool isCurrentUsersComment = this.CurrentUser.Id == this.Data.Comments.GetById(id).AuthorId;
                if (isCurrentUsersComment)
                {
                    return this.PartialView("_CommentOptionsPartial", id);
                }
            }

            return new EmptyResult();
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Delete(int commentId)
        {
            Comment commentToDelete = this.Data.Comments.GetById(commentId);

            this.Data.Comments.Delete(commentToDelete);
            this.Data.SaveChanges();

            return Content(string.Empty);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Comment comment = this.Data.Comments.GetById(id);
            EditCommentViewModel model = Mapper.Map<EditCommentViewModel>(comment);
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCommentViewModel model, int id)
        {
            Comment commentToDb = this.Data.Comments.GetById(id);

            Mapper.CreateMap<EditCommentViewModel, Comment>();
            Mapper.Map<EditCommentViewModel, Comment>(model, commentToDb);

            this.Data.Comments.Update(commentToDb);
            this.Data.SaveChanges();

            return this.RedirectToAction("Details", "Pastes", new { id = commentToDb.PasteId });
        }
    }
}