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

            if (!context.Pastes.Any())
            {
                SeedPaste(context);
            }
        }

        private void SeedPaste(CodeHubDbContext context)
        {
            var user = new User { UserName = "bai dan4o", Email = "baidan4o@abv.bg" };
            var pastes = GetListOfPastes(user);
            foreach (var paste in pastes)
            {
                context.Pastes.Add(paste);
            }
        }

        private IList<Paste> GetListOfPastes(User user)
        {
            var pastes = new List<Paste>
            {
                new Paste
                {
                    Author = user,
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
         
            //check whether the odd digits' sum is bigger than the even's or they are equal
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
                },
                new Paste
                {
                    Author = user,
                    Content =
@"(function () {
    'use strict';

    $.fn.dropdown = function () {
        var $this = this;

        $this.hide();

        var $dropdownListCointainer = $('<div />').addClass('dropdown-list-container'),
            $listContainer = $('<ul />').addClass('dropdown-list-options'),
            $selectOptions = $('option');

        for (var i = 0, len = $selectOptions.length; i < len; i += 1) {
            var currentOptionValue = $selectOptions[i].value,
                currentOptionHtml = $selectOptions[i].innerHTML,
                $listItem = $('<li />')
                                .addClass('dropdown-list-option')
                                .html(currentOptionHtml)
                                .data('data-value', currentOptionValue)
                                .on('click', onListItemClick);

            $listContainer.append($listItem);
        }

        $dropdownListCointainer.append($listContainer);

        $this.after($dropdownListCointainer);

        function onListItemClick() {
            var self = this,
                dataValue = $(self).data('data-value'),
                selector = 'option[value=' + dataValue + ']';

            if ($('#dropdown').find(selector).attr('selected') === 'selected') {
                $('#dropdown').find(selector).removeAttr('selected', '');
                $(this).removeClass('dropdown-styled');
            } else {
                $('#dropdown').find(selector).attr('selected', 'selected');
                $(this).addClass('dropdown-styled');
            }
        }

        return $this;
    };

    $('#dropdown').dropdown();
}());",
                    Title = "jQuery plugin",
                    Syntax = new Syntax { Name = "JavaScript", SyntaxMode = "text/javascript" }
                }
            };

            return pastes;
        }

        private void SeedSyntaxes(CodeHubDbContext context)
        {
            var syntaxes = new List<Syntax>()
            {
                new Syntax { Name = "C++", SyntaxMode = "text/x-c++src" },
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