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

            // TODO: Set to true in production
            BundleTable.EnableOptimizations = false;
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/codemirror").Include(
                "~/Content/codemirror/codemirror.css",
                "~/Content/codemirror/vibrant-ink.css"));

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
                "~/Scripts/codemirror/codemirror.js",
                "~/Scripts/codemirror/mode/clike/clike.js",
                "~/Scripts/codemirror/mode/coffeescript/coffeescript.js",
                "~/Scripts/codemirror/mode/css/css.js",
                "~/Scripts/codemirror/mode/htmlembedded/htmlembedded.js",
                "~/Scripts/codemirror/mode/htmlmixed/htmlmixed.js",
                "~/Scripts/codemirror/mode/javascript/javascript.js",
                "~/Scripts/codemirror/mode/ruby/ruby.js",
                "~/Scripts/codemirror/mode/sql/sql.js"));

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