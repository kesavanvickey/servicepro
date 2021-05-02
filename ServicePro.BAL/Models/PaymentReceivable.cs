using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class PaymentReceivable
    {
        //Database models
        public virtual int PaymentTotal_ID { get; set; }
        public virtual int PaymentCodeTemplate { get; set; }
        public virtual int ServiceItemDetail_ID { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual int IsActive { get; set; }
        public virtual string Created_UserID { get; set; }
        public virtual string Modified_UserID { get; set; }
        public virtual DateTime? Modified_DateTime { get; set; }
        public virtual string AddCol_1 { get; set; }
        public virtual string AddCol_2 { get; set; }
        public virtual string AddCol_3 { get; set; }
        public virtual string AddCol_4 { get; set; }
        public virtual string AddCol_5 { get; set; }

        //Non-Database models
        public virtual string PaymentCodeTemplateName { get; set; }



        public static bool Save(PaymentReceivable paymentReceivable)
        {
            return new PaymentReceivableService().SavePaymentReceivable(paymentReceivable);
        }
        public static bool Delete(int Id)
        {
            return new PaymentReceivableService().DeletePaymentReceivable(Id);
        }
        public static PaymentReceivable Get(int PrimaryKey, ISession session = null)
        {
            return new PaymentReceivableService().GetPaymentReceivable(PrimaryKey, session);
        }
        public static PaymentReceivable GetNew()
        {
            return new PaymentReceivableService().GetNewPaymentReceivable();
        }
        public static Decimal GetPaymentBalance(List<string[]> WhereCondition = null, ISession session = null)
        {
            return new PaymentReceivableService().GetPaymentBalance(WhereCondition, session);
        }
    }
}
