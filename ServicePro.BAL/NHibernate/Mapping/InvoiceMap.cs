using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class InvoiceMap : ClassMap<Invoice>
    {
        public InvoiceMap()
        {
            Id(e => e.InvoiceID);
            Map(e => e.CompanyName);
            Map(e => e.CompanyAddress);
            Map(e => e.CompanyContactNo);
            Map(e => e.CustomerId);
            Map(e => e.ServiceItemId);
            Map(e => e.PrintDateTime);
            Map(e => e.CustomerName);
            Map(e => e.ItemName);
            Map(e => e.ItemReceivedDateTime);
            Map(e => e.ItemDeliverDateTime);
            Map(e => e.ItemDetailTotalAmount);
            Map(e => e.Created_UserId);
            Map(e => e.PaidAmount);
            Map(e => e.Balance);
            Map(e => e.DeliveredDateTime);
            Table("ServicePro.Invoice");
        }
    }
}
