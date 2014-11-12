namespace CodeHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CodeHub.Data.Common.Models;

    public class Syntax : DeletableEntity
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

        [Required]
        public string SyntaxMode { get; set; }

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