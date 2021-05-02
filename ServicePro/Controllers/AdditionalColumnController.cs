using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Collections;
using NHibernate;
using System.Web.Script.Serialization;
using ServicePro.BAL;

namespace ServicePro.Controllers
{
    public class AdditionalColumnController : Controller
    {
        //
        // GET: /AdditionalColumn/
        public ActionResult Index(int? id)
        {
            AdditionalColumnMaster additionalColumnMaster = null;

            //Is User Session Available
            if (Global.UserId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Check Permission for this page
            if(Global.UserType == 2)
            {
                return RedirectToAction("PermissionDenied", "Master");
            }

            try
            {
                if(id > 0)
                {
                    additionalColumnMaster = AdditionalColumnMaster.Get(Convert.ToInt32("0" + id));
                }
                if (additionalColumnMaster == null)
                {
                    additionalColumnMaster = AdditionalColumnMaster.GetNew();
                }
                Global.PageName = "AdditionalColumn Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View("Index",additionalColumnMaster);
        }
        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            AdditionalColumnMaster additionalColumnMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                additionalColumnMaster = Build(jsonBuild);
            }
            try
            {
                if (additionalColumnMaster != null)
                {
                    List<string[]> list = new List<string[]>();
                    list.Add(new string[] { "TableName", additionalColumnMaster.TableName.ToString() });
                    list.Add(new string[] { "AdditionalColumnName", additionalColumnMaster.AdditionalColumnName.ToString() });
                    duplicate = Common.DuplicateCheck("Master.AdditionalColumnMaster", "AdditionalColumnMaster_ID", additionalColumnMaster.AdditionalColumnMaster_ID, list);
                    if (duplicate == false)
                    {
                        additionalColumnMaster = AdditionalColumnMaster.Save(additionalColumnMaster);
                        if (additionalColumnMaster.ReturnValue != 2)
                        {
                            additionalColumnMaster.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        additionalColumnMaster.ReturnValue = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                additionalColumnMaster.ReturnValue = 4;
            }
            return Json(additionalColumnMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            AdditionalColumnMaster additionalColumnMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                additionalColumnMaster = Build(jsonBuild);
            }
            try
            {
                if (additionalColumnMaster.AdditionalColumnMaster_ID > 0)
                {
                    delete = AdditionalColumnMaster.Delete(additionalColumnMaster.AdditionalColumnMaster_ID);
                    if (delete)
                    {
                        additionalColumnMaster.ReturnValue = 1;
                    }
                    else
                    {
                        additionalColumnMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                additionalColumnMaster.ReturnValue = 3;
            }
            return Json(additionalColumnMaster, JsonRequestBehavior.AllowGet);
        }
        public AdditionalColumnMaster Build(string jsonBuild)
        {
            AdditionalColumnMaster additionalColumnMaster = null;
            BuildAdditionalColumnMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildAdditionalColumnMaster>(jsonBuild);
                additionalColumnMaster = AdditionalColumnMaster.Get(build.AdditionalColumnMaster_ID);
                if (additionalColumnMaster == null)
                {
                    additionalColumnMaster = AdditionalColumnMaster.GetNew();
                }
                else
                {
                    additionalColumnMaster.IsActive = build.IsActive;
                    additionalColumnMaster.Modified_DateTime = DateTime.Now;
                }
                additionalColumnMaster.AdditionalColumnName = build.AdditionalColumnName;
                additionalColumnMaster.CompanyMaster_ID = build.CompanyMaster_ID;
                additionalColumnMaster.DataType = build.DataType;
                additionalColumnMaster.DisplayName = build.DisplayName;
                additionalColumnMaster.TableName = build.TableName;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return additionalColumnMaster;
        }
        public class BuildAdditionalColumnMaster
        {
            public int AdditionalColumnMaster_ID { get; set; }
            public int TableName { get; set; }
            public int AdditionalColumnName { get; set; }
            public string DisplayName { get; set; }
            public int DataType { get; set; }
            public int CompanyMaster_ID { get; set; }
            public int IsActive { get; set; }
            public DateTime? Modified_DateTime { get; set; }
        }
        public JsonResult GetTableName(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("TableName", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select TableName -- " });
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAdditionalColumnName(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("AdditionalColumns", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select Additional Column -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("DataType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select DataType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAdditionalColumnGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = new List<AdditionalColumnMaster>();
            try
            {
                List<string[]> SearchColumns = new List<string[]>();
                SearchColumns.Add(new string[] { "TableName.TypeName" });
                SearchColumns.Add(new string[] { "AdditionalColumnMaster.DisplayName" });
                SearchColumns.Add(new string[] { "DataType.TypeName" });
                SearchColumns.Add(new string[] { "TableName.TypeName" });

                records = AdditionalColumnMaster.GetGrid(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = AdditionalColumnMaster.GetGridCount(null, SearchColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
	}
}