using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicePro.BAL;
using System.Web.Script.Serialization;
using System.Collections;
using System.Timers;

namespace ServicePro.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            IList companyList;            
            try
            {
                companyList = Common.FetchList("Master.CompanyMaster", null);

                //Checking Company If Registered
                if(companyList.Count == 0)
                {
                    return RedirectToAction("Index", "Master");
                }
                else
                {
                    foreach (Hashtable tb in companyList)
                    {
                        string key = tb["ActivationMaster_Key"].ToString();
                        if(key == "")
                        {
                            return RedirectToAction("Index", "Master");
                        }
                        else
                        {
                            if (!CommonUtil.CheckProductKey(CommonUtil.Decrypt(key)))
                            {
                                return RedirectToAction("Index", "Master");
                            }
                        }
                    }
                }

                //Cheking User Session Available
                if (Global.UserId != null && Global.Password != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Global.PageName = "Login";
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View("Index", "_Master");
        }
        public JsonResult Validate(string jsonBuild)
        {
            BuildGlobal global = null;
            try
            {
                if(jsonBuild != null)
                {
                    global = Build(jsonBuild);
                    if(global.UserId != null && global.Password != null)
                    {
                        IList Login = MasterControl.LoginValidate(global.UserId, global.Password, global.UserType);
                        if(Login.Count > 0)
                        {
                            Hashtable tb = (Hashtable)Login[0];
                            string logo = @"\UI\dist\img\bg-test.png";
                            if (tb["CompanyLogo"] != null)
                            {
                                logo = tb["CompanyLogo"].ToString();
                            }
                            Global.CompanyId = tb.ContainsKey("CompanyMaster_ID") == true ? Convert.ToInt32("0" + tb["CompanyMaster_ID"]) : 0;
                            Global.CompanyName = tb.ContainsKey("CompanyName") == true ? tb["CompanyName"].ToString() : null;
                            Global.Password = tb.ContainsKey("Password") == true ? tb["Password"].ToString() : null;
                            Global.UserId = tb.ContainsKey("UserName") == true ? tb["UserName"].ToString() : null;
                            Global.UserName = tb.ContainsKey("UserName") == true ? tb["UserName"].ToString() : null;
                            Global.UserMasterId = tb.ContainsKey("UserMaster_ID") == true ? (int)tb["UserMaster_ID"] : 0;
                            Global.CompanyLogo = logo;

                            Global.ReportSignature = tb["ReportSignature"] == null == true ? "" : tb["ReportSignature"].ToString();
                            Global.ReportBottom = tb["ReportBottom"] == null == true ? "" : tb["ReportBottom"].ToString();

                            if (tb.ContainsKey("EmployeeMaster_ID") == true)
                            {
                                Global.CurrentEmployeeId = tb.ContainsKey("EmployeeMaster_ID") == true ? Convert.ToInt32("0" + tb["EmployeeMaster_ID"]) : 0;
                                Global.UserNameForScreen = tb.ContainsKey("UserName") == true && tb.ContainsKey("EmployeeName") == true ? tb["UserName"].ToString() + " - " + tb["EmployeeName"].ToString() : null;
                                if (tb["RollTypeName"].ToString() == "Admin" || tb["RollTypeName"].ToString() == "admin")
                                {
                                    //Admin
                                    Global.UserType = 1;
                                }
                                else
                                {
                                    //Employee
                                    Global.UserType = 2;
                                }
                            }
                            else
                            {
                                //Admin
                                Global.UserNameForScreen = tb.ContainsKey("UserName") == true ? tb["UserName"].ToString() : null;
                                Global.UserType = 1;
                            }

                            global.ReturnValue = 2;
                        }
                        else
                        {
                            global.ReturnValue = 1;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
                global.ReturnValue = 3;
            }
            return Json(global, JsonRequestBehavior.AllowGet);
        }
        public BuildGlobal Build(string jsonBuild)
        {
            BuildGlobal build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildGlobal>(jsonBuild);
                build.Password = CommonUtil.Encrypt(build.Password);
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return build;
        }
        public class BuildGlobal
        {
            public string UserId { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
            public int UserType { get; set; }
            public int ReturnValue { get; set; }
        }
	}
}