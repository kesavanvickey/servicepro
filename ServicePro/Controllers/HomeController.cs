using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicePro.BAL;
using System.Collections;
using System.Data;
using System.IO;

namespace ServicePro.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /AdminHome/
        public ActionResult Index()
        {
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Global.PageName = "Home";
                Global.ShowPageName = false;
                Global.ShowToolBar = false;
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View("Index");
        }
        public ActionResult LogOut()
        {
            try
            {
                Session.Clear();
                if(Session.Count == 0)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View("Index");
        }
        public FileResult Download(int Id)
        {
            StorageMaster obj = null;
            byte[] fileBytes = null;
            string fileName = "";
            try
            {
                obj = StorageMaster.Get(Id, null);
                fileBytes = obj.StorageMaster_Data;
                fileName = obj.FileName + obj.Extension;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult Invoice(int id, int serviceItemId)
        {
            Invoice invoice = new Invoice();
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                if(id > 0)
                {
                    invoice = ServicePro.BAL.Invoice.Get(id, null);
                    if(invoice == null)
                    {
                        invoice = new Invoice();
                    }
                }
                else
                {
                    if (serviceItemId > 0)
                    {
                        invoice = Common.GetReportInvoice(serviceItemId, Global.UserId);
                    }
                }

                Global.PageName = "Invoice";
                Global.ShowPageName = false;
                Global.ShowToolBar = true;
            }
            catch(Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View(invoice);
        }

        [HttpGet]
        public JsonResult GetInvoiceGrid(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total = 0;
            IList records = new List<TypeMaster>();
            try
            {
                List<string[]> LikeColumns = new List<string[]>();
                LikeColumns.Add(new string[] { "InvoiceID" });
                LikeColumns.Add(new string[] { "ServiceItemId" });
                LikeColumns.Add(new string[] { "CustomerId" });
                LikeColumns.Add(new string[] { "CustomerName" });
                LikeColumns.Add(new string[] { "ItemName" });
                LikeColumns.Add(new string[] { "PrintDateTime" });
                LikeColumns.Add(new string[] { "Created_UserId" });

                records = Common.GetGridList("ServicePro.Invoice", null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = Common.GetGridListCount("ServicePro.Invoice", null, LikeColumns, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            try
            {
                if (Global.UserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                Global.PageName = "ChangePassword";
                Global.ShowPageName = false;
                Global.ShowToolBar = false;
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return View();
        }

        #region StorageMaster
        public JsonResult GetStorageType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("StorageType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select StorageType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStorageMaster(int? page, int? limit, string sortBy, string direction, string searchString = null, int Ref_ID = 0, string CodeTempate = null)
        {
            int total = 0;
            IList records = null;
            if (Ref_ID > 0)
            {
                List<string[]> WhereList = new List<string[]>();
                WhereList.Add(new string[] { "Ref_ID", Ref_ID.ToString() });
                WhereList.Add(new string[] { "CodeTemplate", CodeTempate.ToString() });

                records = StorageMaster.GetGrid(WhereList, null, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = records.Count;
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveStorageMaster()
        {
            bool save = false;
            StorageMaster obj = null;
            HttpPostedFileBase file = null;
            Byte[] FileData = null;
            try
            {
                //Request.Form.Get("StorageType")
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        file = files[i];
                    }

                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    FileData = Br.ReadBytes((Int32)str.Length);
                }
                if (Convert.ToInt32("0" + Request.Form.Get("StorageMaster_ID")) > 0)
                {
                    obj = StorageMaster.Get(Convert.ToInt32("0" + Request.Form.Get("StorageMaster_ID")));
                    obj.Modified_UserID = Global.UserId;
                    obj.Modified_DateTime = DateTime.Now;
                }
                else
                {
                    obj = StorageMaster.GetNew();
                    obj.Created_UserID = Global.UserId;
                }

                obj.StorageType = Convert.ToInt32("0" + Request.Form.Get("StorageType"));
                if (Request.Form.Get("CodeTemplate") != "null")
                    obj.CodeTemplate = Convert.ToInt32(Request.Form.Get("CodeTemplate"));
                obj.Ref_ID = Convert.ToInt32(Request.Form.Get("Ref_ID"));
                if (Request.Form.Get("FileName").ToString() != "")
                {
                    obj.FileName = Request.Form.Get("FileName").ToString();
                }
                else
                {
                    obj.FileName = file.FileName;
                }
                obj.IsActive = Convert.ToInt32(Request.Form.Get("IsActive"));
                if (Request.Files.Count > 0)
                {
                    obj.StorageMaster_Data = FileData;
                    obj.Extension = Path.GetExtension(file.FileName);
                    obj.ContentType = Request.ContentType.ToString();
                    obj.FileSize = Request.ContentLength;
                }


                save = StorageMaster.Save(obj);
                if (save)
                {
                    obj.ReturnValue = 1;
                }
                
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(obj.ReturnValue);
        }

        [HttpPost]
        public JsonResult DeleteStorageMaster(int id)
        {
            bool success = false;
            try
            {
                if (id > 0)
                {
                    success = StorageMaster.Delete(id);
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(success);
        }
        #endregion

        #region IDProofMaster
        public JsonResult GetIDProofType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("IDProofType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select IDProofType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetIDProof(int? page, int? limit, string sortBy, string direction, string searchString = null, int Ref_ID = 0, string CodeTempate = null)
        {
            int total = 0;
            IList records = null;
            if (Ref_ID > 0)
            {
                List<string[]> WhereList = new List<string[]>();
                WhereList.Add(new string[] { "Ref_ID", Ref_ID.ToString() });
                WhereList.Add(new string[] { "CodeTemplate", CodeTempate.ToString() });

                records = IDProofMaster.GetGrid(WhereList, null, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = records.Count;
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveIDProof(IDProofMaster iDProofMaster)
        {
            bool save = false;
            try
            {
                if (iDProofMaster.IDProofMaster_ID > 0)
                {
                    iDProofMaster.Modified_UserID = Global.UserId;
                    iDProofMaster.Modified_DateTime = DateTime.Now;
                }
                else
                {
                    iDProofMaster.Created_UserID = Global.UserId;
                }
                save = IDProofMaster.Save(iDProofMaster);
                if (save)
                {
                    iDProofMaster.ReturnValue = 1;
                }

            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(iDProofMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteIDProof(int id)
        {
            bool success = false;
            try
            {
                if (id > 0)
                {
                    success = IDProofMaster.Delete(id);
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(success);
        }
        #endregion

        #region AddressMaster
        [HttpGet]
        public JsonResult GetAddress(int? page, int? limit, string sortBy, string direction, string searchString = null, int Ref_ID = 0, string CodeTempate = null)
        {
            int total = 0;
            IList records = null;
            if (Ref_ID > 0)
            {
                List<string[]> WhereList = new List<string[]>();
                WhereList.Add(new string[] { "AddressMaster.Ref_ID", Ref_ID.ToString() });
                WhereList.Add(new string[] { "AddressMaster.CodeTemplate", CodeTempate.ToString() });

                //List<string[]> LikeColumns = new List<string[]>();
                //LikeColumns.Add(new string[] { "AddressTypeDetail.TypeName" });
                //LikeColumns.Add(new string[] { "AddressMaster.Address1" });
                //LikeColumns.Add(new string[] { "AddressMaster.Address2" });
                //LikeColumns.Add(new string[] { "AddressMaster.Address3" });
                //LikeColumns.Add(new string[] { "CityDetail.TypeName" });
                //LikeColumns.Add(new string[] { "StateDetail.TypeName" });
                //LikeColumns.Add(new string[] { "CountryDetail.TypeName" });
                //LikeColumns.Add(new string[] { "AddressMaster.Pincode" });
                //LikeColumns.Add(new string[] { "AddressMaster.ContactNo1" });
                //LikeColumns.Add(new string[] { "AddressMaster.ContactNo2" });
                //LikeColumns.Add(new string[] { "AddressMaster.Email" });
                //LikeColumns.Add(new string[] { "AddressMaster.IsActive" });

                records = AddressMaster.GetGrid(WhereList, null, searchString, sortBy, direction, Convert.ToInt32("0" + page), Convert.ToInt32("0" + limit), null);
                total = records.Count;
            }
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAddress(AddressMaster addressMaster)
        {
            bool save = false;
            bool duplicate = false;
            try
            {
                List<string[]> list = new List<string[]>();
                list.Add(new string[] { "ContactNo1", addressMaster.ContactNo1.ToString() });
                duplicate = Common.DuplicateCheck("ServicePro.AddressMaster", "AddressMaster_ID", addressMaster.AddressMaster_ID, list);
                if (duplicate)
                {
                    addressMaster.ReturnValue = 2;
                }
                else
                {
                    if (addressMaster.AddressMaster_ID > 0)
                    {
                        addressMaster.Modified_UserID = Global.UserId;
                        addressMaster.Modified_DateTime = DateTime.Now;
                    }
                    else
                    {
                        addressMaster.Created_UserID = Global.UserId;
                    }
                    save = AddressMaster.Save(addressMaster);
                    if (save)
                    {
                        addressMaster.ReturnValue = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(addressMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAddress(int id)
        {
            bool success = false;
            try
            {
                if (id > 0)
                {
                    success = AddressMaster.Delete(id);
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(success);
        }

        public JsonResult GetAddressType(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("AddressType", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select AddressType -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCity(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("City", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select City -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetState(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("State", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select State -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountry(string searchTerm)
        {
            IList<ComboBox> result = new List<ComboBox>();
            DataTable dt = null;
            try
            {
                dt = Common.GetType("Country", true, Global.CompanyId, null);
                result = dt.Select().AsEnumerable().Select(row => new ComboBox
                {
                    id = Convert.ToInt32(row["TypeDetailMaster_ID"]),
                    text = row["TypeName"].ToString()
                }).ToList();
                result.Add(new ComboBox { id = 0, text = " -- Select Country -- " });
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return Json(result.OrderBy(e => e.id), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}                                                                                                                                                                                                   