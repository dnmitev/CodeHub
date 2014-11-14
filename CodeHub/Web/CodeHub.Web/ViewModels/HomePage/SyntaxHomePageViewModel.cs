namespace CodeHub.Web.ViewModels.HomePage
{
    using CodeHub.Data.Models;
    using CodeHub.Web.Infrastructure.Mapping;

    public class SyntaxHomePageViewModel : IMapFrom<Syntax>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}