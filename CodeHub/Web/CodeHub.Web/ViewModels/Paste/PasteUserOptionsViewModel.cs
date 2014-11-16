namespace CodeHub.Web.ViewModels.Paste
{
    public class PasteUserOptionsViewModel
    {
        public string Id { get; set; }

        public bool HasBug { get; set; }

        public bool IsPrivate { get; set; }

        public bool HasCurrentUserAsAuthor { get; set; }
    }
}