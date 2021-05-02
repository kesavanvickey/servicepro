using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class Invoice
    {
        //DB Models
        public virtual int InvoiceID { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CompanyAddress { get; set; }
        public virtual string CompanyContactNo { get; set; }
        public virtual string CustomerId { get; set; }
        public virtual string ServiceItemId { get; set; }
        public virtual string PrintDateTime { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string ItemName { get; set; }
        public virtual string ItemReceivedDateTime { get; set; }
        public virtual string ItemDeliverDateTime { get; set; }
        public virtual string ItemDetailTotalAmount { get; set; }
        public virtual string PaidAmount { get; set; }
        public virtual string Balance { get; set; }
        public virtual string DeliveredDateTime { get; set; }
        public virtual string Created_UserId { get; set; }


        //Non DB
        public virtual int ReturnValue { get; set; }
        public virtual IList<InvoiceDetail> InvoiceDetail { get; set; }


        public static Invoice Save(Invoice invoice, ISession session = null)
        {
            return new InvoiceService().SaveInvoice(invoice, session);
        }
        public static bool Delete(int Id)
        {
            return new InvoiceService().DeleteInvoice(Id);
        }
        public static Invoice Get(int PrimaryKey, ISession session = null)
        {
            return new InvoiceService().GetInvoice(PrimaryKey, session);
        }
    }
}
