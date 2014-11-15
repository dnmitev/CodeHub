namespace CodeHub.Web.ViewModels.Paste
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using AutoMapper;

    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;

    public class BasePasteViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        [HiddenInput]
        public string Id { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Syntax { get; set; }

        public string SyntaxMode { get; set; }

        [Range(0, int.MaxValue)]
        public int Hits { get; set; }

        public string Author { get; set; }

        [UIHint("CustomBug")]
        [Display(Name = "I saw a bug")]
        public bool HasBug { get; set; }

        [UIHint("CustomDate")]
        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Paste, BasePasteViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(p => p.Id.ToString()))
                .ForMember(m => m.Syntax, opt => opt.MapFrom(p => p.Syntax.Name))
                .ForMember(m => m.SyntaxMode, opt => opt.MapFrom(p => p.Syntax.SyntaxMode))
                .ForMember(m => m.Author, opt => opt.MapFrom(p => p.Author.UserName))
                .ReverseMap();
        }
    }
}