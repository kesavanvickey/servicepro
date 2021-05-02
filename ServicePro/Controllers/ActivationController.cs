using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ServicePro.BAL;

namespace ServicePro.Controllers
{
    public class ActivationController : Controller
    {
        //
        // GET: /Activation/
        public ActionResult Index()
        {
            Global.PageName = "Activation";
            return View();
        }

        public JsonResult Save(string jsonBuild)
        {
            bool save = false;
            bool duplicate = false;
            MasterControl masterControl = null;
            if(jsonBuild == null)
            {
                return null;
            }
            else
            {
                masterControl = Build(jsonBuild);
            }
            try
            {
                if(masterControl != null)
                {
                    bool key = CommonUtil.CheckProductKey(masterControl.ActivationMaster_Key);

                    if(key)
                    {
                        masterControl.ActivationMaster_Key = CommonUtil.Encrypt(masterControl.ActivationMaster_Key);

                        List<string[]> list = new List<string[]>();
                        list.Add(new string[] { "ActivationMaster_Key", masterControl.ActivationMaster_Key });
                        duplicate = Common.DuplicateCheck("Master.MasterControl", "ModifierLineNo", masterControl.ModifierLineNo, list);
                        if (duplicate == false)
                        {
                            save = MasterControl.Save(masterControl);
                            if (save)
                            {
                                masterControl.ReturnValue = 2;
                            }
                            else
                            {
                                masterControl.ReturnValue = 3;
                            }
                        }
                        else
                        {
                            masterControl.ReturnValue = 1;
                        }
                    }
                    else
                    {
                        masterControl.ReturnValue = 5;
                    }
                }
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
                masterControl.ReturnValue = 4;
            }
            return Json(masterControl,JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string jsonBuild)
        {
            bool delete = false;
            MasterControl masterControl = null;
            if (jsonBuild == null)
            {
                return null;
            }
            else
            {
                masterControl = Build(jsonBuild);
            }
            try
            {
                if (masterControl.ModifierLineNo > 0)
                {
                    delete = MasterControl.Delete(masterControl.ActivationMaster_Key);
                    if (delete)
                    {
                        masterControl.ReturnValue = 2;
                    }
                    else
                    {
                        masterControl.ReturnValue = 3;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                masterControl.ReturnValue = 4;
            }
            return Json(masterControl, JsonRequestBehavior.AllowGet);
        }
        public class BuildMasterControl
        {
            public string ActivationMaster_Key { get; set; }
            public string InstalledBy { get; set; }
            public string CompanyName { get; set; }
            public int IsActive { get; set; }
            public int ModifierLineNo { get; set; }
        }
        public MasterControl Build(string jsonBuild)
        {
            MasterControl masterControl = null;
            BuildMasterControl build = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                build = serializer.Deserialize<BuildMasterControl>(jsonBuild);
                masterControl = MasterControl.Get(build.ActivationMaster_Key);
                if (masterControl == null)
                {
                    masterControl = MasterControl.GetNew();
                } 
                else
                {
                    masterControl.IsActive = build.IsActive;
                }
                masterControl.ActivationMaster_Key = build.ActivationMaster_Key;
                masterControl.CompanyName = build.CompanyName;
                masterControl.InstalledBy = build.InstalledBy; 
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return masterControl;
        }
	}
}