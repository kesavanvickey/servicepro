using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class WorkOrderMasterMap : ClassMap<WorkOrderMaster>
    {
        public WorkOrderMasterMap()
        {
            Id(e => e.WorkOrderMaster_ID);
            Map(e => e.ServiceItemDetail_ID);
            Map(e => e.WorkCodeTemplate);
            Map(e => e.EmployeeMaster_ID);
            Map(e => e.ServiceStartDate);
            Map(e => e.ServiceEndDate);
            Map(e => e.IsActive);
            Map(e => e.Created_UserID);
            Map(e => e.Modified_UserID);
            Map(e => e.Modified_DateTime);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Table("ServicePro.WorkOrderMaster");
        }
    }
}
