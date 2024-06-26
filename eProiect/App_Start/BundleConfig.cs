﻿using eProiect.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Web;
using System.Web.Optimization;
using System.Web.UI.WebControls;

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



            bundles.Add(new Bundle("~/bundles/pcoded/js").Include(
                "~/Vendor/assets/js/pcoded.min.js"));

            bundles.Add(new Bundle("~/bundles/demo-12/js").Include(
                "~/Vendor/assets/js/demo-12.js"));

            bundles.Add(new Bundle("~/bundles/mCustomScrollbar/js").Include(
                "~/Vendor/assets/js/jquery.mCustomScrollbar.concat.min.js"));

            bundles.Add(new Bundle("~/bundles/script/js").Include(
                "~/Vendor/assets/js/script.min.js"));

            //------------------------FOR_LOGIN_PAGE_STYLE----------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/bundles/flag-icon/css").Include(
                "~/Vendor/assets/pages/flag-icon/flag-icon.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/bootstrap.min/css").Include(
                "~/Vendor/bower_components/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransform()));

            //------------------------FOR_LOGIN_PAGE_SCRIPTS----------------------------------------------------------------------
            bundles.Add(new Bundle("~/bundles/css-scrollbars/js").Include(
            "~/Vendor/bower_components/modernizr/js/css-scrollbars.js"));


            //------------------------SCHEDULE TABLE STYLE
            bundles.Add(new StyleBundle("~/bundles/schedule-table/css").Include(
               "~/Content/themes/schedule-table/schedule-style.css", new CssRewriteUrlTransform()));

            bundles.Add(new Bundle("~/bundles/schedule-table").Include(
            "~/Content/themes/schedule-table/schedule-table.js"));

            //----------------------LOGPAGE STYLES
            bundles.Add(new StyleBundle("~/bundles/logpage-styles/css").Include(
            "~/Vendor/assets/css/logpage-styles.css", new CssRewriteUrlTransform()));


            //---------------------NAVBAR-USER-INFO
            bundles.Add(new StyleBundle("~/bundles/navbar-user-styles/css").Include(
            "~/Vendor/assets/css/navbar-profile-style.css", new CssRewriteUrlTransform()));


            //--------------------LESSONADD-SCRIPTS
            bundles.Add(new Bundle("~/bundles/addlesson-selector/js").Include(
            "~/Vendor/assets/js/update-lessonadd-selector.js"));



            // -----------------------Edit-table----------------------------------------------

            bundles.Add(new Bundle("~/bundles/edittablesU/js").Include(
             "~/Vendor/assets/pages/edit-table/jquerytableditUser.js"));

            bundles.Add(new Bundle("~/bundles/edittablesD/js").Include(
            "~/Vendor/assets/pages/edit-table/jquerytableditDiscipline.js"));

            bundles.Add(new Bundle("~/bundles/edittablesAG/js").Include(
            "~/Vendor/assets/pages/edit-table/jquerytableditAcademicGroup.js"));

            bundles.Add(new Bundle("~/bundles/edittablesCR/js").Include(
            "~/Vendor/assets/pages/edit-table/jquerytableditClassRoom.js"));






            bundles.Add(new StyleBundle("~/bundles/select2").Include(
            "~/Vendor/select2/css/select2-bootstrap-5-theme.css", new CssRewriteUrlTransform()));




            bundles.Add(new Bundle("~/bundles/editableAG/js").Include(
            "~/Vendor/assets/pages/edit-table/editableAG.js"));

            bundles.Add(new Bundle("~/bundles/editableCR/js").Include(
           "~/Vendor/assets/pages/edit-table/editableCR.js"));

            bundles.Add(new Bundle("~/bundles/editableD/js").Include(
           "~/Vendor/assets/pages/edit-table/editableD.js"));

            bundles.Add(new Bundle("~/bundles/editableU/js").Include(
           "~/Vendor/assets/pages/edit-table/editableU.js"));


            // -----------------------Add-Pages----------------------------------------------
            bundles.Add(new StyleBundle("~/bundles/j-pro-modern/css").Include(
            "~/Vendor/assets/pages/j-pro/css/j-pro-modern.css", new CssRewriteUrlTransform()));

            bundles.Add(new Bundle("~/bundles/CheckValues/js").Include(
            "~/Content/themes/Admin/CheckValues.js"));
            bundles.Add(new Bundle("~/bundles/SendPagesNewValues/js").Include(
           "~/Content/themes/Admin/SendPagesNewValues.js"));


            bundles.Add(new StyleBundle("~/bundles/guest-layout-styles/css").Include(
            "~/Vendor/assets/css/guest-layout-styles.css", new CssRewriteUrlTransform()));

            //---------------------GUEST-PAGE-SCRIPTS
            bundles.Add(new Bundle("~/bundles/guest-page/js").Include(
            "~/Vendor/assets/js/guest-page.js"));




            //-----------------------UserDiscipline-----------------------------------

            bundles.Add(new StyleBundle("~/bundles/UserDisciplineForm/css").Include(
               "~/Vendor/assets/pages/j-pro/css/j-forms.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/UserDisciplineEditStyle/css").Include(
               "~/Content/themes/Admin/UserDiscipline/UserDisciplineEditStyle.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/select2.min/css").Include(
               "~/Vendor/bower_components/select2/css/select2.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new Bundle("~/bundles/js/jquery.ui.min.js").Include(
            "~/Vendor/assets/pages/j-pro/js/jquery.ui.min.js"));

            bundles.Add(new Bundle("~/bundles/js/jquery-cloneya.min.js").Include(
            "~/Vendor/assets/pages/j-pro/js/jquery-cloneya.min.js"));

            bundles.Add(new Bundle("~/bundles/js/jquery.maskedinput.min.js").Include(
            "~/Vendor/assets/pages/j-pro/js/jquery.maskedinput.min.js"));

            bundles.Add(new Bundle("~/bundles/js/custom/cloned-form.js").Include(
            "~/Vendor/assets/pages/j-pro/js/custom/cloned-form.js"));

            bundles.Add(new Bundle("~/bundles/select2/js/select2.full.min.js").Include(
            "~/Vendor/bower_components/select2/js/select2.full.min.js"));

            bundles.Add(new Bundle("~/bundles/pages/advance-elements/select2-custom.js").Include(
            "~/Vendor/assets/pages/advance-elements/select2-custom.js"));

            bundles.Add(new Bundle("~/bundles/pages/UserDisciplineJavaScript.js").Include(
            "~/Content/themes/Admin/UserDiscipline/UserDisciplineJavaScript.js"));

            //------------------PENDING-CLASSES-------

            bundles.Add(new Bundle("~/bundles/pending-classes/js").Include(
            "~/Vendor/assets/js/pending-classes.js"));


            //-------------ADMIN-VIEW-CLASSESS
            bundles.Add(new Bundle("~/bundles/admin-teacher-view/js").Include(
            "~/Vendor/assets/js/admin-teacher-view.js"));

            //-----------------User-Profile-----------------------------
            bundles.Add(new Bundle("~/bundles/UserProfile/js").Include(
          "~/Content/themes/Admin/UserProfile.js"));

        }
    }

}