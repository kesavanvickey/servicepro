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
    public class ItemReceivedHandlerController : Controller
    {
        //
        // GET: /ItemReceivedHandler/
        public ActionResult Index(int? id)
        {
            ItemReceivedHandler itemReceivedHandler = null;
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                itemReceivedHandler = ItemReceivedHandler.Get(Convert.ToInt32("0" + id));

                if (itemReceivedHandler == null)
                {
                    itemReceivedHandler = ItemReceivedHandler.GetNew();
                    itemReceivedHandler.CustomerMaster_ID = 0;
                }
                Global.PageName = "ItemReceived Handler";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View(itemReceivedHandler);
        }
        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            ItemReceivedHandler itemReceivedHandler = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                itemReceivedHandler = Build(jsonBuild);
            }
            try
            {
                if (itemReceivedHandler != null)
                {
                    if (duplicate == false)
                    {
                        itemReceivedHandler = ItemReceivedHandler.Save(itemReceivedHandler);
                        if (itemReceivedHandler.ReturnValue != 2)
                        {
                            itemReceivedHandler.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        itemReceivedHandler.ReturnValue = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                itemReceivedHandler.ReturnValue = 4;
            }
            return Json(itemReceivedHandler, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string Id)
        {
            ItemReceivedHandler obj = ItemReceivedHandler.GetNew();
            bool delete = false;
            try
            {
                if (Convert.ToInt32("0"+ Id) > 0)
                {
                    delete = ItemReceivedHandler.Delete(Convert.ToInt32("0" + Id));
                    if (delete)
                    {
                        obj.ReturnValue = 1;
                    }
                    else
                    {
                        obj.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                obj.ReturnValue = 3;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ItemReceivedHandler Build(string jsonBuild)
        {
            ItemReceivedHandler itemReceivedHandler = null;
            BuildItemReceivedHandler build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildItemReceivedHandler>(jsonBuild);
                itemReceivedHandler = ItemReceivedHandler.Get(build.ItemReceivedHandler_ID);
                if (itemReceivedHandler == null)
                {
                    itemReceivedHandler = ItemReceivedHandler.GetNew();
                    itemReceivedHandler.Created_UserID = Global.UserId;
                }
                else
                {
                    itemReceivedHandler.Modified_UserID = Global.UserId;
                    itemReceivedHandler.Modified_DateTime = DateTime.Now;
                }
                itemReceivedHandler.ServiceItemMaster_ID = build.ServiceItemMaster_ID;
                itemReceivedHandler.CustomerMaster_ID = build.CustomerMaster_ID;
                itemReceivedHandler.EmployeeMaster_ID = build.EmployeeMaster_ID;
                itemReceivedHandler.Comments = build.Comments;
                itemReceivedHandler.ReceivedDateTime = build.ReceivedDateTime;
                itemReceivedHandler.IsActive = build.IsActive;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return itemReceivedHandler;
        }
        public class BuildItemReceivedHandler
        {
            public int ItemReceivedHandler_ID { get; set; }
            public int ServiceItemMaster_ID { get; set; }
            public int EmployeeMaster_ID { get; set; }
            public int? CustomerMaster_ID { get; set; }
            public string Comments { get; set; }
            public DateTime ReceivedDateTime { get; set; }
            public int IsActive { get; set; }
        }
        public JsonResult GetCustomer()
        {
            DataTable dt = null;
            IList<ComboBox> result = new List<ComboBox>();
            try
            {
                dt = Common.GetCodeTemplateForComboBox("ServicePro.CustomerMaster", "CustomerMaster_ID", "CustomerCodeTemplate", "CustomerCode", "CustomerName", "CustomerCode");
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["CustomerMaster_ID"]),
                    text = row["CustomerCode"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select Customer -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployee()
        {
            DataTable dt = null;
            IList<ComboBox> result = new List<ComboBox>();
            try
            {
                dt = Common.GetCodeTemplateForComboBox("Master.EmployeeMaster", "EmployeeMaster_ID", "EmployeeCodeTemplate", "EmployeeCode", "EmployeeName", "EmployeeCode");
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["EmployeeMaster_ID"]),
                    text = row["EmployeeCode"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select Employee -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceItem()
        {
            DataTable dt = null;
            IList<ComboBox> result = new List<ComboBox>();
            try
            {
                dt = ServiceItemMaster.GetServiceItemMasterCboByStatus(null, null, null);
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

        [HttpGet]
        public JsonResult GetItemReceivedHandlerGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = null;
            List<string[]> LikeColumns = new List<string[]>();
            try
            {
                LikeColumns.Add(new string[] { "ServiceItemMaster.Brand" });
                LikeColumns.Add(new string[] { "ServiceItemMaster.Model" });
                LikeColumns.Add(new string[] { "ItemReceivedHandler.Comments" });
                LikeColumns.Add(new string[] { "EmployeeMaster.EmployeeName" });
                LikeColumns.Add(new string[] { "CustomerMaster.CustomerName" });
                LikeColumns.Add(new string[] { "ItemReceivedHandler.ReceivedDateTime" });

                records = ItemReceivedHandler.GetGrid(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = ItemReceivedHandler.GetGridCount(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckItemReceived(string ServiceItem)
        {
            int val = 0;
            try
            {
                List<string[]> where = new List<string[]>();
                where.Add(new string[] { "ServiceItemMaster_ID", ServiceItem.ToString() });
                where.Add(new string[] { "IsActive", "1" });
                val = Convert.ToInt32("0" + Common.GetSingleColumnValue("ServicePro.ItemReceivedHandler", "ItemReceivedHandler_ID", where, null));
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(val, JsonRequestBehavior.AllowGet);
        }
	}
}