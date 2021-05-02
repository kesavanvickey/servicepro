using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class WorkOrderMaster
    {
        //Database Models
        public virtual int WorkOrderMaster_ID { get; set; }
        public virtual int ServiceItemDetail_ID { get; set; }
        public virtual int WorkCodeTemplate { get; set; }
        public virtual int EmployeeMaster_ID { get; set; }
        public virtual DateTime ServiceStartDate { get; set; }
        public virtual DateTime? ServiceEndDate { get; set; }
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
        public virtual string WorkCodeTemplateName { get; set; }
        public virtual int ReturnValue { get; set; }
        public virtual ServiceItemDetail ServiceItemDetail { get; set; }
        public virtual int ItemReceivedHandler { get; set; }


        public static WorkOrderMaster Save(WorkOrderMaster workOrderMaster)
        {
            return new WorkOrderMasterService().SaveWorkOrderMaster(workOrderMaster);
        }
        public static bool Delete(int Id)
        {
            return new WorkOrderMasterService().DeleteWorkOrderMaster(Id);
        }
        public static WorkOrderMaster Get(int PrimaryKey, ISession session = null)
        {
            return new WorkOrderMasterService().GetWorkOrderMaster(PrimaryKey, session);
        }
        public static WorkOrderMaster GetNew()
        {
            return new WorkOrderMasterService().GetNewWorkOrderMaster();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new WorkOrderMasterService().GetWorkOrderMasterGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new WorkOrderMasterService().GetWorkOrderMasterGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
    }
}
