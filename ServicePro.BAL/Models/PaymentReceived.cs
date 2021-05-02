using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class PaymentReceived
    {
        //Database Models
        public virtual int PaymentReceived_ID { get; set; }
        public virtual int ServiceItemMaster_ID { get; set; }
        public virtual Decimal Amount { get; set; }
        public virtual int PaymentType { get; set; }
        public virtual string PaymentReferenceNo { get; set; }
        public virtual int PaymentReceivedBy { get; set; }
        public virtual DateTime ReceivedDateTime { get; set; }
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
        public virtual string PaymentTypeName { get; set; }
        public virtual bool SaveSuccess { get; set; }
        //public virtual int ServiceItemMaster_ID { get; set; }
        public virtual int Comments { get; set; }
        public virtual int ItemReceivedHandler { get; set; }
        public virtual ServiceItemMaster ServiceItemMaster { get; set; }


        public static PaymentReceived Save(PaymentReceived paymentReceived)
        {
            return new PaymentReceivedService().SavePaymentReceived(paymentReceived);
        }
        public static bool Delete(PaymentReceived paymentReceived)
        {
            return new PaymentReceivedService().DeletePaymentReceived(paymentReceived);
        }
        public static PaymentReceived Get(int PrimaryKey, ISession session = null)
        {
            return new PaymentReceivedService().GetPaymentReceived(PrimaryKey, session);
        }
        public static PaymentReceived GetNew()
        {
            return new PaymentReceivedService().GetNewPaymentReceived();
        }
        public static IList GetGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new PaymentReceivedService().GetPaymentReceivedGrid(WhereCondition, SearchColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static int GetGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            return new PaymentReceivedService().GetPaymentReceivedGridCount(WhereCondition, LikeColumnList, SearchValue, SortBy, SortingOrder, PageNumber, RowLimit, session);
        }
        public static IList GetPaymentReceivedList(List<string[]> WhereCondition = null, ISession session = null)
        {
            return new PaymentReceivedService().GetPaymentReceivedList(WhereCondition, session);
        }
    }
}

