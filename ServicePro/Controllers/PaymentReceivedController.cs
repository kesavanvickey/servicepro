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
    public class PaymentReceivedController : Controller
    {
        //
        // GET: /PaymentReceived/
        public ActionResult Index(int? id)
        {
            PaymentReceived paymentReceived = null;
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                paymentReceived = PaymentReceived.Get(Convert.ToInt32("0" + id));

                if (paymentReceived == null)
                {
                    paymentReceived = PaymentReceived.GetNew();
                }

                Global.PageName = "PaymentReceived";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View(paymentReceived);
        }

        public JsonResult Save(string jsonBuild)
        {
            PaymentReceived paymentReceived = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                paymentReceived = Build(jsonBuild);
            }
            try
            {
                if (paymentReceived != null)
                {
                    paymentReceived = PaymentReceived.Save(paymentReceived);
                    if (paymentReceived.SaveSuccess)
                    {
                        paymentReceived.ReturnValue = 2;
                    }
                    else
                    {
                        paymentReceived.ReturnValue = 3;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                paymentReceived.ReturnValue = 4;
            }
            return Json(paymentReceived, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string Id)
        {
            bool delete = false;
            PaymentReceived paymentReceived = PaymentReceived.Get(Convert.ToInt32("0" + Id));
            try
            {
                if(paymentReceived != null)
                {
                    if (paymentReceived.PaymentReceived_ID > 0)
                    {
                        delete = PaymentReceived.Delete(paymentReceived);
                        if (delete)
                        {
                            paymentReceived.ReturnValue = 1;
                        }
                        else
                        {
                            paymentReceived.ReturnValue = 2;
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                paymentReceived.ReturnValue = 3;
            }
            return Json(paymentReceived, JsonRequestBehavior.AllowGet);
        }
        public PaymentReceived Build(string jsonBuild)
        {
            PaymentReceived paymentReceived = null;
            BuildPaymentReceived build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildPaymentReceived>(jsonBuild);
                paymentReceived = PaymentReceived.Get(build.PaymentReceived_ID);
                if (paymentReceived == null)
                {
                    paymentReceived = PaymentReceived.GetNew();
                    paymentReceived.PaymentReceivedBy = Global.CurrentEmployeeId;
                    paymentReceived.Created_UserID = Global.UserId;
                }
                else
                {
                    paymentReceived.Modified_UserID = Global.UserId;
                    paymentReceived.Modified_DateTime = DateTime.Now;
                }
                paymentReceived.ReceivedDateTime = build.ReceivedDateTime;
                paymentReceived.ServiceItemMaster_ID = build.ServiceItemMaster_ID;
                paymentReceived.Amount = build.Amount;
                paymentReceived.PaymentType = build.PaymentType;
                paymentReceived.PaymentReferenceNo = build.PaymentReferenceNo;
                paymentReceived.IsActive = build.IsActive;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return paymentReceived;
        }
        public class BuildPaymentReceived
        {
            public int PaymentReceived_ID { get; set; }
            public int ServiceItemMaster_ID { get; set; }
            public Decimal Amount { get; set; }
            public int PaymentType { get; set; }
            public string PaymentReferenceNo { get; set; }
            public int PaymentReceivedBy { get; set; }
            public DateTime ReceivedDateTime { get; set; }
            public int IsActive { get; set; }
        }

        [HttpGet]
        public JsonResult GetPaymentReceivedGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = null;
            List<string[]> LikeColumns = new List<string[]>();
            try
            {
                LikeColumns.Add(new string[] { "PaymentReceived.Amount" });
                LikeColumns.Add(new string[] { "TypeDetailMaster.TypeName" });
                LikeColumns.Add(new string[] { "PaymentReceived.ReceivedDateTime" });
                LikeColumns.Add(new string[] { "ServiceItemMaster.Brand" });
                LikeColumns.Add(new string[] { "ServiceItemMaster.Model" });
                LikeColumns.Add(new string[] { "customerCodeDetail.TypeName" });
                LikeColumns.Add(new string[] { "CustomerMaster.CustomerCode" });
                LikeColumns.Add(new string[] { "CustomerMaster.CustomerName" });
                LikeColumns.Add(new string[] { "EmployeeMaster.EmployeeName" });

                records = PaymentReceived.GetGrid(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = PaymentReceived.GetGridCount(null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("PaymentType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select PaymentType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceItem(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {

                dt = ServiceItemMaster.GetServiceItemMasterForCBO(null, null);
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

        public JsonResult GetComments(int ServiceItemMaster_ID)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                List<string[]> WhereList = new List<string[]>();
                WhereList.Add(new string[] { "ServiceItemMaster_ID", ServiceItemMaster_ID.ToString() });

                dt = Common.FetchTable("ServicePro.ServiceItemDetail", WhereList);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["ServiceItemDetail_ID"]),
                    text = row["Comments"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select Comments -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentReceivedList(int ServiceItemMaster_ID)
        {
            IList list = null;
            try
            {
                List<string[]> WhereList = new List<string[]>();
                WhereList.Add(new string[] { "ServiceItemMaster.ServiceItemMaster_ID", ServiceItemMaster_ID.ToString() });

                list = PaymentReceived.GetPaymentReceivedList(WhereList, null);
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
	}
}