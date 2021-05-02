using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicePro.BAL;
using System.Web.Script.Serialization;
using System.Collections;
using NHibernate;

namespace ServicePro.Controllers
{
    public class TypeMasterController : Controller
    {
        //
        // GET: /TypeMaster/
        public ActionResult Index(int? id)
        {
            TypeMaster typeMaster = null;

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

            try
            {
                typeMaster = TypeMaster.Get(Convert.ToInt32("0" + id));
                if(typeMaster == null)
                {
                    typeMaster = TypeMaster.GetNew();
                }
                Global.PageName = "Type Master";
                Global.ShowPageName = true;
                Global.ShowToolBar = true;
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View("Index",typeMaster);
        }
        public JsonResult Save(string jsonBuild)
        {
            bool duplicate = false;
            TypeMaster typeMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                typeMaster = Build(jsonBuild);
            }
            try
            {
                if (typeMaster != null)
                {
                    List<string[]> list = new List<string[]>();
                    list.Add(new string[] { "TypeMasterName", typeMaster.TypeMasterName });
                    duplicate = Common.DuplicateCheck("Master.TypeMaster", "TypeMaster_ID", typeMaster.TypeMaster_ID, list);
                    if (duplicate == false)
                    {
                        typeMaster = TypeMaster.Save(typeMaster);
                        if (typeMaster.ReturnValue != 2)
                        {
                            typeMaster.ReturnValue = 3;
                        }
                    }
                    else if (duplicate == true)
                    {
                        typeMaster.ReturnValue = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                typeMaster.ReturnValue = 4;
            }
            return Json(typeMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            TypeMaster typeMaster = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                typeMaster = Build(jsonBuild);
            }
            try
            {
                if (typeMaster.CompanyMaster_ID > 0)
                {
                    delete = TypeMaster.Delete(typeMaster.TypeMaster_ID);
                    if (delete)
                    {
                        typeMaster.ReturnValue = 1;
                    }
                    else
                    {
                        typeMaster.ReturnValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                typeMaster.ReturnValue = 3;
            }
            return Json(typeMaster, JsonRequestBehavior.AllowGet);
        }
        public TypeMaster Build(string jsonBuild)
        {
            TypeMaster typeMaster = null;
            BuildTypeMaster build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<TypeDetailMaster> child = new List<TypeDetailMaster>();
            TypeDetailMaster typeDetailMaster = null;
            try
            {
                build = serializer.Deserialize<BuildTypeMaster>(jsonBuild);
                typeMaster = TypeMaster.Get(build.TypeMaster_ID);
                if (typeMaster == null)
                {
                    typeMaster = TypeMaster.GetNew();
                }
                else
                {
                    typeMaster.IsActive = build.IsActive;
                    typeMaster.Modified_DateTime = DateTime.Now;
                }
                typeMaster.TypeMasterName = build.TypeMasterName;
                typeMaster.ShortName = build.ShortName;
                typeMaster.Parent_ID = build.Parent_ID;
                typeMaster.Description = build.Description;
                typeMaster.IsActive = build.IsActive;
                typeMaster.CompanyMaster_ID = Global.CompanyId;
                
                if(typeMaster.TypeDetailMaster.Count > 0)
                {
                    foreach(TypeDetailMaster list in typeMaster.TypeDetailMaster)
                    {
                        typeDetailMaster = new TypeDetailMaster();
                        typeDetailMaster = TypeDetailMaster.Get(list.TypeDetailMaster_ID);
                        if (typeDetailMaster == null)
                        {
                            typeDetailMaster = TypeDetailMaster.GetNew();
                        }
                        else
                        {
                            typeDetailMaster.IsActive = list.IsActive;
                            typeDetailMaster.Modified_DateTime = DateTime.Now;
                        }
                        typeDetailMaster.TypeName = list.TypeName;
                        typeDetailMaster.Description = list.Description;
                        child.Add(typeDetailMaster);
                    }
                    typeMaster.TypeDetailMaster = child;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return typeMaster;
        }
        public class BuildTypeMaster
        {
            public int TypeMaster_ID { get; set; }
            public string TypeMasterName { get; set; }
            public string ShortName { get; set; }
            public int? Parent_ID { get; set; }
            public string Description { get; set; }
            public int CompanyMaster_ID { get; set; }
            public int IsActive { get; set; }
            public string ReturnValue { get; set; }
            public DateTime Modified_DateTime { get; set; }
            public IList<BuildTypeDetailMaster> TypeDetailMaster { get; set; }
        }
        public class BuildTypeDetailMaster
        {
            public int TypeDetailMaster_ID { get; set; }
            public int TypeMaster_ID { get; set; }
            public string TypeName { get; set; }
            public string Description { get; set; }
            public int IsActive { get; set; }
            public int CompanyMaster_ID { get; set; }
        }

        [HttpGet]
        public JsonResult GetTypeMaster(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = new List<TypeMaster>();
            try
            {
                List<string[]> LikeColumns = new List<string[]>();
                LikeColumns.Add(new string[] { "TypeMasterName" });
                LikeColumns.Add(new string[] { "Description" });

                records = Common.GetGridList("Master.TypeMaster", null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = Common.GetGridListCount("Master.TypeMaster", null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTypeDetailMaster(int? page, int? limit, string sortBy, string direction, string searchString = null,int Parameter = 0)
        {
            int total = 0;
            IList records = new List<TypeDetailMaster>();
            if(Parameter > 0)
            {
                List<string[]> WhereList = new List<string[]>();
                WhereList.Add(new string[] { "TypeMaster_ID", Parameter.ToString() });
                
                List<string[]> LikeColumns = new List<string[]>();
                LikeColumns.Add(new string[] { "TypeName" });
                LikeColumns.Add(new string[] { "Description" });

                records = Common.GetGridList("Master.TypeDetailMaster", WhereList, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = Common.GetGridListCount("Master.TypeDetailMaster", WhereList, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GridTypeDetailSave(TypeDetailMaster typeDetailMaster)
        {
            bool save = false;
            bool duplicate = false;
            try
            {
                List<string[]> list = new List<string[]>();
                list.Add(new string[] { "TypeName", typeDetailMaster.TypeName });
                duplicate = Common.DuplicateCheck("Master.TypeDetailMaster", "TypeDetailMaster_ID", typeDetailMaster.TypeDetailMaster_ID, list);
                if(duplicate)
                {
                    typeDetailMaster.ReturnValue = 2;
                }
                else
                {
                    if(typeDetailMaster.TypeDetailMaster_ID > 0 )
                    {
                        typeDetailMaster.Modified_DateTime = DateTime.Now;
                    }
                    save = TypeDetailMaster.Save(typeDetailMaster);
                    if(save)
                    {
                        typeDetailMaster.ReturnValue = 1;
                    }
                }
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(typeDetailMaster,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GridTypeDetailDelete(int id)
        {
            bool success = false;
            try
            {
                if(id > 0)
                {
                    success = TypeDetailMaster.Delete(id);
                }
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(success);
        }
	}
}