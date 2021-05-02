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
    public class CustomerMasterController : Controller
    {
        //
        // GET: /CustomerMaster/
        public ActionResult Index(int? id)
        {
            CustomerMaster customerMaster = null;
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                customerMaster = CustomerMaster.Get(Convert.ToInt32("0" + id));

                if (customerMaster == null)
                {
                    customerMaster = CustomerMaster.GetNew();
                    customerMaster.CustomerCode = Common.GetMaxValueByColumnName("ServicePro.CustomerMaster", "CustomerCode");
                }
                Global.PageName = "Customer Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View(customerMaster);
        }

        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            CustomerMaster customerMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                customerMaster = Build(jsonBuild);
            }
            try
            {
                if (customerMaster != null)
                {
                    //List<string[]> list = new List<string[]>();
                    //list.Add(new string[] { "EmployeeCode", employeeMaster.EmployeeCode.ToString() });
                    //duplicate = Common.DuplicateCheck("Master.EmployeeMaster", "EmployeeMaster_ID", employeeMaster.EmployeeMaster_ID, list);
                    if (duplicate == false)
                    {
                        customerMaster = CustomerMaster.Save(customerMaster);
                        if (customerMaster.ReturnValue != 2)
                        {
                            customerMaster.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        customerMaster.ReturnValue = 1;
                    }
                }
                else
                {
                    customerMaster = new CustomerMaster();
                    customerMaster.ReturnValue = 4;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                customerMaster.ReturnValue = 4;
            }
            return Json(customerMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            CustomerMaster customerMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                customerMaster = Build(jsonBuild);
            }
            try
            {
                if (customerMaster.CustomerMaster_ID > 0)
                {
                    delete = CustomerMaster.Delete(customerMaster.CustomerMaster_ID);
                    if (delete)
                    {
                        customerMaster.ReturnValue = 1;
                    }
                    else
                    {
                        customerMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                customerMaster.ReturnValue = 3;
            }
            return Json(customerMaster, JsonRequestBehavior.AllowGet);
        }
        public CustomerMaster Build(string jsonBuild)
        {
            CustomerMaster customerMaster = null;
            BuildCustomerMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildCustomerMaster>(jsonBuild);
                customerMaster = CustomerMaster.Get(build.CustomerMaster_ID);
                if (customerMaster == null)
                {
                    customerMaster = CustomerMaster.GetNew();
                    customerMaster.EmployeeMaster_ID = Global.CurrentEmployeeId;
                    customerMaster.Created_UserID = Global.UserName;
                }
                else
                {
                    customerMaster.IsActive = build.IsActive;
                    customerMaster.Modified_UserID = Global.UserName;
                    customerMaster.Modified_DateTime = DateTime.Now;
                }
                customerMaster.CustomerCodeTemplate = build.CustomerCodeTemplate;
                customerMaster.CustomerCode = build.CustomerCode;
                customerMaster.CustomerName = build.CustomerName;
                customerMaster.Gender = build.Gender;
                customerMaster.DOB = Convert.ToDateTime(build.DOB);
                customerMaster.AddCol_1 = build.AddCol_1;
                customerMaster.AddCol_2 = build.AddCol_2;
                customerMaster.AddCol_3 = build.AddCol_3;
                customerMaster.AddCol_4 = build.AddCol_4;
                customerMaster.AddCol_5 = build.AddCol_5;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return customerMaster;
        }

        public class BuildCustomerMaster
        {
            public int CustomerMaster_ID { get; set; }
            public int CustomerCodeTemplate { get; set; }
            public int CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public DateTime DOB { get; set; }
            public string Gender { get; set; }
            public int EmployeeMaster_ID { get; set; }
            public int IsActive { get; set; }
            public string Created_UserID { get; set; }
            public string Modified_UserID { get; set; }
            public DateTime? Modified_DateTime { get; set; }
            public string AddCol_1 { get; set; }
            public string AddCol_2 { get; set; }
            public string AddCol_3 { get; set; }
            public string AddCol_4 { get; set; }
            public string AddCol_5 { get; set; }
        }

        public JsonResult GetCustomerCodeTemplate(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("CustomerCodeTemplate", true, Global.CompanyId, null);
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

        [HttpGet]
        public JsonResult GetCustomerMasterGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = new List<CustomerMaster>();
            try
            {
                List<string[]> SearchColumns = new List<string[]>();
                SearchColumns.Add(new string[] { "TypeDetailMaster.TypeName" });
                SearchColumns.Add(new string[] { "CustomerMaster.CustomerCode" });
                SearchColumns.Add(new string[] { "CustomerMaster.Gender" });
                SearchColumns.Add(new string[] { "CustomerMaster.CustomerName" });
                SearchColumns.Add(new string[] { "CustomerMaster.IsActive" });

                records = CustomerMaster.GetGrid(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = CustomerMaster.GetGridCount(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
	}
}