namespace CodeHub.Web.ViewModels.Other
{
    using System.ComponentModel.DataAnnotations;

    public class FilterViewModel
    {
        public FilterViewModel()
        {
            this.OnlyMine = false;
            this.WithBugs = false;
        }

        [Display(Name="Choose syntax")]
        [UIHint("SyntaxesDropDown")]
        public int? Syntax { get; set; }

        [Display(Name="Only mine pastes")]
        public bool OnlyMine { get; set; }

        [Display(Name = "Pastes with bugs")]
        public bool WithBugs { get; set; }
    }
}