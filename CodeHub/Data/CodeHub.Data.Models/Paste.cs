namespace CodeHub.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using CodeHub.Data.Common.Models;

    public class Paste : IAuditInfo, IDeletableEntity
    {
        public Paste()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [MinLength(10)]
        public string Content { get; set; }

        public int SyntaxId { get; set; }

        public virtual Syntax Syntax { get; set; }

        [Range(0,int.MaxValue)]
        public int Hits { get; set; }

        public bool IsPrivate { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public bool HasBug { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string RepoId { get; set; }

        public virtual Repo Repo { get; set; }
    }
}