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
    public class ServiceItemMasterController : Controller
    {
        //
        // GET: /ServiceItemMaster/
        public ActionResult Index(int? id)
        {
            ServiceItemMaster serviceItemMaster = null;
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                serviceItemMaster = ServiceItemMaster.Get(Convert.ToInt32("0" + id));

                if (serviceItemMaster == null)
                {
                    serviceItemMaster = ServiceItemMaster.GetNew();
                }
                Global.PageName = "ServiceItem Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View(serviceItemMaster);
        }

        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            ServiceItemMaster serviceItemMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                serviceItemMaster = Build(jsonBuild);
            }
            try
            {
                if (serviceItemMaster != null)
                {
                    if (duplicate == false)
                    {
                        serviceItemMaster = ServiceItemMaster.Save(serviceItemMaster);
                        if (serviceItemMaster.ReturnValue != 2)
                        {
                            serviceItemMaster.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        serviceItemMaster.ReturnValue = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                serviceItemMaster.ReturnValue = 4;
            }
            return Json(serviceItemMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            ServiceItemMaster serviceItemMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                serviceItemMaster = Build(jsonBuild);
            }
            try
            {
                if (serviceItemMaster.ServiceItemMaster_ID > 0)
                {
                    delete = ServiceItemMaster.Delete(serviceItemMaster.ServiceItemMaster_ID);
                    if (delete)
                    {
                        serviceItemMaster.ReturnValue = 1;
                    }
                    else
                    {
                        serviceItemMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                serviceItemMaster.ReturnValue = 3;
            }
            return Json(serviceItemMaster, JsonRequestBehavior.AllowGet);
        }
        public ServiceItemMaster Build(string jsonBuild)
        {
            ServiceItemMaster serviceItemMaster = null;
            BuildServiceItemMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildServiceItemMaster>(jsonBuild);
                serviceItemMaster = ServiceItemMaster.Get(build.ServiceItemMaster_ID);
                if (serviceItemMaster == null)
                {
                    serviceItemMaster = ServiceItemMaster.GetNew();
                    serviceItemMaster.EmployeeMaster_ID = Global.CurrentEmployeeId;
                    serviceItemMaster.Created_UserID = Global.UserId;
                }
                else
                {
                    serviceItemMaster.Modified_UserID = Global.UserId;
                    serviceItemMaster.Modified_DateTime = DateTime.Now;
                }
                serviceItemMaster.ServiceCodeTemplate = build.ServiceCodeTemplate;
                serviceItemMaster.CustomerMaster_ID = build.CustomerMaster_ID;
                serviceItemMaster.ItemOrderDate = build.ItemOrderDate;
                serviceItemMaster.ItemExpectedDeliverDate = build.ItemExpectedDeliverDate;
                serviceItemMaster.Brand = build.Brand;
                serviceItemMaster.Model = build.Model;
                serviceItemMaster.IsActive = build.IsActive;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return serviceItemMaster;
        }
        public class BuildServiceItemMaster
        {
            public int ServiceItemMaster_ID { get; set; }
            public int ServiceCodeTemplate { get; set; }
            public int CustomerMaster_ID { get; set; }
            public int EmployeeMaster_ID { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public DateTime ItemOrderDate { get; set; }
            public DateTime? ItemExpectedDeliverDate { get; set; }
            public int IsActive { get; set; }            
        }
        public JsonResult GetServiceCodeTemplate(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("ServiceCodeTemplate", true, Global.CompanyId, null);
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
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetServiceItemMasterGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = null;
            List<string[]> LikeColumns = new List<string[]>();
            try
            {
                LikeColumns.Add(new string[] { "CustomerJoinTypeDetail.TypeName" });
                LikeColumns.Add(new string[] { "CustomerJoinServiceItem.CustomerMaster_ID" });
                LikeColumns.Add(new string[] { "CustomerJoinServiceItem.CustomerName" });
                LikeColumns.Add(new string[] { "detailTemplate.TypeName" });
                LikeColumns.Add(new string[] { "ServiceItemMaster_ID" });
                LikeColumns.Add(new string[] { "Brand" });
                LikeColumns.Add(new string[] { "Model" });
                LikeColumns.Add(new string[] { "ReceivedDateTime" });

                records = ServiceItemMaster.GetGrid(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = ServiceItemMaster.GetGridCount(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveServiceItemDetail(ServiceItemDetail serviceItemDetail)
        {
            bool save = false;
            try
            {
                if (serviceItemDetail.ServiceItemDetail_ID == 0)
                {
                    serviceItemDetail.PaymentReceivable.Created_UserID = Global.UserId;
                    serviceItemDetail.Created_UserID = Global.UserId;
                }
                else
                {
                    PaymentReceivable objPayment = PaymentReceivable.Get(serviceItemDetail.PaymentReceivable.PaymentTotal_ID);
                    ServiceItemDetail objServiceDetail = ServiceItemDetail.Get(serviceItemDetail.ServiceItemDetail_ID);

                    serviceItemDetail.Created_UserID = objServiceDetail.Created_UserID;
                    serviceItemDetail.PaymentReceivable.Created_UserID = objPayment.Created_UserID;
                    serviceItemDetail.PaymentReceivable.Modified_UserID = Global.UserId;
                    serviceItemDetail.PaymentReceivable.Modified_DateTime = DateTime.Now;
                    serviceItemDetail.Modified_UserID = Global.UserId;
                    serviceItemDetail.Modified_DateTime = DateTime.Now;
                }
                serviceItemDetail.PaymentReceivable.PaymentCodeTemplate = Common.GetTypeMasterIdByName("PaymentCodeTemplate", null);
                serviceItemDetail.PaymentReceivable.IsActive = serviceItemDetail.IsActive;
                save = ServiceItemDetail.Save(serviceItemDetail);
                if (save)
                {
                    serviceItemDetail.ReturnValue = 1;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                serviceItemDetail.ReturnValue = 2;
            }
            return Json(serviceItemDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteServiceItemDetail(int id)
        {
            string success = "";
            try
            {
                if (id > 0)
                {
                    if(ServiceItemDetail.DeleteValidationForPayment(id))
                    {
                        bool save = ServiceItemDetail.Delete(id);
                        if(!save)
                        {
                            success = "Deleted Successfully";
                        }
                    }
                    else
                    {
                        success = "Please delete paid amount before delete this!";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(success);
        }

        [HttpGet]
        public JsonResult ServiceItemDetailGrid(int? page, int? limit, string sortBy, string direction, string searchString = null, int ServiceItemMaster_ID = 0)
        {
            int total = 0;
            IList records = new List<TypeDetailMaster>();
            if (ServiceItemMaster_ID > 0)
            {
                List<string[]> WhereList = new List<string[]>();
                WhereList.Add(new string[] { "ServiceItemMaster_ID", ServiceItemMaster_ID.ToString() });

                records = ServiceItemDetail.GetGrid(WhereList, null, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = records.Count;

            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
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
                result.Add(new ComboBox { id = 0, text = " -- Select StatusType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
	}
}