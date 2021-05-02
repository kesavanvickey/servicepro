using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class ServiceItemMasterService
    {
        #region ServiceItemMaster
        public ServiceItemMaster SaveServiceItemMaster(ServiceItemMaster serviceItemMaster)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (serviceItemMaster.ServiceItemMaster_ID > 0)
                        {
                            session.SaveOrUpdate(serviceItemMaster);
                        }
                        else
                        {
                            session.Save(serviceItemMaster);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            serviceItemMaster.ReturnValue = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
                transaction.Rollback();
            }
            finally
            {
                session.Dispose();
            }
            return serviceItemMaster;
        }
        public bool DeleteServiceItemMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    ServiceItemMaster serviceItemMaster = GetServiceItemMaster(Id, session);
                    if (serviceItemMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(serviceItemMaster);
                            transaction.Commit();
                            if (transaction.WasCommitted == true)
                            {
                                deleteSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                session.Dispose();
            }
            return deleteSuccess;
        }
        public ServiceItemMaster GetServiceItemMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ServiceItemMaster serviceItemMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                serviceItemMaster = session.Get<ServiceItemMaster>(PrimaryKey);
                if (serviceItemMaster.ServiceItemMaster_ID > 0)
                {
                    serviceItemMaster = SetServiceItemMaster(serviceItemMaster, session);
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return serviceItemMaster;
        }
        public ServiceItemMaster GetNewServiceItemMaster()
        {
            ServiceItemMaster serviceItemMaster = new ServiceItemMaster();
            serviceItemMaster.IsActive = 1;
            return serviceItemMaster;
        }
        protected ServiceItemMaster SetServiceItemMaster(ServiceItemMaster serviceItemMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster serviceCodeTemplateName = TypeDetailMaster.Get(serviceItemMaster.ServiceCodeTemplate, session);
                    serviceItemMaster.ServiceCodeTemplateName = serviceCodeTemplateName.TypeName;

                    List<string[]> where = new List<string[]>();
                    where.Add(new string[] { "ServiceItemMaster_ID", serviceItemMaster.ServiceItemMaster_ID.ToString() });
                    where.Add(new string[] { "IsActive", "1" });
                    serviceItemMaster.ItemReceivedHandler = Convert.ToInt32("0" + Common.GetSingleColumnValue("ServicePro.ItemReceivedHandler", "ItemReceivedHandler_ID", where, session));
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return serviceItemMaster;
        }
        public IList GetServiceItemMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    // Grid Select query Coding here

                    int top = Convert.ToInt32("0" + PageNumber) * Convert.ToInt32("0" + RowLimit);

                    var sql = "select top " + top + " detailTemplate.TypeName as ServiceCodeTemplateName,";
                    sql += @" ServiceItemMaster_ID,";
                    sql += @" CustomerJoinTypeDetail.TypeName +''+ CONVERT(varchar,CustomerJoinServiceItem.CustomerCode) +'-'+  CustomerJoinServiceItem.CustomerName as Customer,";
                    sql += @" Brand,";
                    sql += @" Model,";
                    sql += @" CONVERT(VARCHAR,ItemOrderDate) AS ItemOrderDate,";
                    sql += @" CONVERT(VARCHAR,ItemExpectedDeliverDate) AS ItemExpectedDeliverDate,";
                    sql += @" ServiceItemMaster.IsActive";
                    sql += @" from ServicePro.ServiceItemMaster ";
                    sql += @" inner join Master.TypeDetailMaster as detailTemplate on detailTemplate.TypeDetailMaster_ID = ServiceItemMaster.ServiceCodeTemplate";
                    sql += @" inner join ServicePro.CustomerMaster as CustomerJoinServiceItem on CustomerJoinServiceItem.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = ServiceItemMaster.EmployeeMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster as CustomerJoinTypeDetail on CustomerJoinTypeDetail.TypeDetailMaster_ID = CustomerJoinServiceItem.CustomerCodeTemplate";

                    if (SearchColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        sql += @" WHERE ";
                        int ListCount = SearchColumnList.Count;
                        int i = 1;
                        foreach (var like in SearchColumnList)
                        {
                            sql += @" " + like[0] + " LIKE '%" + SearchValue + "%' ";
                            if (i != ListCount)
                            {
                                sql += @" OR ";
                                i++;
                            }
                        }
                    }

                    sql += @" EXCEPT ";
                    var except = top - RowLimit;
                    sql += "select top " + except + " detailTemplate.TypeName as ServiceCodeTemplateName,";
                    sql += @" ServiceItemMaster_ID,";
                    sql += @" CustomerJoinTypeDetail.TypeName +''+ CONVERT(varchar,CustomerJoinServiceItem.CustomerCode) +'-'+  CustomerJoinServiceItem.CustomerName as Customer,";
                    sql += @" Brand,";
                    sql += @" Model,";
                    sql += @" CONVERT(VARCHAR,ItemOrderDate) AS ItemOrderDate,";
                    sql += @" CONVERT(VARCHAR,ItemExpectedDeliverDate) AS ItemExpectedDeliverDate,";
                    sql += @" ServiceItemMaster.IsActive";
                    sql += @" from ServicePro.ServiceItemMaster ";
                    sql += @" inner join Master.TypeDetailMaster as detailTemplate on detailTemplate.TypeDetailMaster_ID = ServiceItemMaster.ServiceCodeTemplate";
                    sql += @" inner join ServicePro.CustomerMaster as CustomerJoinServiceItem on CustomerJoinServiceItem.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = ServiceItemMaster.EmployeeMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster as CustomerJoinTypeDetail on CustomerJoinTypeDetail.TypeDetailMaster_ID = CustomerJoinServiceItem.CustomerCodeTemplate";

                    if (SearchColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        sql += @" WHERE ";
                        int ListCount = SearchColumnList.Count;
                        int i = 1;
                        foreach (var like in SearchColumnList)
                        {
                            sql += @" " + like[0] + " LIKE '%" + SearchValue + "%' ";
                            if (i != ListCount)
                            {
                                sql += @" OR ";
                                i++;
                            }
                        }
                    }

                    if (SortBy != null && SortingOrder != null)
                    {
                        sql += @" ORDER BY " + SortBy + " " + SortingOrder + " ";
                    }
                    IQuery iquery = session.CreateSQLQuery(sql);
                    list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return list;
        }
        public int GetServiceItemMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            int ReturnCount = 0;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    //Add Grid Count coding here

                    var sql = "select detailTemplate.TypeName as ServiceCodeTemplateName,";
                    sql += @" ServiceItemMaster_ID,";
                    sql += @" CustomerJoinTypeDetail.TypeName +''+ CONVERT(varchar,CustomerJoinServiceItem.CustomerCode) +'-'+  CustomerJoinServiceItem.CustomerName as Customer,";
                    sql += @" Brand,";
                    sql += @" Model,";
                    sql += @" CONVERT(VARCHAR,ItemOrderDate) AS ItemOrderDate,";
                    sql += @" CONVERT(VARCHAR,ItemExpectedDeliverDate) AS ItemExpectedDeliverDate,";
                    sql += @" ServiceItemMaster.IsActive";
                    sql += @" from ServicePro.ServiceItemMaster ";
                    sql += @" inner join Master.TypeDetailMaster as detailTemplate on detailTemplate.TypeDetailMaster_ID = ServiceItemMaster.ServiceCodeTemplate";
                    sql += @" inner join ServicePro.CustomerMaster as CustomerJoinServiceItem on CustomerJoinServiceItem.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = ServiceItemMaster.EmployeeMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster as CustomerJoinTypeDetail on CustomerJoinTypeDetail.TypeDetailMaster_ID = CustomerJoinServiceItem.CustomerCodeTemplate";

                    if (LikeColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        sql += @" WHERE ";
                        int ListCount = LikeColumnList.Count;
                        int i = 1;
                        foreach (var like in LikeColumnList)
                        {
                            sql += @" " + like[0] + " LIKE '%" + SearchValue + "%' ";
                            if (i != ListCount)
                            {
                                sql += @" OR ";
                                i++;
                            }
                        }
                    }

                    IQuery iquery = session.CreateSQLQuery(sql);
                    IList list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                    ReturnCount = list.Count;
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return ReturnCount;
        }
        public DataTable GetServiceItemMasterCboByStatus(string StatusTypeName, List<string[]> WhereCondition = null, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    // Grid Select query Coding here

                    var sql = "select distinct ServiceItemMaster.ServiceItemMaster_ID,";
                    sql += @" detailServiceTemplate.TypeName +''+ CONVERT(VARCHAR,ServiceItemMaster.ServiceItemMaster_ID) +' - '+ ServiceItemMaster.Brand +' '+ ServiceItemMaster.Model +' | '+ CustomerMaster.CustomerName as ServiceItem";
                    sql += @" from ServicePro.ServiceItemMaster";
                    sql += @" inner join ServicePro.ServiceItemDetail on ServiceItemDetail.ServiceItemMaster_ID = ServiceItemMaster.ServiceItemMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster as detailStatus on detailStatus.TypeDetailMaster_ID = ServiceItemDetail.StatusType";
                    sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster as detailServiceTemplate on detailServiceTemplate.TypeDetailMaster_ID = ServiceItemMaster.ServiceCodeTemplate";

                    if (WhereCondition != null)
                    {
                        if (WhereCondition.Count > 0)
                        {
                            sql += @" WHERE ";
                            int count = WhereCondition.Count;
                            int checkCount = 1;
                            foreach (var where in WhereCondition)
                            {
                                for (var i = 1; i < where.Length; i++)
                                {
                                    string ColumnName = null;
                                    string ColumnValue = null;
                                    ColumnName = where[0];
                                    ColumnValue = where[1];
                                    sql += @" " + ColumnName + " = '" + ColumnValue + "' ";
                                }
                                if (checkCount != count)
                                {
                                    sql += @" AND ";
                                    checkCount++;
                                }
                            }
                        }
                    }

                    if (StatusTypeName != null)
                    {
                        if (WhereCondition.Count > 0)
                        {
                            sql += @" AND";
                        }
                        else
                        {
                            sql += @" WHERE";
                        }
                        sql += @" detailStatus.TypeName = '" + StatusTypeName + "'";
                    }

                    IQuery iquery = session.CreateSQLQuery(sql);
                    list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return CommonUtil.ConvertToDataTable(null, list);
        }
        public DataTable GetServiceItemMasterForCBO(List<string[]> WhereCondition = null, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    // Grid Select query Coding here

                    var sql = "select ServiceItemMaster_ID,";
                    sql += @" TypeDetailMaster.TypeName +''+ CONVERT(VARCHAR,ServiceItemMaster_ID) +' - '+ Brand +' '+ Model +' | '+ CustomerMaster.CustomerName as ServiceItem";
                    sql += @" from ServicePro.ServiceItemMaster";
                    sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemMaster.ServiceCodeTemplate";

                    if (WhereCondition != null)
                    {
                        if(WhereCondition.Count > 0)
                        {
                            sql += @" WHERE ";
                            int count = WhereCondition.Count;
                            int checkCount = 1;
                            foreach (var where in WhereCondition)
                            {
                                for (var i = 1; i < where.Length; i++)
                                {
                                    string ColumnName = null;
                                    string ColumnValue = null;
                                    ColumnName = where[0];
                                    ColumnValue = where[1];
                                    sql += @" " + ColumnName + " = '" + ColumnValue + "' ";
                                }
                                if (checkCount != count)
                                {
                                    sql += @" AND ";
                                    checkCount++;
                                }
                            }
                        }
                    }

                    IQuery iquery = session.CreateSQLQuery(sql);
                    list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return CommonUtil.ConvertToDataTable(null, list);
        }
        #endregion
    }
}
