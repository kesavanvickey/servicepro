using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class ServiceItemMaster
    {
        //Database Models
        public virtual int ServiceItemMaster_ID { get; set; }
        public virtual int ServiceCodeTemplate { get; set; }
        public virtual int CustomerMaster_ID { get; set; }
        public virtual int EmployeeMaster_ID { get; set; }
        public virtual string Brand { get; set; }
        public virtual string Model { get; set; }
        public virtual DateTime ItemOrderDate { get; set; }
        public virtual DateTime? ItemExpectedDeliverDate { get; set; }
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
        public virtual string ServiceCodeTemplateName { get; set; }
        public virtual int ReturnValue { get; set; }
        public virtual int ItemReceivedHandler { get; set; }




        public static ServiceItemMaster Save(ServiceItemMaster serviceItemMaster)
        {
            return new ServiceItemMasterService().SaveServiceItemMaster(serviceItemMaster);
        }
        public static bool Delete(int Id)
        {
            return new ServiceItemMasterService().DeleteServiceItemMaster(Id);
        }
        public static ServiceItemMaster Get(int PrimaryKey, ISession session = null)
        {
            return new ServiceItemMasterService().GetServiceItemMaster(PrimaryKey, session);
        }
        public static ServiceItemMaster GetNew()
        {
            return new ServiceItemMasterService().GetNewServiceItemMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new ServiceItemMasterService().GetServiceItemMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new ServiceItemMasterService().GetServiceItemMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static DataTable GetServiceItemMasterCboByStatus(string StatusTypeName, List<string[]> WhereCondition = null, ISession session = null)
        {
            return new ServiceItemMasterService().GetServiceItemMasterCboByStatus(StatusTypeName, WhereCondition, session);
        }
        public static DataTable GetServiceItemMasterForCBO(List<string[]> WhereCondition = null, ISession session = null)
        {
            return new ServiceItemMasterService().GetServiceItemMasterForCBO(WhereCondition, session);
        }
    }
}
