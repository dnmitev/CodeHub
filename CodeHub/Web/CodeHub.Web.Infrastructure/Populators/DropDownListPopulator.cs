namespace CodeHub.Web.Infrastructure.Populators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using CodeHub.Data.Contracts;
    using CodeHub.Web.Infrastructure.Caching;

    public class DropDownListPopulator : IDropDownListPopulator
    {
        private readonly ICodeHubData data;
        private readonly ICacheService cache;

        public DropDownListPopulator(ICodeHubData data, ICacheService cache)
        {
            this.data = data;
            this.cache = cache;
        }

        public IEnumerable<SelectListItem> GetSyntaxes()
        {
            var syntaxes = this.cache.Get<IEnumerable<SelectListItem>>("syntaxes",
                () =>
                {
                    return this.data.Syntaxes
                       .All()
                       .Select(c => new SelectListItem
                       {
                           Value = c.Id.ToString(),
                           Text = c.Name
                       })
                       .ToList();
                });

            return syntaxes;
        }
    }
}