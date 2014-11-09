namespace CodeHub.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using CodeHub.Data.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeHubDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CodeHubDbContext context)
        {
            if (!context.Syntaxes.Any())
            {
                var syntaxes = new List<Syntax>()
                {
                    new Syntax {Name="C#"},
                    new Syntax {Name="JavaScript"},
                    new Syntax {Name="HTML"},
                    new Syntax {Name="CSS"},
                    new Syntax {Name="SQL"},
                    new Syntax {Name="C++"}
                };

                foreach (var syntax in syntaxes)
                {
                    context.Syntaxes.Add(syntax);
                }
            }
        }
    }
}
