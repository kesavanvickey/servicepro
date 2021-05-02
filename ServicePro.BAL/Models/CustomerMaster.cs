using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class CustomerMaster
    {
        //Database Models
        public virtual int CustomerMaster_ID { get; set; }
        public virtual int CustomerCodeTemplate { get; set; }
        public virtual int CustomerCode { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual DateTime DOB { get; set; }
        public virtual string Gender { get; set; }
        public virtual int EmployeeMaster_ID { get; set; }
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
        public virtual int ReturnValue { get; set; }
        public virtual string CustomerCodeTemplateName { get; set; }
        public virtual string CustomerCodeFullName { get; set; }



        public static CustomerMaster Save(CustomerMaster customerMaster)
        {
            return new CustomerMasterService().SaveCustomerMaster(customerMaster);
        }
        public static bool Delete(int Id)
        {
            return new CustomerMasterService().DeleteCustomerMaster(Id);
        }
        public static CustomerMaster Get(int PrimaryKey, ISession session = null)
        {
            return new CustomerMasterService().GetCustomerMaster(PrimaryKey, session);
        }
        public static CustomerMaster GetNew()
        {
            return new CustomerMasterService().GetNewCustomerMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new CustomerMasterService().GetCustomerMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new CustomerMasterService().GetCustomerMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}
