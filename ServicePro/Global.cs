using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro
{
    public class Global
    {
        public static string UserId
        {
            get { return (String)HttpContext.Current.Session["UserId"]; }
            set { HttpContext.Current.Session["UserId"] = value; }
        }
        public static string Password
        {
            get { return (String)HttpContext.Current.Session["Password"]; }
            set { HttpContext.Current.Session["Password"] = value; }
        }
        public static string UserName
        {
            get { return (String)HttpContext.Current.Session["UserName"]; }
            set { HttpContext.Current.Session["UserName"] = value; }
        }
        public static int UserType
        {
            get { return (int)HttpContext.Current.Session["UserType"]; }
            set { HttpContext.Current.Session["UserType"] = value; }
        }
        public static string PageName
        {
            get { return (string)HttpContext.Current.Session["PageName"]; }
            set { HttpContext.Current.Session["PageName"] = value; }
        }
        public static string CompanyName
        {
            get { return (string)HttpContext.Current.Session["CompanyName"]; }
            set { HttpContext.Current.Session["CompanyName"] = value; }
        }
        public static int CompanyId
        {
            get { return (int)HttpContext.Current.Session["CompanyId"]; }
            set { HttpContext.Current.Session["CompanyId"] = value; }
        }
        public static string RollTypeName
        {
            get { return (string)HttpContext.Current.Session["RollTypeName"]; }
            set { HttpContext.Current.Session["RollTypeName"] = value; }
        }
        public static string UserNameForScreen
        {
            get { return (string)HttpContext.Current.Session["UserNameForScreen"]; }
            set { HttpContext.Current.Session["UserNameForScreen"] = value; }
        }
        public static bool ShowPageName
        {
            get { return HttpContext.Current.Session["ShowPageName"] == null ? false : (bool)HttpContext.Current.Session["ShowPageName"]; }
            set { HttpContext.Current.Session["ShowPageName"] = value; }
        }
        public static bool ShowToolBar
        {
            get { return HttpContext.Current.Session["ShowToolBar"] == null ? false : (bool)HttpContext.Current.Session["ShowToolBar"]; }
            set { HttpContext.Current.Session["ShowToolBar"] = value; }
        }
        public static int CurrentEmployeeId
        {
            get { return (int)HttpContext.Current.Session["CurrentEmployeeId"]; }
            set { HttpContext.Current.Session["CurrentEmployeeId"] = value; }
        }
        public static int UserMasterId
        {
            get { return (int)HttpContext.Current.Session["UserMasterId"]; }
            set { HttpContext.Current.Session["UserMasterId"] = value; }
        }
        public static string CompanyLogo
        {
            get { return (string)HttpContext.Current.Session["CompanyLogo"]; }
            set { HttpContext.Current.Session["CompanyLogo"] = value; }
        }
        public static string ReportSignature
        {
            get { return (string)HttpContext.Current.Session["ReportSignature"]; }
            set { HttpContext.Current.Session["ReportSignature"] = value; }
        }
        public static string ReportBottom
        {
            get { return (string)HttpContext.Current.Session["ReportBottom"]; }
            set { HttpContext.Current.Session["ReportBottom"] = value; }
        }
    }
}