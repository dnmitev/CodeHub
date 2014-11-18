namespace CodeHub.Web.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    
    using AutoMapper;

    using CodeHub.Data.Contracts;
    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Filters;
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

            return this.Content(string.Empty);
        }

        [HttpPost]
        public ActionResult Edit(EditCommentViewModel model, int id)
        {
            if (model != null && ModelState.IsValid)
            {
                Comment commentToDb = this.Data.Comments.GetById(id);

                Mapper.CreateMap<EditCommentViewModel, Comment>();
                Mapper.Map<EditCommentViewModel, Comment>(model, commentToDb);

                this.Data.Comments.Update(commentToDb);
                this.Data.SaveChanges();

                var modelToReturn = Mapper.Map<EditCommentViewModel>(commentToDb);

                return this.Json(modelToReturn);
            }
            
            var errorList = ModelState.Values
                                      .SelectMany(m => m.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();

            throw new HttpException();
        }
    }
}