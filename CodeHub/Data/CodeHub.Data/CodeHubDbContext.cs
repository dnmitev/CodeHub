namespace CodeHub.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    using CodeHub.Data.Migrations;
    using CodeHub.Data.Models;

    public class CodeHubDbContext : IdentityDbContext<User>
    {
        public CodeHubDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CodeHubDbContext, Configuration>());
        }

        public static CodeHubDbContext Create()
        {
            return new CodeHubDbContext();
        }
    }
}