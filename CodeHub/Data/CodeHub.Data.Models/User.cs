﻿namespace CodeHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    
    using CodeHub.Common;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Paste> pastes;
        private ICollection<Repo> repos;
        private ICollection<Comment> comments;

        public User()
        {
            this.Pastes = new HashSet<Paste>();
            this.Repos = new HashSet<Repo>();
            this.Comments = new HashSet<Comment>();
        }

        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [DefaultValue(GlobalConstants.DefaultUserAvatar)]
        public string Avatar { get; set; }

        [Range(0, int.MaxValue)]
        [DefaultValue(100)]
        public int Points { get; set; }

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }
    }
}