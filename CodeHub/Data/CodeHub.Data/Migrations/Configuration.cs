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
            }

            SeedPaste(context);
        }

        private void SeedPaste(CodeHubDbContext context)
        {
            if (!context.Pastes.Any())
            {
                var paste =
                    new Paste
                    {
                        Author = new User { UserName = "bai dan4o", Email = "baidan4o@abv.bg" },
                        Content =
                            @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngryFemaleGPS
{
    class AngryFemaleGPS
    {
        static void Main(string[] args)
        {
            //input the G PiSi number
            string number = Console.ReadLine();
            number = number.TrimStart(new char[] { '0', '-' });

            //find the sums of odd and even digits
            int oddSum = 0;
            int evenSum = 0;

            for (int i = 0; i < number.Length; i++)
            {
                int digit = number[i] - '0';

                if ((digit & 1) == 1)
                {
                    oddSum += digit;
                }
                else
                {
                    evenSum += digit;
                }
            }
         
            //chech whether the odd digits' sum is bigger than the even's or they are equal
            if (evenSum > oddSum)
            {
                Console.WriteLine(""right {0}"",evenSum);
            }
            else if (oddSum > evenSum)
            {
                Console.WriteLine(""left {0}"", oddSum);
            }
            else
            {
                Console.WriteLine(""straight {0}"",evenSum);
            }
        }
    }
}",
                        Title = "Angry Femele GPS",
                        Syntax = new Syntax { Name = "C#", SyntaxMode = "text/x-csharp" }
                    };

                context.Pastes.Add(paste);
            }
        }

        private void SeedSyntaxes(CodeHubDbContext context)
        {
            var syntaxes = new List<Syntax>()
            {
                new Syntax { Name = "C++", SyntaxMode = "text/x-c++src" },
                new Syntax { Name = "JavaScript", SyntaxMode = "text/javascript" },
                new Syntax { Name = "HTML", SyntaxMode = "text/html" },
                new Syntax { Name = "CSS", SyntaxMode = "text/css" },
                new Syntax { Name = "SQL", SyntaxMode = "text/x-mssql" },
                new Syntax { Name = "CoffeeScript", SyntaxMode = "text/x-coffeescript" },
                new Syntax { Name = "Java", SyntaxMode = "text/x-java" },
                new Syntax { Name = "Ruby", SyntaxMode = "text/x-ruby" },
                new Syntax { Name = "TypeScript", SyntaxMode = "text/typescript" }
            };

            foreach (var syntax in syntaxes)
            {
                context.Syntaxes.Add(syntax);
            }
        }
    }
}