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
    internal class UserMasterService
    {
        #region UserMaster
        public UserMaster SaveUserMaster(UserMaster userMaster)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (userMaster.UserMaster_ID > 0)
                        {
                            session.SaveOrUpdate(userMaster);
                        }
                        else
                        {
                            session.Save(userMaster);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            userMaster.ReturnValue = 2;
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
            return userMaster;
        }
        public bool DeleteUserMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    UserMaster userMaster = GetUserMaster(Id, session);
                    if (userMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(userMaster);
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
        public UserMaster GetUserMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            UserMaster userMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();
                userMaster = session.Get<UserMaster>(PrimaryKey);
                if (userMaster.UserMaster_ID > 0)
                {
                    userMaster = SetUserMaster(userMaster, session);
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
            return userMaster;
        }
        public UserMaster GetNewUserMaster()
        {
            UserMaster userMaster = new UserMaster();
            userMaster.IsActive = 1;
            return userMaster;
        }
        protected UserMaster SetUserMaster(UserMaster userMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster rollType = TypeDetailMaster.Get(userMaster.RollType, session);
                    userMaster.RollTypeName = rollType.TypeName;

                    userMaster.Password = CommonUtil.Decrypt(userMaster.Password);
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
            return userMaster;
        }
        public IList GetUserMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
                    var sql = " SELECT TOP " + top + " DetailEmployeeCodeTemplate.TypeName AS EmployeeCodeTemplate,";
                    sql += @" EmployeeMaster.EmployeeCode,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" EmployeeMaster.EmployeeCode,";
                    sql += @" UserMaster.UserName,";
                    sql += @" UserMaster.UserMaster_ID,";
                    sql += @" DetailRollType.TypeName as RollType,";
                    sql += @" EmployeeMaster.IsActive";
                    sql += @" FROM ServicePro.UserMaster";
                    sql += @" INNER JOIN Master.EmployeeMaster ON EmployeeMaster.EmployeeMaster_ID = UserMaster.EmployeeMaster_ID";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailRollType ON DetailRollType.TypeDetailMaster_ID = UserMaster.RollType";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmployeeCodeTemplate ON DetailEmployeeCodeTemplate.TypeDetailMaster_ID = EmployeeMaster.EmployeeCodeTemplate";


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

                    if (WhereCondition != null || SearchColumnList != null)
                    {
                        if (WhereCondition != null)
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
                                    sql += @"" + ColumnName + " = '" + ColumnValue + "' ";
                                }
                                if (checkCount != count)
                                {
                                    sql += @" AND ";
                                    checkCount++;
                                }
                            }
                        }
                    }

                    if (top != RowLimit)
                    {
                        sql += @" EXCEPT ";
                        sql += @" SELECT TOP " + top + " DetailEmployeeCodeTemplate.TypeName AS EmployeeCodeTemplate,";
                        sql += @" EmployeeMaster.EmployeeCode,";
                        sql += @" EmployeeMaster.EmployeeName,";
                        sql += @" EmployeeMaster.EmployeeCode,";
                        sql += @" UserMaster.UserName,";
                        sql += @" UserMaster.UserMaster_ID,";
                        sql += @" DetailRollType.TypeName as RollType,";
                        sql += @" EmployeeMaster.IsActive";
                        sql += @" FROM ServicePro.UserMaster";
                        sql += @" INNER JOIN Master.EmployeeMaster ON EmployeeMaster.EmployeeMaster_ID = UserMaster.EmployeeMaster_ID";
                        sql += @" INNER JOIN Master.TypeDetailMaster AS DetailRollType ON DetailRollType.TypeDetailMaster_ID = UserMaster.RollType";
                        sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmployeeCodeTemplate ON DetailEmployeeCodeTemplate.TypeDetailMaster_ID = EmployeeMaster.EmployeeCodeTemplate";
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
        public int GetUserMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = " SELECT DetailEmployeeCodeTemplate.TypeName AS EmployeeCodeTemplate,";
                    sql += @" EmployeeMaster.EmployeeCode,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" EmployeeMaster.EmployeeCode,";
                    sql += @" UserMaster.UserName,";
                    sql += @" UserMaster.UserMaster_ID,";
                    sql += @" DetailRollType.TypeName as RollType,";
                    sql += @" EmployeeMaster.IsActive";
                    sql += @" FROM ServicePro.UserMaster";
                    sql += @" INNER JOIN Master.EmployeeMaster ON EmployeeMaster.EmployeeMaster_ID = UserMaster.EmployeeMaster_ID";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailRollType ON DetailRollType.TypeDetailMaster_ID = UserMaster.RollType";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailEmployeeCodeTemplate ON DetailEmployeeCodeTemplate.TypeDetailMaster_ID = EmployeeMaster.EmployeeCodeTemplate";

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
