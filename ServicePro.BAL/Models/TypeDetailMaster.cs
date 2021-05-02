using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro.BAL
{
    public class TypeDetailMaster
    {
        //Database Models
        public virtual int TypeDetailMaster_ID { get; set; }
        public virtual int TypeMaster_ID { get; set; }
        public virtual string TypeName { get; set; }
        public virtual string Description { get; set; }
        public virtual int IsActive { get; set; }
        public virtual int CompanyMaster_ID { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }

        //Non Database Models
        public virtual int ReturnValue { get; set; }
        public virtual string TypeMasterName { get; set; }


        public static bool Save(TypeDetailMaster typeDetailMaster)
        {
            return new TypeDetailMasterService().SaveTypeDetailMaster(typeDetailMaster);
        }
        public static bool Delete(int Id)
        {
            return new TypeDetailMasterService().DeleteTypeDetailMaster(Id);
        }
        public static TypeDetailMaster Get(int PrimaryKey, ISession session = null)
        {
            return new TypeDetailMasterService().GetTypeDetailMaster(PrimaryKey, session);
        }
        public static TypeDetailMaster GetNew()
        {
            return new TypeDetailMasterService().GetNewTypeDetailMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new TypeDetailMasterService().GetTypeDetailMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new TypeDetailMasterService().GetTypeDetailMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}