using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicePro.BAL;
using FluentNHibernate.Mapping;

namespace ServicePro.BAL
{
    internal class UserMasterMap : ClassMap<UserMaster>
    {
        public UserMasterMap()
        {
            Id(e => e.UserMaster_ID);
            Map(e => e.EmployeeMaster_ID);
            Map(e => e.UserName);
            Map(e => e.Password);
            Map(e => e.RollType);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Map(e => e.IsActive);
            Map(e => e.Modified_DateTime);
            Table("ServicePro.UserMaster");
        }
    }
}