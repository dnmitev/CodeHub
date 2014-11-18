namespace CodeHub.Web.ViewModels.User
{
    using System;
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

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = " Points so far")]
        public int Points { get; set; }

        public string Avatar { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>()
                         .ForMember(m => m.Id, opt => opt.MapFrom(u => u.Id.ToString()))
                         .ForMember(m => m.FullName, opt => opt.MapFrom(u => string.Format("{0} {1}", u.FirstName, u.LastName)))
                         .ReverseMap();
        }
    }
}