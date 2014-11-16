namespace CodeHub.Web.ViewModels.Comment
{
    using System;

    using AutoMapper;

    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;
    using System.ComponentModel.DataAnnotations;

    public class CommentViewModel : BaseCommentViewModel, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public int AuthorPoints { get; set; }

        [UIHint("CustomDate")]
        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
                .ForMember(m => m.AuthorName, opt => opt.MapFrom(c => c.Author.UserName))
                .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(c => c.Author.Points))
                .ReverseMap();
        }
    }
}