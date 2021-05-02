using ServicePro.BAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ServicePro
{
    public class WorkOrderMasterController : Controller
    {
        //
        // GET: /WorkOrderMaster/
        public ActionResult Index(int? id)
        {
            WorkOrderMaster workOrderMaster = null;
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                workOrderMaster = WorkOrderMaster.Get(Convert.ToInt32("0" + id));

                if (workOrderMaster == null)
                {
                    workOrderMaster = WorkOrderMaster.GetNew();
                }

                Global.PageName = "WorkOrder Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View(workOrderMaster);
        }

        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            WorkOrderMaster workOrderMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                workOrderMaster = Build(jsonBuild);
            }
            try
            {
                if (workOrderMaster != null)
                {
                    if (duplicate == false)
                    {
                        workOrderMaster = WorkOrderMaster.Save(workOrderMaster);
                        if (workOrderMaster.ReturnValue != 2)
                        {
                            workOrderMaster.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        workOrderMaster.ReturnValue = 1;
                    }
                }
                else
                {
                    workOrderMaster = new WorkOrderMaster();
                    workOrderMaster.ReturnValue = 4;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                workOrderMaster.ReturnValue = 4;
            }
            return Json(workOrderMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            WorkOrderMaster workOrderMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                workOrderMaster = Build(jsonBuild);
            }
            try
            {
                if (workOrderMaster.WorkOrderMaster_ID > 0)
                {
                    delete = WorkOrderMaster.Delete(workOrderMaster.WorkOrderMaster_ID);
                    if (delete)
                    {
                        workOrderMaster.ReturnValue = 1;
                    }
                    else
                    {
                        workOrderMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                workOrderMaster.ReturnValue = 3;
            }
            return Json(workOrderMaster, JsonRequestBehavior.AllowGet);
        }
        public WorkOrderMaster Build(string jsonBuild)
        {
            WorkOrderMaster workOrderMaster = null;
            BuildWorkOrderMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildWorkOrderMaster>(jsonBuild);
                workOrderMaster = WorkOrderMaster.Get(build.WorkOrderMaster_ID);
                if (workOrderMaster == null)
                {
                    workOrderMaster = WorkOrderMaster.GetNew();
                    workOrderMaster.Created_UserID = Global.UserId;
                    workOrderMaster.ServiceItemDetail = ServiceItemDetail.Get(build.ServiceItemDetail_ID);
                }
                else
                {
                    workOrderMaster.Modified_UserID = Global.UserId;
                    workOrderMaster.Modified_DateTime = DateTime.Now;
                }
                workOrderMaster.ServiceItemDetail.StatusType = build.StatusType;
                workOrderMaster.IsActive = build.IsActive;
                workOrderMaster.ServiceItemDetail_ID = build.ServiceItemDetail_ID;
                workOrderMaster.WorkCodeTemplate = build.WorkCodeTemplate;
                workOrderMaster.EmployeeMaster_ID = Global.CurrentEmployeeId;
                workOrderMaster.ServiceStartDate = build.ServiceStartDate;
                workOrderMaster.ServiceEndDate = build.ServiceEndDate;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return workOrderMaster;
        }

        public class BuildWorkOrderMaster
        {
            public int WorkOrderMaster_ID { get; set; }
            public int ServiceItemDetail_ID { get; set; }
            public int WorkCodeTemplate { get; set; }
            public int EmployeeMaster_ID { get; set; }
            public DateTime ServiceStartDate { get; set; }
            public DateTime? ServiceEndDate { get; set; }
            public int StatusType { get; set; }
            public int IsActive { get; set; }
            public DateTime? Modified_DateTime { get; set; }
        }


        [HttpGet]
        public JsonResult GetWorkOrderMasterGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = null;
            List<string[]> LikeColumns = new List<string[]>();
            try
            {
                LikeColumns.Add(new string[] { "ServiceItemMaster.Brand" });
                LikeColumns.Add(new string[] { "ServiceItemMaster.Model" });
                LikeColumns.Add(new string[] { "ServiceItemDetail.Comments" });
                LikeColumns.Add(new string[] { "EmployeeMaster.EmployeeName" });
                LikeColumns.Add(new string[] { "TypeDetailMaster.TypeName" });
                LikeColumns.Add(new string[] { "WorkOrderMaster.ServiceStartDate" });
                LikeColumns.Add(new string[] { "WorkOrderMaster.ServiceEndDate" });

                records = WorkOrderMaster.GetGrid(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = WorkOrderMaster.GetGridCount(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetServiceItemMasterGrid(int? page, int? limit, string sortBy, string direction, string searchString = null, int ServiceItemMaster_ID = 0, int ServiceItemDetail_ID = 0)
        {
            int total = 0;
            IList records = null;
            List<string[]> where = new List<string[]>();
            try
            {
                if (ServiceItemMaster_ID > 0)
                {
                    where.Add(new string[] { "ServiceItemDetail.ServiceItemMaster_ID", ServiceItemMaster_ID.ToString() });
                }

                if (ServiceItemDetail_ID > 0)
                {
                    where.Add(new string[] { "ServiceItemDetail.ServiceItemDetail_ID", ServiceItemDetail_ID.ToString() });
                }

                records = ServiceItemDetail.GetServiceItemDetailForWorkOrder(where, null, null, null, null, 0, 0, null);
                total = records.Count;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkCodeTemplate(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("WorkCodeTemplate", true, Global.CompanyId, null);
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
        public JsonResult GetServiceItem(int ServiceItemMaster_ID)
        {
            DataTable dt = null;
            IList<ComboBox> result = new List<ComboBox>();
            string status = null;
            List<string[]> WhereList = new List<string[]>();
            try
            {
                if (ServiceItemMaster_ID == 0)
                {
                    status = "New";
                }
                else
                {
                    WhereList.Add(new string[] { "ServiceItemMaster.ServiceItemMaster_ID", ServiceItemMaster_ID.ToString() });
                }
                
                dt = ServiceItemMaster.GetServiceItemMasterCboByStatus(status, WhereList, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["ServiceItemMaster_ID"]),
                    text = row["ServiceItem"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select ServiceItem -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatusType()
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("StatusType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                //result.Add(new ComboBox { id = 0, text = " -- Select StatusType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
	}
}