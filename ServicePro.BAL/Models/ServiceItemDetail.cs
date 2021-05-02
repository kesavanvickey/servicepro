using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class ServiceItemDetail
    {
        //Database Models
        public virtual int ServiceItemDetail_ID { get; set; }
        public virtual int ServiceItemMaster_ID { get; set; }
        public virtual string Comments { get; set; }
        public virtual int StatusType { get; set; }
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
        public virtual PaymentReceivable PaymentReceivable { get; set; }



        public static bool Save(ServiceItemDetail serviceItemDetail)
        {
            return new ServiceItemDetailService().SaveServiceItemDetail(serviceItemDetail);
        }
        public static bool Delete(int Id)
        {
            return new ServiceItemDetailService().DeleteServiceItemDetail(Id);
        }
        public static ServiceItemDetail Get(int PrimaryKey, ISession session = null)
        {
            return new ServiceItemDetailService().GetServiceItemDetail(PrimaryKey, session);
        }
        public static ServiceItemDetail GetNew()
        {
            return new ServiceItemDetailService().GetNewServiceItemDetail();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new ServiceItemDetailService().GetServiceItemDetailGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static IList GetServiceItemDetailForWorkOrder(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new ServiceItemDetailService().GetServiceItemDetailForWorkOrder(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static bool DeleteValidationForPayment(int serviceItemDetailID, ISession session = null)
        {
            return new ServiceItemDetailService().DeleteValidationForPayment(serviceItemDetailID, session);
        }
        public static Decimal GetPaidAmoutByItemDetailId(int serviceItemDetailID, ISession session = null)
        {
            return new ServiceItemDetailService().GetPaidAmoutByItemDetailId(serviceItemDetailID, session);
        }
    }
}
