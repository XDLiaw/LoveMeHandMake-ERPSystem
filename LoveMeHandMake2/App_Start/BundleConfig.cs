using System.Web;
using System.Web.Optimization;

namespace LoveMeHandMake2
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Create bundel for jQueryUI  
            //js  
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                      "~/Scripts/jquery.datetimepicker.full.js"));
            bundles.Add(new ScriptBundle("~/bundles/calendarBox").Include(
                      "~/Scripts/calendarBox.js"));
            //css  
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
                   "~/Content/jquery.datetimepicker.css"));
            bundles.Add(new StyleBundle("~/Content/fileBrowseBtn").Include(
                   "~/Content/btn.file.browse.css"));  
        }
    }
}
