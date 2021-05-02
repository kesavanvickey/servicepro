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
    internal class CustomerMasterService
    {
        #region CustomerMaster
        public CustomerMaster SaveCustomerMaster(CustomerMaster customerMaster)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (customerMaster.CustomerMaster_ID > 0)
                        {
                            session.SaveOrUpdate(customerMaster);
                        }
                        else
                        {
                            session.Save(customerMaster);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            customerMaster.ReturnValue = 2;
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
            return customerMaster;
        }
        public bool DeleteCustomerMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    CustomerMaster customerMaster = GetCustomerMaster(Id, session);
                    if (customerMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(customerMaster);
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
        public CustomerMaster GetCustomerMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            CustomerMaster customerMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                customerMaster = session.Get<CustomerMaster>(PrimaryKey);
                if (customerMaster.CustomerMaster_ID > 0)
                {
                    customerMaster = SetCustomerMaster(customerMaster, session);
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
            return customerMaster;
        }
        public CustomerMaster GetNewCustomerMaster()
        {
            CustomerMaster customerMaster = new CustomerMaster();
            customerMaster.IsActive = 1;
            return customerMaster;
        }
        protected CustomerMaster SetCustomerMaster(CustomerMaster customerMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster customerCodeName = TypeDetailMaster.Get(customerMaster.CustomerCodeTemplate, session);
                    customerMaster.CustomerCodeTemplateName = customerCodeName.TypeName;

                    customerMaster.CustomerCodeFullName = customerCodeName.TypeName.ToString() + customerMaster.CustomerCode;
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
            return customerMaster;
        }
        public IList GetCustomerMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var top = PageNumber * RowLimit;
                    var sql = " SELECT TOP " + top + " CustomerMaster.CustomerMaster_ID,";
                    sql += @" TypeDetailMaster.TypeName,";
                    sql += @" CustomerMaster.CustomerCode,";
                    sql += @" CustomerMaster.CustomerName,";
                    sql += @" CustomerMaster.DOB,";
                    sql += @" CustomerMaster.Gender,";
                    sql += @" CustomerMaster.IsActive";
                    sql += @" FROM ServicePro.CustomerMaster";
                    sql += @" INNER JOIN Master.TypeDetailMaster ON TypeDetailMaster.TypeDetailMaster_ID = CustomerMaster.CustomerCodeTemplate";
                    

                    if (WhereCondition != null)
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

                    if (SearchColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        if (WhereCondition != null)
                        {
                            sql += @" AND ";
                        }
                        else
                        {
                            sql += @" WHERE ";
                        }
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

                    if (top != RowLimit)
                    {
                        var except = top - RowLimit;
                        sql += @" EXCEPT ";
                        sql += @" SELECT TOP " + except + " CustomerMaster.CustomerMaster_ID,";
                        sql += @" TypeDetailMaster.TypeName,";
                        sql += @" CustomerMaster.CustomerCode,";
                        sql += @" CustomerMaster.CustomerName,";
                        sql += @" CustomerMaster.DOB,";
                        sql += @" CustomerMaster.Gender,";
                        sql += @" CustomerMaster.IsActive";
                        sql += @" FROM ServicePro.CustomerMaster";
                        sql += @" INNER JOIN Master.TypeDetailMaster ON TypeDetailMaster.TypeDetailMaster_ID = CustomerMaster.CustomerCodeTemplate";
                    }

                    if (WhereCondition != null)
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

                    if (SearchColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        if (WhereCondition != null)
                        {
                            sql += @" AND ";
                        }
                        else
                        {
                            sql += @" WHERE ";
                        }
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
        public int GetCustomerMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = " SELECT CustomerMaster.CustomerMaster_ID,";
                    sql += @" TypeDetailMaster.TypeName,";
                    sql += @" CustomerMaster.CustomerCode,";
                    sql += @" CustomerMaster.CustomerName,";
                    sql += @" CustomerMaster.DOB,";
                    sql += @" CustomerMaster.Gender,";
                    sql += @" CustomerMaster.IsActive";
                    sql += @" FROM ServicePro.CustomerMaster";
                    sql += @" INNER JOIN Master.TypeDetailMaster ON TypeDetailMaster.TypeDetailMaster_ID = CustomerMaster.CustomerCodeTemplate";

                    if (WhereCondition != null)
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

                    if (LikeColumnList != null && !String.IsNullOrEmpty(SearchValue))
                    {
                        if (WhereCondition != null)
                        {
                            sql += @" AND ";
                        }
                        else
                        {
                            sql += @" WHERE ";
                        }
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
                    IList getList = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                    ReturnCount = getList.Count;

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
        #endregion
    }
}
