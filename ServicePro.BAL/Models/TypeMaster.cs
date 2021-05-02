using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro.BAL
{
    public class TypeMaster 
    {
        //Database Models
        public virtual int TypeMaster_ID { get; set; }
        public virtual string TypeMasterName { get; set; }
        public virtual string ShortName { get; set; }
        public virtual int? Parent_ID { get; set; }
        public virtual string Description { get; set; }
        public virtual int CompanyMaster_ID { get; set; }
        public virtual int IsActive { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }

        //Non-Database Models
        public virtual int ReturnValue { get; set; }
        public virtual IList<TypeDetailMaster> TypeDetailMaster { get; set; }



        public static TypeMaster Save(TypeMaster typeMaster)
        {
            return new TypeMasterService().SaveTypeMaster(typeMaster);
        }
        public static bool Delete(int Id)
        {
            return new TypeMasterService().DeleteTypeMaster(Id);
        }
        public static TypeMaster Get(int PrimaryKey, ISession session = null)
        {
            return new TypeMasterService().GetTypeMaster(PrimaryKey, session);
        }
        public static TypeMaster GetNew()
        {
            return new TypeMasterService().GetNewTypeMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new TypeMasterService().GetTypeMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new TypeMasterService().GetTypeMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}