using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class InvoiceDetailMap : ClassMap<InvoiceDetail>
    {
        public InvoiceDetailMap()
        {
            Id(e => e.InvoiceDetail_ID);
            Map(e => e.InvoiceID);
            Map(e => e.Type);
            Map(e => e.Comments);
            Map(e => e.StatusType);
            Map(e => e.Amount);
            Map(e => e.ReceivedDateTime);
            Map(e => e.RefNo);
            Map(e => e.PaymentType);
            Table("ServicePro.InvoiceDetail");
        }
    }
}
