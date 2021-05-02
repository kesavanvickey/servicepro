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
    public class EmployeeMasterController : Controller
    {
        //
        // GET: /EmployeeMaster/
        public ActionResult Index(int? id)
        {
            EmployeeMaster employeeMaster = null;
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                employeeMaster = EmployeeMaster.Get(Convert.ToInt32("0" + id));

                if (employeeMaster == null)
                {
                    employeeMaster = EmployeeMaster.GetNew();
                    employeeMaster.EmployeeCode = Common.GetMaxValueByColumnName("Master.EmployeeMaster", "EmployeeCode");
                }
                Global.PageName = "Employee Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View("Index", employeeMaster);
        }
        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            EmployeeMaster employeeMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                employeeMaster = Build(jsonBuild);
            }
            try
            {
                if (employeeMaster != null)
                {
                    List<string[]> list = new List<string[]>();
                    list.Add(new string[] { "EmployeeCode", employeeMaster.EmployeeCode.ToString() });
                    duplicate = Common.DuplicateCheck("Master.EmployeeMaster", "EmployeeMaster_ID", employeeMaster.EmployeeMaster_ID, list);
                    if (duplicate == false)
                    {
                        employeeMaster = EmployeeMaster.Save(employeeMaster);
                        if (employeeMaster.ReturnValue != 2)
                        {
                            employeeMaster.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        employeeMaster.ReturnValue = 1;
                    }
                }
                else
                {
                    employeeMaster = new EmployeeMaster();
                    employeeMaster.ReturnValue = 4;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                employeeMaster.ReturnValue = 4;
            }
            return Json(employeeMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            EmployeeMaster employeeMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                employeeMaster = Build(jsonBuild);
            }
            try
            {
                if (employeeMaster.EmployeeMaster_ID > 0)
                {
                    delete = EmployeeMaster.Delete(employeeMaster.EmployeeMaster_ID);
                    if (delete)
                    {
                        employeeMaster.ReturnValue = 1;
                    }
                    else
                    {
                        employeeMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                employeeMaster.ReturnValue = 3;
            }
            return Json(employeeMaster, JsonRequestBehavior.AllowGet);
        }
        public EmployeeMaster Build(string jsonBuild)
        {
            EmployeeMaster employeeMaster = null;
            BuildEmployeeMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildEmployeeMaster>(jsonBuild);
                employeeMaster = EmployeeMaster.Get(build.EmployeeMaster_ID);
                if (employeeMaster == null)
                {
                    employeeMaster = EmployeeMaster.GetNew();
                }
                else
                {
                    employeeMaster.IsActive = build.IsActive;
                    employeeMaster.Modified_DateTime = DateTime.Now;
                }
                employeeMaster.EmployeeCodeTemplate = build.EmployeeCodeTemplate;
                employeeMaster.EmployeeCode = build.EmployeeCode;
                employeeMaster.EmployeeName = build.EmployeeName;
                employeeMaster.EmployeeType = build.EmployeeType;
                employeeMaster.Gender = build.Gender;
                employeeMaster.JointDate = Convert.ToDateTime(build.JointDate);
                employeeMaster.DOB = Convert.ToDateTime(build.DOB);
                employeeMaster.CompanyMaster_ID = Global.CompanyId;
                employeeMaster.AddCol_1 = build.AddCol_1;
                employeeMaster.AddCol_2 = build.AddCol_2;
                employeeMaster.AddCol_3 = build.AddCol_3;
                employeeMaster.AddCol_4 = build.AddCol_4;
                employeeMaster.AddCol_5 = build.AddCol_5;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return employeeMaster;
        }

        public class BuildEmployeeMaster
        {
            public int EmployeeMaster_ID { get; set; }
            public int EmployeeCode { get; set; }
            public int EmployeeCodeTemplate { get; set; }
            public string EmployeeName { get; set; }
            public string DOB { get; set; }
            public string Gender { get; set; }
            public int EmployeeType { get; set; }
            public string JointDate { get; set; }
            public int CompanyMaster_ID { get; set; }
            public string AddCol_1 { get; set; }
            public string AddCol_2 { get; set; }
            public string AddCol_3 { get; set; }
            public string AddCol_4 { get; set; }
            public string AddCol_5 { get; set; }
            public int IsActive { get; set; }
            public DateTime? Modified_DateTime { get; set; }
        }

        public JsonResult GetEmployeeCodeType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("EmployeeCodeTemplate", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                //result.Add(new ComboBox { id = 0, text = " -- Select EmployeeCodeType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("EmployeeType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select EmployeeType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeMasterGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = new List<EmployeeMaster>();
            try
            {
                List<string[]> SearchColumns = new List<string[]>();
                SearchColumns.Add(new string[] { "EmployeeMaster.EmployeeName" });
                SearchColumns.Add(new string[] { "EmployeeMaster.EmployeeCode" });
                SearchColumns.Add(new string[] { "EmployeeMaster.Gender" });
                SearchColumns.Add(new string[] { "EmployeeMaster.JointDate" });
                SearchColumns.Add(new string[] { "DetailEmloyeeCodeTemplate.TypeName" });
                SearchColumns.Add(new string[] { "DetailEmployeeType.TypeName" });

                records = EmployeeMaster.GetGrid(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = EmployeeMaster.GetGridCount(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
	}
}