using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class PaymentReceivableMap : ClassMap<PaymentReceivable>
    {
        public PaymentReceivableMap()
        {
            Id(e => e.PaymentTotal_ID);
            Map(e => e.PaymentCodeTemplate);
            Map(e => e.ServiceItemDetail_ID);
            Map(e => e.Amount);
            Map(e => e.IsActive);
            Map(e => e.Created_UserID);
            Map(e => e.Modified_UserID);
            Map(e => e.Modified_DateTime);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Table("ServicePro.PaymentReceivable");
        }
    }
}
