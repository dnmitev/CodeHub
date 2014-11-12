namespace CodeHub.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;

    using System;
    using System.Data.Entity;
    using System.Linq;

    using CodeHub.Data.Contracts;
    using CodeHub.Data.Common.Models;
    using CodeHub.Data.Migrations;
    using CodeHub.Data.Models;

    public class CodeHubDbContext : IdentityDbContext<User>, ICodeHubDbContext
    {
        public CodeHubDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CodeHubDbContext, Configuration>());
        }

        public IDbSet<Paste> Pastes { get; set; }

        public IDbSet<Repo> Repos { get; set; }

        public IDbSet<Syntax> Syntaxes { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        public static CodeHubDbContext Create()
        {
            return new CodeHubDbContext();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return base.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                            this.ChangeTracker.Entries()
                                .Where(
                                     e =>
                                         e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (
                var entry in
                    this.ChangeTracker.Entries()
                        .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}