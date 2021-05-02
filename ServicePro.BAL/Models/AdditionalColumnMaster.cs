using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro.BAL
{
    public class AdditionalColumnMaster
    {
        //Database Models
        public virtual int AdditionalColumnMaster_ID { get; set; }
        public virtual int TableName { get; set; }
        public virtual int AdditionalColumnName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual int DataType { get; set; }
        public virtual int CompanyMaster_ID { get; set; }
        public virtual int IsActive { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }

        //Non DataBase Models
        public virtual int ReturnValue { get; set; }
        public virtual string TableNameString { get; set; }
        public virtual string AdditionalColumnNameString { get; set; }
        public virtual string DataTypeString { get; set; }


        public static AdditionalColumnMaster Save(AdditionalColumnMaster additionalColumnMaster)
        {
            return new AdditionalColumnMasterService().SaveAdditionalColumnMaster(additionalColumnMaster);
        }
        public static bool Delete(int Id)
        {
            return new AdditionalColumnMasterService().DeleteAdditionalColumnMaster(Id);
        }
        public static AdditionalColumnMaster Get(int PrimaryKey, ISession session = null)
        {
            return new AdditionalColumnMasterService().GetAdditionalColumnMaster(PrimaryKey, session);
        }
        public static AdditionalColumnMaster GetNew()
        {
            return new AdditionalColumnMasterService().GetNewAdditionalColumnMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new AdditionalColumnMasterService().GetAdditionalColumnGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new AdditionalColumnMasterService().GetAdditionalColumnGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}