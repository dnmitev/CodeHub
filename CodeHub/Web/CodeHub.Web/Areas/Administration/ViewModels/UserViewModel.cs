namespace CodeHub.Web.Areas.Administration.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;

    public class UserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        public string UserName { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [DefaultValue(0)]
        [Display(Name = "Pastes made")]
        public int PastesCount { get; set; }

        [DefaultValue(0)]
        [Display(Name = "Comments made")]
        public int CommentsMade { get; set; }

        [DefaultValue(0)]
        [Display(Name = "Total score")]
        public int Points { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>()
                         .ForMember(m => m.FullName, opt => opt.MapFrom(u => u.FirstName + " " + u.LastName))
                         .ForMember(m => m.PastesCount, opt => opt.MapFrom(u => u.Pastes.Count))
                         .ForMember(m => m.CommentsMade, opt => opt.MapFrom(u => u.Comments.Count))
                         .ReverseMap();
        }
    }
}