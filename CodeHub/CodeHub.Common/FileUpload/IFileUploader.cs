namespace CodeHub.Common.FileUpload
{
    using System.Web;

    public interface IFileUploader
    {
        string GetUploadedFilePath(HttpPostedFileBase file, string mainFolderName, string subfolderName);
    }
}