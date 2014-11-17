namespace CodeHub.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using CodeHub.Common;
    using CodeHub.Common.RandomGenerator;
    using CodeHub.Common.RandomGenerator.Contracts;
    using CodeHub.Data.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeHubDbContext>
    {
        private readonly IList<User> users;
        private readonly IList<Comment> comments;

        private readonly IDictionary<string, string> pastesData;

        private readonly IRandomProvider randomProvider;

        private IList<Syntax> syntaxes;
        private IList<Paste> pastes;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;

            this.users = new List<User>();
            this.comments = new List<Comment>();

            this.pastesData = new Dictionary<string, string>();

            this.randomProvider = RandomProvider.Instance;
        }

        protected override void Seed(CodeHubDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == GlobalConstants.AdminRole))
            {
                this.SeedRoles(context);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                this.SeedUsers(context);
                context.SaveChanges();
            }

            if (!context.Syntaxes.Any())
            {
                this.SeedSyntaxes(context);
                context.SaveChanges();
            }

            if (!context.Pastes.Any())
            {
                this.SeedPastes(context);
                context.SaveChanges();
            }

            if (!context.Comments.Any())
            {
                this.SeedComments(context);
                context.SaveChanges();
            }

            if (!context.Repos.Any())
            {
                this.SeedRepo(context);
                context.SaveChanges();
            }
        }
 
        private void SeedRepo(CodeHubDbContext context)
        {
            User currentOwner = this.users[this.randomProvider.GetRandomInt(0, this.users.Count - 1)];
            context.Repos.Add(
                new Repo()
                {
                    Name = this.randomProvider.GetRandomLengthString(5, 30),
                    Owner = currentOwner,
                    Pastes = currentOwner.Pastes
                });
        }

        private void SeedComments(CodeHubDbContext context)
        {
            List<Paste> pastesToDb = context.Pastes.ToList();
            int numberOfComments = this.randomProvider.GetRandomInt(5, 10);
            for (int i = 0; i < numberOfComments; i++)
            {
                this.comments.Add(
                    new Comment()
                    {
                        Author = this.users[this.randomProvider.GetRandomInt(0, this.users.Count - 1)],
                        Content = this.randomProvider.GetRandomString(150),
                        Paste = pastesToDb[this.randomProvider.GetRandomInt(0, pastesToDb.Count - 1)]
                    });
            }

            foreach (Comment comment in this.comments)
            {
                context.Comments.Add(comment);
            }
        }

        private void SeedPastes(CodeHubDbContext context)
        {
            this.ReadPastesFromFile();

            this.pastes = new List<Paste>()
            {
                new Paste
                {
                    Author = this.users[this.randomProvider.GetRandomInt(0, this.users.Count - 1)],
                    Title = "Find Catalan's numbers problem",
                    Content = this.pastesData["C#"],
                    Description = this.pastesData["description"],
                    Syntax = this.syntaxes[0]
                },
                new Paste
                {
                    Author = this.users[this.randomProvider.GetRandomInt(0, this.users.Count - 1)],
                    Title = "HTML5 Example",
                    Content = this.pastesData["HTML"],
                    Description = this.pastesData["description"],
                    Syntax = this.syntaxes[2]
                },
                new Paste
                {
                    Author = this.users[this.randomProvider.GetRandomInt(0, this.users.Count - 1)],
                    Title = "jQuery plugin with CoffeeScript",
                    Content = this.pastesData["CoffeeScript"],
                    Description = this.pastesData["description"],
                    Syntax = this.syntaxes[5]
                },
                new Paste
                {
                    Author = this.users[this.randomProvider.GetRandomInt(0, this.users.Count - 1)],
                    Title = "JavaScript source code example",
                    Content = this.pastesData["JavaScript"],
                    Description = this.pastesData["description"],
                    Syntax = this.syntaxes[8]
                },
                new Paste
                {
                    Author = this.users[this.randomProvider.GetRandomInt(0, this.users.Count - 1)],
                    Title = "CSS source code example",
                    Content = this.pastesData["CSS"],
                    Description = this.pastesData["description"],
                    Syntax = this.syntaxes[3]
                }
            };

            foreach (Paste paste in this.pastes)
            {
                context.Pastes.Add(paste);
            }
        }

        private void ReadPastesFromFile()
        {
            string codeBase = Assembly.GetCallingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string ddlPath = Path.GetDirectoryName(path);

            using (var reader = new StreamReader(string.Format("{0}\\SeedData\\CatalanNumber.cs", ddlPath), Encoding.UTF8))
            {
                this.pastesData["C#"] = reader.ReadToEnd();
            }

            using (var reader = new StreamReader(string.Format("{0}\\SeedData\\index.html", ddlPath), Encoding.UTF8))
            {
                this.pastesData["HTML"] = reader.ReadToEnd();
            }

            using (var reader = new StreamReader(string.Format("{0}\\SeedData\\scripts.coffee", ddlPath), Encoding.UTF8))
            {
                this.pastesData["CoffeeScript"] = reader.ReadToEnd();
            }

            using (var reader = new StreamReader(string.Format("{0}\\SeedData\\scripts.js", ddlPath), Encoding.UTF8))
            {
                this.pastesData["JavaScript"] = reader.ReadToEnd();
            }

            using (var reader = new StreamReader(string.Format("{0}\\SeedData\\styles.css", ddlPath), Encoding.UTF8))
            {
                this.pastesData["CSS"] = reader.ReadToEnd();
            }

            using (var reader = new StreamReader(string.Format("{0}\\SeedData\\lorem.txt", ddlPath), Encoding.UTF8))
            {
                this.pastesData["description"] = reader.ReadToEnd();
            }
        }

        private void SeedUsers(CodeHubDbContext context)
        {
            this.SeedAdmins(context);

            for (int i = 0; i < 3; i++)
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);
                var user = new User { UserName = string.Format("pesho{0}", i) };

                manager.Create(user, "asdasd");
                context.SaveChanges();

                this.users.Add(user);
            }
        }

        private void SeedAdmins(CodeHubDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == "admin@admin.bg"))
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);
                var user = new User { UserName = "admin" };

                manager.Create(user, "asdasd");
                context.SaveChanges();

                manager.AddToRole(user.Id, GlobalConstants.AdminRole);
                context.SaveChanges();
            }
        }

        private void SeedRoles(CodeHubDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole { Name = GlobalConstants.AdminRole };

            manager.Create(role);
        }

        private void SeedSyntaxes(CodeHubDbContext context)
        {
            this.syntaxes = new List<Syntax>()
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

            foreach (Syntax syntax in this.syntaxes)
            {
                context.Syntaxes.Add(syntax);
            }
        }
    }
}