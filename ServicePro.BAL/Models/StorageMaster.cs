using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class StorageMaster
    {
        //Database Models
        public virtual int StorageMaster_ID { get; set; }
        public virtual int? CodeTemplate { get; set; }
        public virtual int Ref_ID { get; set; }
        public virtual string FileName { get; set; }
        public virtual int StorageType { get; set; }
        public virtual string Extension { get; set; }
        public virtual string ContentType { get; set; }
        
        public virtual Byte[] StorageMaster_Data { get; set; }

        public virtual int? FileSize { get; set; }
        public virtual int IsActive { get; set; }
        public virtual string Created_UserID { get; set; }
        public virtual string Modified_UserID { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }
        public virtual string AddCol_1 { get; set; }
        public virtual string AddCol_2 { get; set; }
        public virtual string AddCol_3 { get; set; }
        public virtual string AddCol_4 { get; set; }
        public virtual string AddCol_5 { get; set; }

        //Non-Database Models
        public virtual string CodeTemplateName { get; set; }
        public virtual string StorageTypeName { get; set; }
        public virtual int ReturnValue { get; set; }



        public static bool Save(StorageMaster storageMaster)
        {
            return new StorageMasterService().SaveStorageMaster(storageMaster);
        }
        public static bool Delete(int Id)
        {
            return new StorageMasterService().DeleteStorageMaster(Id);
        }
        public static StorageMaster Get(int PrimaryKey, ISession session = null)
        {
            return new StorageMasterService().GetStorageMaster(PrimaryKey, session);
        }
        public static StorageMaster GetNew()
        {
            return new StorageMasterService().GetNewStorageMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new StorageMasterService().GetStorageMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new StorageMasterService().GetStorageMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}
