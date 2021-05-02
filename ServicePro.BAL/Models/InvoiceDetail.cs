using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    public class InvoiceDetail
    {
        public virtual int InvoiceDetail_ID { get; set; }
        public virtual int InvoiceID { get; set; }
        public virtual string Type { get; set; }
        public virtual string Comments { get; set; }
        public virtual string StatusType { get; set; }
        public virtual string Amount { get; set; }
        public virtual string PaymentType { get; set; }
        public virtual string RefNo { get; set; }
        public virtual string ReceivedDateTime { get; set; }
    }
}

