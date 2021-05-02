using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using ServicePro.BAL;

namespace ServicePro.BAL
{
    internal class EmployeeMasterMap : ClassMap<EmployeeMaster>
    {
        public EmployeeMasterMap()
        {
            Id(e => e.EmployeeMaster_ID);
            Map(e => e.EmployeeCode);
            Map(e => e.EmployeeCodeTemplate);
            Map(e => e.EmployeeName);
            Map(e => e.EmployeeType);
            Map(e => e.Gender);
            Map(e => e.JointDate);
            Map(e => e.DOB);
            Map(e => e.CompanyMaster_ID);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Map(e => e.IsActive);
            Map(e => e.Modified_DateTime);
            Table("Master.EmployeeMaster");
        }
    }
}