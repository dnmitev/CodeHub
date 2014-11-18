namespace CodeHub.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper;

    using CodeHub.Common.FileUpload;
    using CodeHub.Data.Contracts;

    using ViewModel = CodeHub.Web.ViewModels.User.UserViewModel;

    public class UserController : BaseController
    {
        private const string DefaultFileUploadFolder = "imgs";

        private readonly IFileUploader fileUploader;

        public UserController(ICodeHubData data, IFileUploader fileUploader) : base(data)
        {
            this.fileUploader = fileUploader;
        }

        public ActionResult Index()
        {
            ViewModel userModel = Mapper.Map<ViewModel>(this.CurrentUser);
            return this.View(userModel);
        }

        [HttpPost]
        public ActionResult Edit(ViewModel model, HttpPostedFileBase avatar)
        {
            if (model != null && this.ModelState.IsValid)
            {
                string[] names = model.FullName.Split(new char[] { ' ' });

                this.CurrentUser.FirstName = names[0];
                this.CurrentUser.LastName = names[1];
                this.CurrentUser.Email = model.Email;
                if (avatar != null)
                {
                    this.CurrentUser.Avatar = this.fileUploader.GetUploadedFilePath(avatar, DefaultFileUploadFolder, this.CurrentUser.UserName);
                }

                this.Data.Users.Update(this.CurrentUser);
                this.Data.SaveChanges();

                return this.RedirectToAction("Index", "User");
            }

            return new EmptyResult();
        }
    }
}