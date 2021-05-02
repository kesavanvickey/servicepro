using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class ServiceItemDetailService
    {
        #region ServiceItemDetail
        public bool SaveServiceItemDetail(ServiceItemDetail serviceItemDetail)
        {
            bool save = false;
            ITransaction transaction = null;
            ISession session = null;
            PaymentReceivable obj = serviceItemDetail.PaymentReceivable;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (serviceItemDetail.ServiceItemDetail_ID > 0)
                        {
                            session.SaveOrUpdate(serviceItemDetail);
                        }
                        else
                        {
                            session.Save(serviceItemDetail);
                        }

                        obj.ServiceItemDetail_ID = serviceItemDetail.ServiceItemDetail_ID;

                        if (obj.PaymentTotal_ID > 0)
                        {
                            session.SaveOrUpdate(obj);
                        }
                        else
                        {
                            session.Save(obj);
                        }

                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            save = true;
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
            return save;
        }
        public bool DeleteServiceItemDetail(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    ServiceItemDetail serviceItemDetail = GetServiceItemDetail(Id, session);
                    PaymentReceivable paymentobj = serviceItemDetail.PaymentReceivable;
                    if (serviceItemDetail != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(paymentobj);
                            session.Delete(serviceItemDetail);
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
        public ServiceItemDetail GetServiceItemDetail(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ServiceItemDetail serviceItemDetail = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                serviceItemDetail = session.Get<ServiceItemDetail>(PrimaryKey);
                if (serviceItemDetail.ServiceItemDetail_ID > 0)
                {
                    serviceItemDetail = SetServiceItemDetail(serviceItemDetail, session);
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
            return serviceItemDetail;
        }
        public ServiceItemDetail GetNewServiceItemDetail()
        {
            ServiceItemDetail serviceItemDetail = new ServiceItemDetail();
            serviceItemDetail.IsActive = 1;
            return serviceItemDetail;
        }
        protected ServiceItemDetail SetServiceItemDetail(ServiceItemDetail serviceItemDetail, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    List<string[]> list = new List<string[]>();
                    list.Add(new string[] { "ServiceItemDetail_ID", serviceItemDetail.ServiceItemDetail_ID.ToString() });
                    serviceItemDetail.PaymentReceivable = PaymentReceivable.Get(Convert.ToInt32("0" + Common.GetSingleColumnValue("ServicePro.PaymentReceivable", "PaymentTotal_ID", list, session)));
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
            return serviceItemDetail;
        }
        public IList GetServiceItemDetailGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = "select ServiceItemDetail.ServiceItemDetail_ID,";
                    sql += @" PaymentTotal_ID,";
                    sql += @" Comments,";
                    sql += @" Amount,";
                    sql += @" TypeDetailMaster.TypeName as Status,";
                    sql += @" ServiceItemDetail.StatusType,";
                    sql += @" ServiceItemDetail.IsActive";
                    sql += @" from ServicePro.ServiceItemDetail";
                    sql += @" inner join ServicePro.PaymentReceivable on PaymentReceivable.ServiceItemDetail_ID = ServiceItemDetail.ServiceItemDetail_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemDetail.StatusType";

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
                                sql += @"" + ColumnName + " = '" + ColumnValue + "' ";
                            }
                            if (checkCount != count)
                            {
                                sql += @" AND ";
                                checkCount++;
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
        public IList GetServiceItemDetailForWorkOrder(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = "select ServiceItemDetail.ServiceItemDetail_ID,";
                    sql += @" ServiceItemDetail.Comments,";
                    sql += @" ServiceItemDetail.StatusType,";
                    sql += @" TypeDetailMaster.TypeName as Status";
                    sql += @" from ServicePro.ServiceItemDetail";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemDetail.StatusType";

                    sql += @" WHERE ServiceItemDetail.IsActive = '" + 1 + "'";

                    if (WhereCondition.Count > 0)
                    {
                        sql += @" AND ";
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
        public bool DeleteValidationForPayment(int serviceItemDetailID, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            bool allow = true;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    var sql = "select CONVERT(DECIMAL,ISNULL(SUM(ISNULL(Amount,0)),0)) as TotalItemAmount,";
                    sql += @" CONVERT(DECIMAL,(select ISNULL(Amount,0) from ServicePro.PaymentReceivable where ServiceItemDetail_ID = '" + serviceItemDetailID + "')) as ItemAmount";
                    sql += @" from ServicePro.ServiceItemDetail";
                    sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ServiceItemDetail.ServiceItemMaster_ID";
                    sql += @" inner join ServicePro.PaymentReceivable on PaymentReceivable.ServiceItemDetail_ID = ServiceItemDetail.ServiceItemDetail_ID";
                    sql += @" where ServiceItemMaster.ServiceItemMaster_ID =";
                    sql += @" (select ServiceItemMaster_ID from ServicePro.ServiceItemDetail where ServiceItemDetail.ServiceItemDetail_ID = '" + serviceItemDetailID + "')";
                    IQuery iquery = session.CreateSQLQuery(sql);
                    Hashtable ht = (Hashtable)iquery.SetResultTransformer(Transformers.AliasToEntityMap).List()[0];

                    sql = "";
                    sql += @" select ISNULL(SUM(ISNULL(Amount,0)),0) as PaidAmount from ServicePro.PaymentReceived";
                    sql += @" inner join ServicePro.ServiceItemMaster on PaymentReceived.ServiceItemMaster_ID = ServiceItemMaster.ServiceItemMaster_ID";
                    sql += @" where ServiceItemMaster.ServiceItemMaster_ID =";
                    sql += @" (select ServiceItemMaster_ID from ServicePro.ServiceItemDetail where ServiceItemDetail.ServiceItemDetail_ID = '" + serviceItemDetailID + "')";
                    IQuery iquery2 = session.CreateSQLQuery(sql);
                    Decimal paidAmount = iquery2.UniqueResult<Decimal>();

                    Decimal calculate = Convert.ToDecimal("0" + ht["TotalItemAmount"]) - paidAmount;
                    if(calculate < Convert.ToDecimal("0" + ht["ItemAmount"]))
                    {
                        allow = false;
                    }
                }
            }
            catch (Exception ex)
            {
                allow = false;
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return allow;
        }
        public Decimal GetPaidAmoutByItemDetailId(int serviceItemDetailID, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            Decimal amount = 0;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    string sql = " select ISNULL(SUM(ISNULL(Amount,0)),0) as PaidAmount from ServicePro.PaymentReceived";
                    sql += @" inner join ServicePro.ServiceItemMaster on PaymentReceived.ServiceItemMaster_ID = ServiceItemMaster.ServiceItemMaster_ID";
                    sql += @" where ServiceItemMaster.ServiceItemMaster_ID =";
                    sql += @" (select ServiceItemMaster_ID from ServicePro.ServiceItemDetail where ServiceItemDetail.ServiceItemDetail_ID = '" + serviceItemDetailID + "')";
                    IQuery iquery2 = session.CreateSQLQuery(sql);
                    amount = iquery2.UniqueResult<Decimal>();
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
            return amount;
        }
        #endregion
    }
}
