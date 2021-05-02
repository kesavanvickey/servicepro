using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro.BAL
{
    public class CompanyMaster 
    {
        //Database Models
        public virtual int CompanyMaster_ID { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CompanyType { get; set; }
        public virtual string TinNo { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Recovery_Mobile { get; set; }
        public virtual string Recovery_Email { get; set; }
        public virtual string Recovery_Question { get; set; }
        public virtual string Recovery_Answer { get; set; }
        public virtual string ActivationMaster_Key { get; set; }
        public virtual int IsActive { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }
        public virtual string CompanyLogo { get; set; }
        public virtual string ReportSignature { get; set; }
        public virtual string ReportBottom { get; set; }

        //Non-Database Models
        public virtual int ReturnValue { get; set; }
        //public virtual IList AddressMaster { get; set; }
        //public virtual IList IDProofMaster { get; set; }
        //public virtual IList StorageMaster { get; set; }



        public static CompanyMaster Save(CompanyMaster companyMaster)
        {
            return new CompanyMasterService().SaveCompanyMaster(companyMaster);
        }
        public static bool Delete(int Id)
        {
            return new CompanyMasterService().DeleteCompanyMaster(Id);
        }
        public static CompanyMaster Get(int PrimaryKey, ISession session = null)
        {
            return new CompanyMasterService().GetCompanyMaster(PrimaryKey, session);
        }
        public static CompanyMaster GetNew()
        {
            return new CompanyMasterService().GetNewCompanyMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new CompanyMasterService().GetCompanyMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new CompanyMasterService().GetCompanyMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}