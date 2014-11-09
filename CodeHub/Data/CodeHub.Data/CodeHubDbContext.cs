namespace CodeHub.Data
{
    using CodeHub.Data.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CodeHubDbContext : IdentityDbContext<User>
    {
        public CodeHubDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static CodeHubDbContext Create()
        {
            return new CodeHubDbContext();
        }
    }
}