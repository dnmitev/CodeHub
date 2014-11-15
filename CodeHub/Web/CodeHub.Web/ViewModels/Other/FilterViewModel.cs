namespace CodeHub.Web.ViewModels.Other
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class FilterViewModel
    {
        public int? Syntax { get; set; }

        public bool? OnlyMine { get; set; }

        public bool? WithBugs { get; set; }
    }
}