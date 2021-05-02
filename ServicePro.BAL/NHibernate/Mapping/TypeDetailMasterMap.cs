using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicePro.BAL;
using FluentNHibernate.Mapping;

namespace ServicePro.BAL
{
    internal class TypeDetailMasterMap : ClassMap<TypeDetailMaster>
    {
        public TypeDetailMasterMap()
        {
            Id(x => x.TypeDetailMaster_ID);
            Map(x => x.TypeName);
            Map(x => x.Description);
            Map(x => x.CompanyMaster_ID);
            Map(x => x.IsActive);
            Map(x => x.TypeMaster_ID);
            Map(x => x.Modified_DateTime);
            Table("Master.TypeDetailMaster");
        }
    }
}