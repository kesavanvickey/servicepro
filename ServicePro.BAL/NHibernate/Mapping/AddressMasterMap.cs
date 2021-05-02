using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class AddressMasterMap : ClassMap<AddressMaster>
    {
        public AddressMasterMap()
        {
            Id(e => e.AddressMaster_ID);
            Map(e => e.CodeTemplate);
            Map(e => e.Ref_ID);
            Map(e => e.AddressType);
            Map(e => e.Address1);
            Map(e => e.Address2);
            Map(e => e.Address3);
            Map(e => e.City);
            Map(e => e.State);
            Map(e => e.Country);
            Map(e => e.Pincode);
            Map(e => e.ContactNo1);
            Map(e => e.ContactNo2);
            Map(e => e.Email);
            Map(e => e.IsActive);
            Map(e => e.Created_UserID);
            Map(e => e.Modified_UserID);
            Map(e => e.Modified_DateTime);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Table("ServicePro.AddressMaster");
        }
    }
}
