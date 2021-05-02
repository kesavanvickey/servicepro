using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using ServicePro.BAL;

namespace ServicePro.BAL
{
    internal class AdditionalColumnMasterMap : ClassMap<AdditionalColumnMaster>
    {
        public AdditionalColumnMasterMap()
        {
            Id(x => x.AdditionalColumnMaster_ID);
            Map(x => x.AdditionalColumnName);
            Map(x => x.CompanyMaster_ID);
            Map(x => x.DataType);
            Map(x => x.DisplayName);
            Map(x => x.IsActive);
            Map(x => x.Modified_DateTime);
            Map(x => x.TableName);
            Table("Master.AdditionalColumnMaster");
        }
    }
}