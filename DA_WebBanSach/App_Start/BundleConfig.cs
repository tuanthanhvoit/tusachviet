using System.Web;
using System.Web.Optimization;

namespace DA_WebBanSach
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerytemplate").Include(
                        "~/Scripts/easing.js",
                        "~/Scripts/lavalamp.js",
                        "~/Scripts/easySlider1.7.js",
                        "~/Scripts/pajinate.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scriptsAdmin").Include(
                        "~/Scripts/js/jquery.js",
                        "~/Scripts/js/jquery-ui-1.8.16.custom.js",
                        "~/Scripts/js/bootstrap.js",
                        "~/Scripts/js/prettify.js",
                        "~/Scripts/js/sparkline.js",
                        "~/Scripts/js/nicescroll.js",
                        "~/Scripts/js/accordion.js",
                        "~/Scripts/js/smart-wizard.js",
                        "~/Scripts/js/vaidation.js",
                        "~/Scripts/js/dynamic-form.js",
                        "~/Scripts/js/fullcalendar.js",
                        "~/Scripts/js/raty.js",
                        "~/Scripts/js/noty.js",
                        "~/Scripts/js/cleditor.js",
                        "~/Scripts/js/data-table.js",
                        "~/Scripts/js/TableTools.js",
                        "~/Scripts/js/ColVis.js",
                        "~/Scripts/js/plupload.full.js",
                        "~/Scripts/js/elfinder/elfinder.js",
                        "~/Scripts/js/chosen.js",
                        "~/Scripts/js/uniform.js",
                        "~/Scripts/js/tagsinput.js",
                        "~/Scripts/js/colorbox.js",
                        "~/Scripts/js/check-all.js",
                        "~/Scripts/js/inputmask.js",
                        "~/Scripts/js/browserplus.js",
                        "~/Scripts/js/plupupload/jquery.plupload.queue/plupload.queue.js",
                        "~/Scripts/js/excanvas.js",
                        "~/Scripts/js/jqplot.js",
                        "~/Scripts/js/custom.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/chartJS").Include(
                        "~/Scripts/js/chart/jqplot.highlighter.js",
                        "~/Scripts/js/chart/jqplot.cursor.js",
                        "~/Scripts/js/chart/jqplot.barRenderer.js",
                        "~/Scripts/js/chart/jqplot.pointLabels.js",
                        "~/Scripts/js/chart/jqplot.dateAxisRenderer.js",
                        "~/Scripts/js/chart/jqplot.pieRenderer.js",
                        "~/Scripts/js/chart/jqplot.donutRenderer.js",
                        "~/Scripts/js/chart/jqplot.categoryAxisRenderer.js",
                        "~/Scripts/js/chart/jqplot.logAxisRenderer.min.js",
                        "~/Scripts/js/chart/jqplot.canvasTextRenderer.js",
                        "~/Scripts/js/chart/jqplot.canvasAxisTickRenderer.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/ff.css"));

            bundles.Add(new StyleBundle("~/Content/cssAdmin").Include(
                        "~/Content/css/bootstrap.css",
                        "~/Content/css/bootstrap-responsive.css",
                        "~/Content/css/jquery-ui-1.8.16.custom.css",
                        "~/Content/css/jquery.jqplot.css",
                        "~/Content/css/prettify.css",
                        "~/Content/css/elfinder.min.css",
                        "~/Content/css/elfinder.theme.css",
                        "~/Content/css/fullcalendar.css",
                        "~/Content/css/jquery.plupload.queue.css",
                        "~/Content/css/styles.css",
                        "~/Content/css/icons-sprite.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}