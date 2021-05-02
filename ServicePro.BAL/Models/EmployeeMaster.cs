using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePro.BAL
{
    public class EmployeeMaster
    {
        //Database Models
        public virtual int EmployeeMaster_ID { get; set; }
        public virtual int EmployeeCode { get; set; }
        public virtual int EmployeeCodeTemplate { get; set; }
        public virtual string EmployeeName { get; set; }
        public virtual DateTime DOB { get; set; }
        public virtual string Gender { get; set; }
        public virtual int EmployeeType { get; set; }
        public virtual DateTime JointDate { get; set; }
        public virtual int CompanyMaster_ID { get; set; }
        public virtual string AddCol_1 { get; set; }
        public virtual string AddCol_2 { get; set; }
        public virtual string AddCol_3 { get; set; }
        public virtual string AddCol_4 { get; set; }
        public virtual string AddCol_5 { get; set; }
        public virtual int IsActive { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }

        //Non DataBase Models
        public virtual int ReturnValue { get; set; }
        public virtual string EmployeeCodeTemplateName { get; set; }
        public virtual string EmployeeTypeName { get; set; }
        public virtual string EmployeeCodeFullName { get; set; }


        public static EmployeeMaster Save(EmployeeMaster employeeMaster)
        {
            return new EmployeeMasterService().SaveEmployeeMaster(employeeMaster);
        }
        public static bool Delete(int Id)
        {
            return new EmployeeMasterService().DeleteEmployeeMaster(Id);
        }
        public static EmployeeMaster Get(int PrimaryKey, ISession session = null)
        {
            return new EmployeeMasterService().GetEmployeeMaster(PrimaryKey, session);
        }
        public static EmployeeMaster GetNew()
        {
            return new EmployeeMasterService().GetNewEmployeeMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new EmployeeMasterService().GetEmployeeMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new EmployeeMasterService().GetEmployeeMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}