namespace CodeHub.Web.ViewModels.Paste
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;

    public class AddPasteViewModel : IMapFrom<Paste>
    {
        [Required]
        [AllowHtml]
        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(10)]
        [UIHint("SourceCode")]
        public string Content { get; set; }

        [Required]
        [AllowHtml]
        [UIHint("KendoTextEditor")]
        [MinLength(50)]
        public string Description { get; set; }

        [UIHint("DropDownList")]
        [Display(Name = "Source code syntax")]
        public int SyntaxId { get; set; }

        public IEnumerable<SelectListItem> Syntaxes { get; set; }

        [Display(Name = "I want this to be private")]
        public bool IsPrivate { get; set; }

        [Display(Name = "I have a bug")]
        public bool HasBug { get; set; }
    }
}