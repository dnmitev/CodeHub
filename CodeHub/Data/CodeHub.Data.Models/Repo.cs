namespace CodeHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using CodeHub.Data.Common.Models;

    public class Repo : IAuditInfo, IDeletableEntity
    {
        private ICollection<Paste> pastes;

        public Repo()
        {
            this.Id = Guid.NewGuid();
            this.Pastes = new HashSet<Paste>();
        }

        [Key]
        public Guid Id { get; set; }

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

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Range(0,int.MaxValue)]
        public int Rating { get; set; }
    }
}