namespace CodeHub.Web.Areas.Administration.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;

    public class PasteViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        [UIHint("GridForeignKey")]
        public int SyntaxId { get; set; }

        public string Author { get; set; }

        [Display(Name = "Has bug")]
        public bool HasBug { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Paste, PasteViewModel>()
                         .ForMember(m => m.Author, opt => opt.MapFrom(p => p.Author.UserName))
                         .ReverseMap();
        }
    }
}