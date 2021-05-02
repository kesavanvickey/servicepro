using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ServicePro.BAL;

namespace ServicePro.BAL
{
    internal class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            string DBConnection = ConfigurationManager.ConnectionStrings["ServiceProConnection"].ConnectionString;
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                  .ConnectionString(DBConnection)
                              .ShowSql()
                )
               .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<MasterControl>()
                              .AddFromAssemblyOf<CompanyMaster>()
                              .AddFromAssemblyOf<TypeMaster>()
                              .AddFromAssemblyOf<TypeDetailMaster>()
                              .AddFromAssemblyOf<AdditionalColumnMaster>()
                              .AddFromAssemblyOf<EmployeeMaster>()
                              .AddFromAssemblyOf<TimeSheet>()
                              .AddFromAssemblyOf<UserMaster>()
                              .AddFromAssemblyOf<CustomerMaster>()
                              .AddFromAssemblyOf<AddressMaster>()
                              .AddFromAssemblyOf<IDProofMaster>()
                              .AddFromAssemblyOf<StorageMaster>()
                              .AddFromAssemblyOf<ServiceItemMaster>()
                              .AddFromAssemblyOf<ServiceItemDetail>()
                              .AddFromAssemblyOf<PaymentReceivable>()
                              .AddFromAssemblyOf<WorkOrderMaster>()
                              .AddFromAssemblyOf<PaymentReceived>()
                              .AddFromAssemblyOf<ItemReceivedHandler>()
                              .AddFromAssemblyOf<Invoice>()
                              .AddFromAssemblyOf<InvoiceDetail>()
                         )
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                                                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}