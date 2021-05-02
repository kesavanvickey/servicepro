using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class CustomerMasterMap : ClassMap<CustomerMaster>
    {
        public CustomerMasterMap()
        {
            Id(e => e.CustomerMaster_ID);
            Map(e => e.CustomerCodeTemplate);
            Map(e => e.CustomerCode);
            Map(e => e.CustomerName);
            Map(e => e.DOB);
            Map(e => e.Gender);
            Map(e => e.EmployeeMaster_ID);
            Map(e => e.IsActive);
            Map(e => e.Created_UserID);
            Map(e => e.Modified_UserID);
            Map(e => e.Modified_DateTime);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Table("ServicePro.CustomerMaster");
        }
    }
}
