using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using ServicePro.BAL;

namespace ServicePro.BAL
{
    internal class TimeSheetMap : ClassMap<TimeSheet>
    {
        public TimeSheetMap()
        {
            Id(e => e.TimeSheet_ID);
            Map(e => e.EmployeeMaster_ID);
            Map(e => e.TimeSheet_Date);
            Map(e => e.CheckInDateTime);
            Map(e => e.CheckOutDateTime);
            Map(e => e.IsActive);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Table("ServiePro.TimeSheet");
        }
    }
}