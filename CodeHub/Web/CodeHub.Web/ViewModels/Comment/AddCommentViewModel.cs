namespace CodeHub.Web.ViewModels.Comment
{
    public class AddCommentViewModel : BaseCommentViewModel
    {
        public AddCommentViewModel()
        {
        }

        public AddCommentViewModel(string pasteId)
        {
            this.PasteId = pasteId;
        }

        public string PasteId { get; set; }
    }
}