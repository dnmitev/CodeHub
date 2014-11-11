using System.Web;
using System.Web.Optimization;

namespace CodeHub.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            RegisterScriptBundles(bundles);
            RegisterStyleBundles(bundles);

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            // TODO: Set to true in production
            BundleTable.EnableOptimizations = false;
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/codemirror").Include(
                "~/Content/codemirror-3.01/codemirror.css",
                "~/Content/codemirror-3.0/theme/vibrant-ink.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.darkly.css"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                "~/Content/kendo/kendo.common.min.css",
                "~/Content/kendo/kendo.common-bootstrap.min.css",
                "~/Content/kendo/kendo.black.min.css"));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                "~/Content/site.css"));
        }

        private static void RegisterScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/kendo/kendo.all.min.js",
                "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/codemirror").Include(
                "~/Scripts/codemirror-2.37/lib/codemirror.js",
                "~/Scripts/codemirror-2.37/mode/clike/clike.js",
                "~/Scripts/codemirror-2.37/mode/coffeescript/coffeescript.js",
                "~/Scripts/codemirror-2.37/mode/css/css.js",
                "~/Scripts/codemirror-2.37/mode/htmlembedded/htmlembedded.js",
                "~/Scripts/codemirror-2.37/mode/htmlmixed/htmlmixed.js",
                "~/Scripts/codemirror-2.37/mode/javascript/javascript.js",
                "~/Scripts/codemirror-2.37/mode/ruby/ruby.js",
                "~/Scripts/codemirror-2.37/mode/sql/sql.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/kendo/jquery.min.js"));
            //.Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));
        }
    }
}