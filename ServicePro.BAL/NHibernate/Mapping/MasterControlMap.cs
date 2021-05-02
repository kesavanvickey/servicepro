using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicePro.BAL;
using FluentNHibernate.Mapping;

namespace ServicePro.BAL
{
    internal class MasterControlMap : ClassMap<MasterControl>
    {
        public MasterControlMap()
        {
            Id(x => x.ModifierLineNo);
            Map(x => x.ActivationMaster_Key);
            Map(x => x.CompanyName);
            Map(x => x.InstalledBy);
            Map(x => x.IsActive);
            Table("Master.MasterControl");
        }
    }
}