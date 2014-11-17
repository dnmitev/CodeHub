namespace CodeHub.Web.ViewModels.Comment
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;

    public class BaseCommentViewModel : IMapFrom<Comment>
    {
        [Required]
        [AllowHtml]
        [MinLength(5)]
        [UIHint("BootstrapTextArea")]
        [Display(Name = "Enter your comment")]
        public string Content { get; set; }
    }
}