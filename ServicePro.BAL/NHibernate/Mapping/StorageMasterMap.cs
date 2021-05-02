using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class StorageMasterMap : ClassMap<StorageMaster>
    {
        public StorageMasterMap()
        {
            Id(e => e.StorageMaster_ID);
            Map(e => e.CodeTemplate);
            Map(e => e.Ref_ID);
            Map(e => e.FileName);
            Map(e => e.StorageType);
            Map(e => e.Extension);
            Map(e => e.ContentType);
            Map(e => e.StorageMaster_Data).CustomSqlType("VARBINARY (MAX) FILESTREAM")
                                          .Length(2147483647)
                                          .Not.Nullable();
            Map(e => e.FileSize);
            Map(e => e.IsActive);
            Map(e => e.Created_UserID);
            Map(e => e.Modified_UserID);
            Map(e => e.Modified_DateTime);
            Map(e => e.AddCol_1);
            Map(e => e.AddCol_2);
            Map(e => e.AddCol_3);
            Map(e => e.AddCol_4);
            Map(e => e.AddCol_5);
            Table("ServicePro.StorageMaster");
        }
    }
}
