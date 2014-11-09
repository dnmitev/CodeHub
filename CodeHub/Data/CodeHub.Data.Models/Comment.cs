namespace CodeHub.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    using CodeHub.Data.Common.Models;

    public class Comment : IAuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [MinLength(5)]
        public string Content { get; set; }

        public string PasteId { get; set; }

        public virtual Paste Paste { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}