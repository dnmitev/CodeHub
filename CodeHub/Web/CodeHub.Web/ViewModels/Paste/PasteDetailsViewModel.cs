namespace CodeHub.Web.ViewModels.Paste
{
    using CodeHub.Web.Infrastructure.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using CodeHub.Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class PasteDetailsViewModel : IMapFrom<Paste>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        [MinLength(10)]
        public string Content { get; set; }

        [MinLength(50)]
        public string Description { get; set; }

        public int SyntaxId { get; set; }

        public virtual Syntax Syntax { get; set; }

        [Range(0, int.MaxValue)]
        public int Hits { get; set; }

        public bool IsPrivate { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public bool HasBug { get; set; }



        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}