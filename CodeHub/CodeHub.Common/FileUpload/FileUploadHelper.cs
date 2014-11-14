namespace CodeHub.Common.FileUpload
{
    using System;
    using System.IO;
    using System.Web;

    public class FileUploadHelper : IFileUploader
    {
        public string GetUploadedFilePath(HttpPostedFileBase file, string mainFolderName, string subfolderName)
        {
            string fileExtension = this.GetFileExtension(file);
            string uniqueFileName = string.Format("{0}.{1}", Guid.NewGuid(), fileExtension);

            this.GetDirectoryByPathAndName(mainFolderName, string.Format("~/{0}", mainFolderName));
            this.GetDirectoryByPathAndName(subfolderName, string.Format("~/{0}/{1}", mainFolderName, subfolderName));

            file.SaveAs(HttpContext.Current.Server.MapPath(string.Format("~/{0}/{1}/{2}", mainFolderName, subfolderName, uniqueFileName)));

            return string.Format("/{0}/{1}/{2}", mainFolderName, subfolderName, uniqueFileName);
        }

        private void GetDirectoryByPathAndName(string dirName, string pathPattern)
        {
            bool dirExists = Directory.Exists(HttpContext.Current.Server.MapPath(dirName));
            if (!dirExists)
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(pathPattern));
            }
        }

        private string GetFileExtension(HttpPostedFileBase file)
        {
            string[] fileData = Path.GetFileName(file.FileName).Split(new char[] { '.' });
            string fileExtension = fileData[fileData.Length - 1];

            return fileExtension;
        }
    }
}