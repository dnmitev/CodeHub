namespace CodeHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using CodeHub.Data.Common.Models;

    public class Repo : DeletableEntity
    {
        private ICollection<Paste> pastes;

        public Repo()
        {
            this.Id = Guid.NewGuid();
            this.Pastes = new HashSet<Paste>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }

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

        [Range(0, int.MaxValue)]
        public int Rating { get; set; }
    }
}