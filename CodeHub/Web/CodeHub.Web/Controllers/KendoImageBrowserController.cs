namespace CodeHub.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;

    using Kendo.Mvc.UI;

    public class KendoImageBrowserController : EditorImageBrowserController
    {
        private const string ContentFolderRoot = "~/Content/";
        private const string PrettyName = "Images/";

        private static readonly string[] FoldersToCopy = new[] { "~/Content/shared/" };

        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string ContentPath
        {
            get
            {
                return this.CreateUserFolder();
            }
        }

        private string CreateUserFolder()
        {
            string virtualPath = Path.Combine(ContentFolderRoot, "UserFiles", PrettyName);

            string path = this.Server.MapPath(virtualPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (string sourceFolder in FoldersToCopy)
                {
                    this.CopyFolder(Server.MapPath(sourceFolder), path);
                }
            }

            return virtualPath;
        }

        private void CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (string file in Directory.EnumerateFiles(source))
            {
                string dest = Path.Combine(destination, Path.GetFileName(file));
                System.IO.File.Copy(file, dest);
            }

            foreach (string folder in Directory.EnumerateDirectories(source))
            {
                string dest = Path.Combine(destination, Path.GetFileName(folder));
                this.CopyFolder(folder, dest);
            }
        }
    }
}