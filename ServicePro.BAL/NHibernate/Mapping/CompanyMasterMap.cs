using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicePro.BAL;
using FluentNHibernate.Mapping;

namespace ServicePro.BAL
{
    internal class CompanyMasterMap : ClassMap<CompanyMaster>
    {
        public CompanyMasterMap()
        {
            Id(x => x.CompanyMaster_ID);
            Map(x => x.CompanyName);
            Map(x => x.CompanyType);
            Map(x => x.TinNo);
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.Recovery_Mobile);
            Map(x => x.Recovery_Email);
            Map(x => x.Recovery_Question);
            Map(x => x.Recovery_Answer);
            Map(x => x.ActivationMaster_Key);
            Map(x => x.Modified_DateTime);
            Map(x => x.IsActive);
            Map(x => x.CompanyLogo);
            Map(x => x.ReportSignature);
            Map(x => x.ReportBottom);
            Table("Master.CompanyMaster");
        }
    }
}