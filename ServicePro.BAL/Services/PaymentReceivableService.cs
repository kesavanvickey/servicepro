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
    internal class PaymentReceivableService
    {
        #region PaymentReceivable
        public bool SavePaymentReceivable(PaymentReceivable paymentReceivable)
        {
            bool save = false;
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (paymentReceivable.PaymentTotal_ID > 0)
                        {
                            session.SaveOrUpdate(paymentReceivable);
                        }
                        else
                        {
                            session.Save(paymentReceivable);
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
        public bool DeletePaymentReceivable(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    PaymentReceivable paymentReceivable = GetPaymentReceivable(Id, session);
                    if (paymentReceivable != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(paymentReceivable);
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
        public PaymentReceivable GetPaymentReceivable(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            PaymentReceivable paymentReceivable = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                paymentReceivable = session.Get<PaymentReceivable>(PrimaryKey);
                if (paymentReceivable.PaymentTotal_ID > 0)
                {
                    paymentReceivable = SetPaymentReceivable(paymentReceivable, session);
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
            return paymentReceivable;
        }
        public PaymentReceivable GetNewPaymentReceivable()
        {
            PaymentReceivable paymentReceivable = new PaymentReceivable();
            paymentReceivable.IsActive = 1;
            return paymentReceivable;
        }
        protected PaymentReceivable SetPaymentReceivable(PaymentReceivable paymentReceivable, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster paymentCodeTemplate = TypeDetailMaster.Get(paymentReceivable.PaymentCodeTemplate, session);
                    paymentReceivable.PaymentCodeTemplateName = paymentCodeTemplate.TypeName;
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
            return paymentReceivable;
        }
        public IList GetPaymentReceivableGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IList list = null;
            try
            {
                
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
        public int GetPaymentReceivableGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            int ReturnCount = 0;
            try
            {
                
                
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
        public Decimal GetPaymentBalance(List<string[]> WhereCondition = null, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            Decimal balance = 0;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                var sql = "select PaymentReceivable.Amount - ISNULL(SUM(PaymentReceived.Amount),0) as Balance";
                sql += @" from ServicePro.PaymentReceived";
                sql += @" right join ServicePro.PaymentReceivable on PaymentReceivable.PaymentTotal_ID = PaymentReceived.PaymentTotal_ID";

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

                sql += @" group by PaymentReceivable.Amount";

                IQuery iquery = session.CreateSQLQuery(sql);
                balance = iquery.UniqueResult<Decimal>();
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
            return balance;
        }
        #endregion
    }
}
