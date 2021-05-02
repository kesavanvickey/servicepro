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
    internal class EmployeeMasterService
    {
        #region EmployeeMaster
        public EmployeeMaster SaveEmployeeMaster(EmployeeMaster employeeMaster)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (employeeMaster.EmployeeMaster_ID > 0)
                        {
                            session.SaveOrUpdate(employeeMaster);
                        }
                        else
                        {
                            session.Save(employeeMaster);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            employeeMaster.ReturnValue = 2;
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
            return employeeMaster;
        }
        public bool DeleteEmployeeMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    EmployeeMaster employeeMaster = GetEmployeeMaster(Id, session);
                    if (employeeMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(employeeMaster);
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
        public EmployeeMaster GetEmployeeMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            EmployeeMaster employeeMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                employeeMaster = session.Get<EmployeeMaster>(PrimaryKey);
                if (employeeMaster.EmployeeMaster_ID > 0)
                {
                    employeeMaster = SetEmployeeMaster(employeeMaster, session);
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
            return employeeMaster;
        }
        public EmployeeMaster GetNewEmployeeMaster()
        {
            EmployeeMaster employeeMaster = new EmployeeMaster();
            employeeMaster.IsActive = 1;
            return employeeMaster;
        }
        protected EmployeeMaster SetEmployeeMaster(EmployeeMaster employeeMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster idCodeType = TypeDetailMaster.Get(employeeMaster.EmployeeCodeTemplate, session);
                    employeeMaster.EmployeeCodeTemplateName = idCodeType.TypeName;

                    TypeDetailMaster employeeType = TypeDetailMaster.Get(employeeMaster.EmployeeType, session);
                    employeeMaster.EmployeeTypeName = idCodeType.TypeName;

                    employeeMaster.EmployeeCodeFullName = idCodeType.TypeName.ToString() + employeeMaster.EmployeeCode;
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
            return employeeMaster;
        }
        public IList GetEmployeeMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
                    var sql = " SELECT TOP " + top + " EmployeeMaster.EmployeeMaster_ID,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" DetailEmloyeeCodeTemplate.TypeName AS EmployeeCodeTemplate,";
                    sql += @" EmployeeMaster.EmployeeCode,";
                    sql += @" EmployeeMaster.Gender,";
                    sql += @" DetailEmployeeType.TypeName AS EmployeeType,";
                    sql += @" EmployeeMaster.IsActive,";
                    sql += @" EmployeeMaster.JointDate";
                    sql += @" FROM Master.EmployeeMaster";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmloyeeCodeTemplate ON EmployeeMaster.EmployeeCodeTemplate = DetailEmloyeeCodeTemplate.TypeDetailMaster_ID ";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmployeeType ON EmployeeMaster.EmployeeType = DetailEmployeeType.TypeDetailMaster_ID";

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
                        sql += @" SELECT TOP " + except + " EmployeeMaster.EmployeeMaster_ID,";
                        sql += @" EmployeeMaster.EmployeeName,";
                        sql += @" DetailEmloyeeCodeTemplate.TypeName AS EmployeeCodeTemplate,";
                        sql += @" EmployeeMaster.EmployeeCode,";
                        sql += @" EmployeeMaster.Gender,";
                        sql += @" DetailEmployeeType.TypeName AS EmployeeType,";
                        sql += @" EmployeeMaster.IsActive,";
                        sql += @" EmployeeMaster.JointDate";
                        sql += @" FROM Master.EmployeeMaster";
                        sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmloyeeCodeTemplate ON EmployeeMaster.EmployeeCodeTemplate = DetailEmloyeeCodeTemplate.TypeDetailMaster_ID ";
                        sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmployeeType ON EmployeeMaster.EmployeeType = DetailEmployeeType.TypeDetailMaster_ID";
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
        public int GetEmployeeMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = "SELECT EmployeeMaster.EmployeeMaster_ID,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" DetailEmloyeeCodeTemplate.TypeName AS EmployeeCodeTemplate,";
                    sql += @" EmployeeMaster.EmployeeCode,";
                    sql += @" EmployeeMaster.Gender,";
                    sql += @" DetailEmployeeType.TypeName AS EmployeeType,";
                    sql += @" EmployeeMaster.IsActive,";
                    sql += @" EmployeeMaster.JointDate";
                    sql += @" FROM Master.EmployeeMaster";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmloyeeCodeTemplate ON EmployeeMaster.EmployeeCodeTemplate = DetailEmloyeeCodeTemplate.TypeDetailMaster_ID ";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmployeeType ON EmployeeMaster.EmployeeType = DetailEmployeeType.TypeDetailMaster_ID";

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
