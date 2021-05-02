using System.Web;
using System.Web.Optimization;

namespace ServicePro
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/toastr/js").Include(
                       "~/UI/plugins/toastr/Scripts/toastr.js*",
                       "~/UI/plugins/toastr/Scripts/toastrImp.js"));
            bundles.Add(new StyleBundle("~/bundles/toastr/css").Include("~/UI/plugins/toastr/Content/toastr.css"));
            bundles.Add(new StyleBundle("~/bundles/bootstrap").Include("~/UI/bootstrap/css/bootstrap.min.css"));
            BundleTable.EnableOptimizations = false;
        }
    }
}
