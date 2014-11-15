namespace CodeHub.Web.ViewModels.HomePage
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;

    public class PasteHomePageViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Author { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Paste, PasteHomePageViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(p => p.Id.ToString()))
                .ForMember(m => m.Author, opt => opt.MapFrom(a => a.Author.UserName))
                .ReverseMap();
        }
    }
}