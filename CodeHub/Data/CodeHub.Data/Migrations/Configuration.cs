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
                SeedSyntaxes(context);
                context.SaveChanges();
            }


        }

        private void SeedSyntaxes(CodeHubDbContext context)
        {
            var syntaxes = new List<Syntax>()
            {
                new Syntax { Name = "C#", SyntaxMode = "text/x-csharp" },
                new Syntax { Name = "C++", SyntaxMode = "text/x-c++src" },
                new Syntax { Name = "HTML", SyntaxMode = "text/html" },
                new Syntax { Name = "CSS", SyntaxMode = "text/css" },
                new Syntax { Name = "SQL", SyntaxMode = "text/x-mssql" },
                new Syntax { Name = "CoffeeScript", SyntaxMode = "text/x-coffeescript" },
                new Syntax { Name = "Java", SyntaxMode = "text/x-java" },
                new Syntax { Name = "Ruby", SyntaxMode = "text/x-ruby" },
                new Syntax { Name = "JavaScript", SyntaxMode = "text/javascript" },
                new Syntax { Name = "TypeScript", SyntaxMode = "text/typescript" }
            };

            foreach (var syntax in syntaxes)
            {
                context.Syntaxes.Add(syntax);
            }
        }
    }
}