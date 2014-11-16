namespace CodeHub.Web.ViewModels.Paste
{
    using AutoMapper;
    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    
    public class PasteDetailsViewModel : BasePasteViewModel, IHaveCustomMappings
    {
        [MinLength(10)]
        [UIHint("SourceCode")]
        public string Content { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        [MinLength(50)]
        public string Description { get; set; }

        [Display(Name="Private")]
        public bool IsPrivate { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Paste, PasteDetailsViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(p => p.Id.ToString()))
                .ForMember(m => m.Syntax, opt => opt.MapFrom(p => p.Syntax.Name))
                .ForMember(m => m.SyntaxMode, opt => opt.MapFrom(p => p.Syntax.SyntaxMode))
                .ForMember(m => m.Author, opt => opt.MapFrom(p => p.Author.UserName))
                .ReverseMap();
        }
    }
}