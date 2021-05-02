using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using ServicePro.BAL;

namespace ServicePro.BAL
{
    internal class TypeMasterMap : ClassMap<TypeMaster>
    {
        public TypeMasterMap()
        {
            Id(x => x.TypeMaster_ID);
            Map(x => x.TypeMasterName);
            Map(x => x.ShortName);
            Map(x => x.Parent_ID);
            Map(x => x.Description);
            Map(x => x.CompanyMaster_ID);
            Map(x => x.Modified_DateTime);
            Map(x => x.IsActive);
            Table("Master.TypeMaster");
        }
    }
}