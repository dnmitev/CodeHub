namespace CodeHub.Data.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CodeHub.Data.Common.Models;

    public class User : IdentityUser, IAuditInfo, IDeletableEntity
    {
        private ICollection<Paste> pastes;
        private ICollection<Repo> repos;
        private ICollection<Comment> comments;
        
        public User()
        {
            this.Pastes = new HashSet<Paste>();
            this.Repos = new HashSet<Repo>();
            this.Comments = new HashSet<Comment>();

            // Prevent UserManager.CreateAsyns cause an exception
            this.CreatedOn = DateTime.Now;
        }

        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        public string Avatar { get; set; }

        [Range(0, int.MaxValue)]
        public int Points { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

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

        public virtual ICollection<Comment> Comments
        {
            get
            {
                return this.comments;
            }

            set
            {
                this.comments = value;
            }
        }

        public virtual ICollection<Repo> Repos
        {
            get
            {
                return this.repos;
            }

            set
            {
                this.repos = value;
            }
        }
    }
}