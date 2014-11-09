namespace CodeHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using CodeHub.Data.Common.Models;

    public class Syntax : IAuditInfo, IDeletableEntity
    {
        private ICollection<Paste> pastes;

        public Syntax()
        {
            this.Pastes = new HashSet<Paste>();
        }

        [Key]
        public int Id { get; set; }

        [MinLength(1)]
        [MaxLength(20)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Paste> Pastes
        {
            get
            {
                return this.pastes;
            }

            set
            {
                this.pastes = value;
            }
        }
    }
}