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
    internal class PaymentReceivedService
    {
        #region PaymentReceived
        public PaymentReceived SavePaymentReceived(PaymentReceived paymentReceived)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (paymentReceived.PaymentReceived_ID > 0)
                        {
                            session.SaveOrUpdate(paymentReceived);
                        }
                        else
                        {
                            session.Save(paymentReceived);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            paymentReceived.SaveSuccess = true;
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
            return paymentReceived;
        }
        public bool DeletePaymentReceived(PaymentReceived paymentReceived)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    if (paymentReceived != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            paymentReceived.IsActive = 0;

                            session.Delete(paymentReceived);
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
        public PaymentReceived GetPaymentReceived(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            PaymentReceived paymentReceived = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                paymentReceived = session.Get<PaymentReceived>(PrimaryKey);
                if (paymentReceived.PaymentReceived_ID > 0)
                {
                    paymentReceived = SetPaymentReceived(paymentReceived, session);
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
            return paymentReceived;
        }
        public PaymentReceived GetNewPaymentReceived()
        {
            PaymentReceived paymentReceived = new PaymentReceived();
            paymentReceived.IsActive = 1;
            return paymentReceived;
        }
        protected PaymentReceived SetPaymentReceived(PaymentReceived paymentReceived, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster paymentType = TypeDetailMaster.Get(paymentReceived.PaymentType, session);
                    paymentReceived.PaymentTypeName = paymentType.TypeName;

                    //PaymentReceivable paymentReceivable = PaymentReceivable.Get(paymentReceived.PaymentTotal_ID, session);

                    paymentReceived.ServiceItemMaster = ServiceItemMaster.Get(paymentReceived.ServiceItemMaster_ID, session);

                    ////for comments trigger
                    //paymentReceived.Comments = serviceItemDetail.ServiceItemDetail_ID;


                    List<string[]> where = new List<string[]>();
                    where.Add(new string[] { "ServiceItemMaster_ID", paymentReceived.ServiceItemMaster_ID.ToString() });
                    where.Add(new string[] { "IsActive", "1" });
                    paymentReceived.ItemReceivedHandler = Convert.ToInt32("0" + Common.GetSingleColumnValue("ServicePro.ItemReceivedHandler", "ItemReceivedHandler_ID", where, session));

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
            return paymentReceived;
        }
        public IList GetPaymentReceivedGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = "select top " + top + " PaymentReceived.PaymentReceived_ID,";
                    sql += @" PaymentReceived.Amount,";
                    sql += @" TypeDetailMaster.TypeName as PaymentType,";
                    sql += @" CONVERT(VARCHAR,PaymentReceived.ReceivedDateTime) as ReceivedDateTime,";
                    sql += @" ServiceItemMaster.Brand +'-'+ ServiceItemMaster.Model as Item,";
                    sql += @" customerCodeDetail.TypeName +''+ CONVERT(VARCHAR,CustomerMaster.CustomerCode) +' - '+ CustomerMaster.CustomerName as Customer,";
                    sql += @" EmployeeMaster.EmployeeName as PaymentReceivedBy";
                    sql += @" from ServicePro.PaymentReceived";
                    sql += @" left outer join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = PaymentReceived.ServiceItemMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = PaymentReceived.PaymentType";
                    sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = PaymentReceived.PaymentReceivedBy";
                    sql += @" inner join Master.TypeDetailMaster as customerCodeDetail on customerCodeDetail.TypeDetailMaster_ID = CustomerMaster.CustomerCodeTemplate";

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
                    sql += "select top " + except + " PaymentReceived.PaymentReceived_ID,";
                    sql += @" PaymentReceived.Amount,";
                    sql += @" TypeDetailMaster.TypeName as PaymentType,";
                    sql += @" CONVERT(VARCHAR,PaymentReceived.ReceivedDateTime) as ReceivedDateTime,";
                    sql += @" ServiceItemMaster.Brand +'-'+ ServiceItemMaster.Model as Item,";
                    sql += @" customerCodeDetail.TypeName +''+ CONVERT(VARCHAR,CustomerMaster.CustomerCode) +' - '+ CustomerMaster.CustomerName as Customer,";
                    sql += @" EmployeeMaster.EmployeeName as PaymentReceivedBy";
                    sql += @" from ServicePro.PaymentReceived";
                    sql += @" left outer join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = PaymentReceived.ServiceItemMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = PaymentReceived.PaymentType";
                    sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = PaymentReceived.PaymentReceivedBy";
                    sql += @" inner join Master.TypeDetailMaster as customerCodeDetail on customerCodeDetail.TypeDetailMaster_ID = CustomerMaster.CustomerCodeTemplate";

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
        public int GetPaymentReceivedGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = "select PaymentReceived.PaymentReceived_ID,";
                    sql += @" PaymentReceived.Amount,";
                    sql += @" TypeDetailMaster.TypeName as PaymentType,";
                    sql += @" CONVERT(VARCHAR,PaymentReceived.ReceivedDateTime) as ReceivedDateTime,";
                    sql += @" ServiceItemMaster.Brand +'-'+ ServiceItemMaster.Model as Item,";
                    sql += @" customerCodeDetail.TypeName +''+ CONVERT(VARCHAR,CustomerMaster.CustomerCode) +' - '+ CustomerMaster.CustomerName as Customer,";
                    sql += @" EmployeeMaster.EmployeeName as PaymentReceivedBy";
                    sql += @" from ServicePro.PaymentReceived";
                    sql += @" left outer join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = PaymentReceived.ServiceItemMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = PaymentReceived.PaymentType";
                    sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = PaymentReceived.PaymentReceivedBy";
                    sql += @" inner join Master.TypeDetailMaster as customerCodeDetail on customerCodeDetail.TypeDetailMaster_ID = CustomerMaster.CustomerCodeTemplate";

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
        public IList GetPaymentReceivedList(List<string[]> WhereCondition = null, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                //var sql = "select PaymentReceivable.Amount as Total,";
                //sql += @" ISNULL((select SUM(tempAS.Amount) from ServicePro.PaymentReceived as tempAS where tempAS.PaymentTotal_ID = PaymentReceivable.PaymentTotal_ID and tempAS.IsActive = '1'),0) as PaidAmountTotal,";
                //sql += @" PaymentReceivable.Amount - CONVERT(NUMERIC,(ISNULL((select SUM(tempAS.Amount) from ServicePro.PaymentReceived as tempAS where tempAS.PaymentTotal_ID = PaymentReceivable.PaymentTotal_ID and tempAS.IsActive = '1'),0))) as Balance,";
                //sql += @" PaymentReceived.PaymentReferenceNo,";
                //sql += @" PaymentReceived.PaymentReceived_ID,";
                //sql += @" CONVERT(VARCHAR,PaymentReceived.ReceivedDateTime) AS ReceivedDateTime,";
                //sql += @" PaymentReceived.Amount as PaidAmount,";
                //sql += @" CustomerMaster.CustomerName,";
                //sql += @" EmployeeMaster.EmployeeName,";
                //sql += @" PaymentReceivable.PaymentTotal_ID,";
                //sql += @" TypeDetailMaster.TypeName as PaymentType,";
                //sql += @" PaymentReceived.IsActive";
                //sql += @" from ServicePro.ServiceItemDetail";
                //sql += @" inner join ServicePro.PaymentReceivable on PaymentReceivable.ServiceItemDetail_ID = ServiceItemDetail.ServiceItemDetail_ID";
                //sql += @" left outer join ServicePro.PaymentReceived on PaymentReceivable.PaymentTotal_ID = PaymentReceived.PaymentTotal_ID";
                //sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ServiceItemDetail.ServiceItemMaster_ID";
                //sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                //sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = ServiceItemMaster.EmployeeMaster_ID";
                //sql += @" left outer join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = PaymentReceived.PaymentType";

                var sql = " select ISNULL(PaymentReceivable.Amount, 0) as ReceivableAmount,";
                sql += @" ISNULL(SUM(PaymentReceived.Amount), 0) as ReceivedAmount,";
                sql += @" ServiceItemDetail.Comments,";
                sql += @" TypeDetailMaster.TypeName as StatusType,";
                sql += @" CustomerMaster.CustomerName,";
                sql += @" ServiceItemMaster.Brand +' '+ ServiceItemMaster.Model as ServiceItem";
                sql += @" from ServicePro.ServiceItemDetail";
                sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ServiceItemDetail.ServiceItemMaster_ID";
                sql += @" inner join ServicePro.PaymentReceivable on PaymentReceivable.ServiceItemDetail_ID = ServiceItemDetail.ServiceItemDetail_ID";
                sql += @" left outer join ServicePro.PaymentReceived on PaymentReceived.ServiceItemMaster_ID = ServiceItemMaster.ServiceItemMaster_ID";
                sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemDetail.StatusType";
                sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";

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

                sql += @" GROUP BY ServiceItemDetail.Comments, PaymentReceivable.Amount, TypeDetailMaster.TypeName,";
                sql += @" CustomerMaster.CustomerName, ServiceItemMaster.Brand, ServiceItemMaster.Model";

                IQuery iquery = session.CreateSQLQuery(sql);
                list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
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
        #endregion
    }
}
