using ServicePro.BAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ServicePro.Controllers
{
    public class UserMasterController : Controller
    {
        //
        // GET: /UserMaster/
        public ActionResult Index(int? id)
        {
            UserMaster userMater = null;
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                userMater = UserMaster.Get(Convert.ToInt32("0" + id));

                if (userMater == null)
                {
                    userMater = UserMaster.GetNew();
                }
                Global.PageName = "User Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View(userMater);
        }

        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            UserMaster userMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                userMaster = Build(jsonBuild);
            }
            try
            {
                if (userMaster != null)
                {
                    List<string[]> list = new List<string[]>();
                    list.Add(new string[] { "UserName", userMaster.UserName.ToString() });
                    list.Add(new string[] { "EmployeeMaster_ID", userMaster.EmployeeMaster_ID.ToString() });
                    duplicate = Common.DuplicateCheck("ServicePro.UserMaster", "UserMaster_ID", userMaster.UserMaster_ID, list);
                    if (duplicate == false)
                    {
                        userMaster = UserMaster.Save(userMaster);
                        if (userMaster.ReturnValue != 2)
                        {
                            userMaster.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        userMaster.ReturnValue = 1;
                    }
                }
                else
                {
                    userMaster = new UserMaster();
                    userMaster.ReturnValue = 4;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                userMaster.ReturnValue = 4;
            }
            return Json(userMaster, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangePassword(string oldPasssword, string newPassword)
        {
            string returnValue = "Please Refresh and try again";
            try
            {
                List<string[]> whereList = new List<string[]>();
                whereList.Add(new string[] { "UserMaster_ID", Global.UserMasterId.ToString() });
                whereList.Add(new string[] { "Password", CommonUtil.Encrypt(oldPasssword) });
                
                IList list = Common.FetchList("ServicePro.UserMaster", whereList, null);
                if(list != null)
                {
                    if(list.Count > 0)
                    {
                        Hashtable Id = (Hashtable)list[0];
                        UserMaster obj = UserMaster.Get((int)Id["UserMaster_ID"]);
                        obj.Password = CommonUtil.Encrypt(newPassword);
                        obj = UserMaster.Save(obj);
                        if(obj.ReturnValue == 2)
                        {
                            returnValue = "Successfully Updated";
                        }
                        else
                        {
                            returnValue = "Please Refresh & Try Again";
                        }
                    }
                    else
                    {
                        returnValue = "Old Password is Not Valid";
                    }
                }
                else
                {
                    returnValue = "Old Password is Not Valid";
                }
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
                returnValue = "Please Refresh & Try Again";
            }
            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            UserMaster userMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                userMaster = Build(jsonBuild);
            }
            try
            {
                if (userMaster.UserMaster_ID > 0)
                {
                    delete = EmployeeMaster.Delete(userMaster.UserMaster_ID);
                    if (delete)
                    {
                        userMaster.ReturnValue = 1;
                    }
                    else
                    {
                        userMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                userMaster.ReturnValue = 3;
            }
            return Json(userMaster, JsonRequestBehavior.AllowGet);
        }
        public UserMaster Build(string jsonBuild)
        {
            UserMaster userMaster = null;
            BuildUserMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildUserMaster>(jsonBuild);
                userMaster = UserMaster.Get(build.UserMaster_ID);
                if (userMaster == null)
                {
                    userMaster = UserMaster.GetNew();
                }
                else
                {
                    userMaster.IsActive = build.IsActive;
                    userMaster.Modified_DateTime = DateTime.Now;
                }
                userMaster.EmployeeMaster_ID = build.EmployeeMaster_ID;
                userMaster.UserName = build.UserName;
                userMaster.Password = CommonUtil.Encrypt(build.Password);
                userMaster.RollType = build.RollType;
                userMaster.AddCol_1 = build.AddCol_1;
                userMaster.AddCol_2 = build.AddCol_2;
                userMaster.AddCol_3 = build.AddCol_3;
                userMaster.AddCol_4 = build.AddCol_4;
                userMaster.AddCol_5 = build.AddCol_5;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return userMaster;
        }

        public class BuildUserMaster
        {
            public int UserMaster_ID { get; set; }
            public int EmployeeMaster_ID { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int RollType { get; set; }
            public string AddCol_1 { get; set; }
            public string AddCol_2 { get; set; }
            public string AddCol_3 { get; set; }
            public string AddCol_4 { get; set; }
            public string AddCol_5 { get; set; }
            public int IsActive { get; set; }
            public DateTime? Modified_DateTime { get; set; }
        }

        public JsonResult GetEmployee(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.FetchTable("Master.EmployeeMaster", null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["EmployeeMaster_ID"]),
                    text = row["EmployeeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select Employee -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRollType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("RollType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select RollType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserMasterGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = new List<UserMaster>();
            try
            {
                List<string[]> SearchColumns = new List<string[]>();
                SearchColumns.Add(new string[] { "DetailEmployeeCodeTemplate.TypeName" });
                SearchColumns.Add(new string[] { "EmployeeMaster.EmployeeCode" });
                SearchColumns.Add(new string[] { "EmployeeMaster.EmployeeName" });
                SearchColumns.Add(new string[] { "UserMaster.UserName" });
                SearchColumns.Add(new string[] { "DetailRollType.TypeName" });

                records = UserMaster.GetGrid(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = UserMaster.GetGridCount(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
    }
}