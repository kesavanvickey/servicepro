using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class ServiceItemMasterMap : ClassMap<ServiceItemMaster>
    {
        public ServiceItemMasterMap()
        {
            Id(x => x.ServiceItemMaster_ID);
            Map(x => x.ServiceCodeTemplate);
            Map(x => x.CustomerMaster_ID);
            Map(x => x.EmployeeMaster_ID);
            Map(x => x.Brand);
            Map(x => x.Model);
            Map(e => e.ItemOrderDate);
            Map(e => e.ItemExpectedDeliverDate);
            Map(e => e.IsActive);
            Map(e => e.Created_UserID);
            Map(e => e.Modified_UserID);
            Map(e => e.Modified_DateTime);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Table("ServicePro.ServiceItemMaster");
        }
    }
}
