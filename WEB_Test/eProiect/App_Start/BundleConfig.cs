using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace eProiect.App_Start
{
     public class BundleConfig
     {
          public static void RegistrerBoundles(BundleCollection bundles)
          {
               bundles.IgnoreList.Clear();
            
            //----------STYLE-BUNDLES----------------------------------------------------
            //Bootstrap 
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
                "~/Vendor/bower_components/bootstrap/css/bootstrap.css", new CssRewriteUrlTransform()));

            //Themify icons
            bundles.Add(new StyleBundle("~/bundles/themify-icons/css1").Include(
                "~/Vendor/assets/icon/themify-icons/themify-icons.css", new CssRewriteUrlTransform()));

            //Ico font
            bundles.Add(new StyleBundle("~/bundles/ico-font/css2").Include(
                "~/Vendor/assets/icon/icofont/css/icofont.css", new CssRewriteUrlTransform()));

            //Menu search css
            bundles.Add(new StyleBundle("~/bundles/menu-search/css3").Include(
                "~/Vendor/assets/pages/menu-search/css/component.css", new CssRewriteUrlTransform()));

            //style css
            bundles.Add(new StyleBundle("~/bundles/style/css4").Include(
                "~/Vendor/assets/css/style.css", new CssRewriteUrlTransform()));
            
            bundles.Add(new StyleBundle("~/bundles/mCustomScrollbar/css5").Include(
                "~/Vendor/assets/css/jquery.mCustomScrollbar.css", new CssRewriteUrlTransform()));


            //----------SCRIPT-BUNDLES----------------------------------------------------
            //Jquery bundles
            bundles.Add(new Bundle("~/bundles/jquery/js").Include(
                "~/Vendor/bower_components/jquery/js/jquery.min.js"));
            
            bundles.Add(new Bundle("~/bundles/jquery-ui/js").Include(
                "~/Vendor/bower_components/jquery-ui/js/jquery-ui.min.js"));

            bundles.Add(new Bundle("~/bundles/popper.js/js").Include(
                "~/Vendor/bower_components/popper.js/js/popper.min.js"));

            bundles.Add(new Bundle("~/bundles/bootstrap/js").Include(
                "~/Vendor/bower_components/bootstrap/js/bootstrap.min.js"));

            //Jquery slimscroll js
            bundles.Add(new Bundle("~/bundles/jquery-slimscroll/js").Include(
                "~/Vendor/bower_components/jquery-slimscroll/js/jquery.slimscroll.js"));

            //Modernizr js
            bundles.Add(new Bundle("~/bundles/modernizr/js").Include(
                "~/Vendor/bower_components/modernizr/js/modernizr.js"));

            //Am chart
            bundles.Add(new Bundle("~/bundles/widget/amcharts").Include(
                "~/Vendor/assets/pages/widget/amchart/amcharts.min.js"));
            
            bundles.Add(new Bundle("~/bundles/widget/serial").Include(
                "~/Vendor/assets/pages/widget/amchart/serial.min.js"));

            //Chart js
            bundles.Add(new Bundle("~/bundles/chart.js/js").Include(
                "~/Vendor/bower_components/chart.js/js/Chart.js"));

            //Todo js
            bundles.Add(new Bundle("~/bundles/pages/todo").Include(
                "~/Vendor/assets/pages/todo/todo.js"));

            //i18next
            bundles.Add(new Bundle("~/bundles/i18next/js").Include(
                "~/Vendor/bower_components/i18next/js/i18next.min.js"));

            bundles.Add(new Bundle("~/bundles/i18next-xhr-backend/js").Include(
                "~/Vendor/bower_components/i18next-xhr-backend/js/i18nextXHRBackend.min.js"));

            bundles.Add(new Bundle("~/bundles/i18next-browser-languagedetector/js").Include(
                "~/Vendor/bower_components/i18next-browser-languagedetector/js/i18nextBrowserLanguageDetector.min.js"));

            bundles.Add(new Bundle("~/bundles/jquery-i18next/js").Include(
                "~/Vendor/bower_components/jquery-i18next/js/jquery-i18next.min.js"));

            //Custom js
            bundles.Add(new Bundle("~/bundles/pages/dashboard").Include(
                "~/Vendor/assets/pages/dashboard/custom-dashboard.min.js"));

            bundles.Add(new Bundle("~/bundles/SmoothScroll/js").Include(
                "~/Vendor/assets/js/SmoothScroll.js"));

            bundles.Add(new Bundle("~/bundles/pcoded/js").Include(
                "~/Vendor/assets/js/pcoded.min.js"));

            bundles.Add(new Bundle("~/bundles/demo-12/js").Include(
                "~/Vendor/assets/js/demo-12.js"));

            bundles.Add(new Bundle("~/bundles/mCustomScrollbar/js").Include(
                "~/Vendor/assets/js/jquery.mCustomScrollbar.concat.min.js"));

            bundles.Add(new Bundle("~/bundles/script/js").Include(
                "~/Vendor/assets/js/script.min.js"));

          }

     }
}