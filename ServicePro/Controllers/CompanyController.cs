using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ServicePro.BAL;
using System.Web.Script.Serialization;
using System.Collections;
using NHibernate;
using System.Data;
using System.IO;

namespace ServicePro.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/
        public ActionResult Index(int? id)
        {
            CompanyMaster companyMaster = null;
            try
            {
                //Is User Session Available
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                //Check Permission for this page
                if (Global.UserType == 2)
                {
                    return RedirectToAction("PermissionDenied", "Master");
                }

                companyMaster = CompanyMaster.Get(Convert.ToInt32("0" + id));
                if(companyMaster == null)
                {
                    companyMaster = CompanyMaster.GetNew();
                }
                Global.PageName = "Company Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View("Index",companyMaster);
        }
        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            CompanyMaster companyMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                companyMaster = Build(jsonBuild);
            }
            try
            {
                if (companyMaster != null)
                {
                    if (CommonUtil.CheckProductKey(companyMaster.ActivationMaster_Key))
                    {
                        companyMaster.ActivationMaster_Key = CommonUtil.Encrypt(companyMaster.ActivationMaster_Key);
                        if (!Common.IsValueExist("Master.MasterControl", "ActivationMaster_Key", companyMaster.ActivationMaster_Key))
                        {
                            companyMaster.ReturnValue = 5;
                        }
                        else
                        {
                            List<string[]> list = new List<string[]>();
                            list.Add(new string[] { "ActivationMaster_Key", companyMaster.ActivationMaster_Key });
                            duplicate = Common.DuplicateCheck("Master.CompanyMaster", "CompanyMaster_ID", companyMaster.CompanyMaster_ID, list);
                            if (duplicate == false)
                            {
                                companyMaster = CompanyMaster.Save(companyMaster);
                                if (companyMaster.ReturnValue != 2)
                                {
                                    companyMaster.ReturnValue = 3;
                                }
                            }
                            else if (duplicate == true)
                            {
                                companyMaster.ReturnValue = 1;
                            }
                        }
                    }
                    else
                    {
                        companyMaster.ReturnValue = 6;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                companyMaster.ReturnValue = 4;
            }
            return Json(companyMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            CompanyMaster companyMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                companyMaster = Build(jsonBuild);
            }
            try
            {
                if (companyMaster.CompanyMaster_ID > 0)
                {
                    delete = CompanyMaster.Delete(companyMaster.CompanyMaster_ID);
                    if (delete)
                    {
                        companyMaster.ReturnValue = 1;
                    }
                    else
                    {
                        companyMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                companyMaster.ReturnValue = 3;
            }
            return Json(companyMaster, JsonRequestBehavior.AllowGet);
        }
        public CompanyMaster Build(string jsonBuild)
        {
            CompanyMaster companyMaster = null;
            BuildCompanyMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildCompanyMaster>(jsonBuild);
                companyMaster = CompanyMaster.Get(build.CompanyMaster_ID);
                if (companyMaster == null)
                {
                    companyMaster = CompanyMaster.GetNew();
                    companyMaster.ActivationMaster_Key = build.ActivationMaster_Key;
                }
                else
                {
                    companyMaster.IsActive = build.IsActive;
                    companyMaster.Modified_DateTime = DateTime.Now;
                }
                companyMaster.CompanyName = build.CompanyName;
                companyMaster.CompanyType = build.CompanyType;
                companyMaster.TinNo = build.TinNo;
                companyMaster.UserName = build.UserName;
                companyMaster.Password = CommonUtil.Encrypt(build.Password);
                companyMaster.Recovery_Mobile = build.Recovery_Mobile;
                companyMaster.Recovery_Email = build.Recovery_Email;
                companyMaster.Recovery_Question = build.Recovery_Question;
                companyMaster.Recovery_Answer = build.Recovery_Answer;
                companyMaster.CompanyLogo = build.CompanyLogo;
                companyMaster.ReportSignature = build.ReportSignature;
                companyMaster.ReportBottom = build.ReportBottom;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return companyMaster;
        }
        public class BuildCompanyMaster
        {
            public int CompanyMaster_ID { get; set; }
            public string CompanyName { get; set; }
            public string CompanyType { get; set; }
            public string CompanyLogo { get; set; }
            public string ReportSignature { get; set; }
            public string ReportBottom { get; set; }
            public string TinNo { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Recovery_Mobile { get; set; }
            public string Recovery_Email { get; set; }
            public string Recovery_Question { get; set; }
            public string Recovery_Answer { get; set; }
            public string ActivationMaster_Key { get; set; }
            public int IsActive { get; set; }
        }
        public JsonResult GetCompanyMaster(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = null;
            try
            {
                List<string[]> LikeColumns = new List<string[]>();
                LikeColumns.Add(new string[] { "CompanyName" });
                LikeColumns.Add(new string[] { "TinNo" });

                records = Common.GetGridList("Master.CompanyMaster", null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = Common.GetGridListCount("Master.CompanyMaster", null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
	}
}