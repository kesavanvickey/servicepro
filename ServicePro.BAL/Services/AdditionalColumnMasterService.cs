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
    internal class AdditionalColumnMasterService
    {
        #region AdditionalColumnMaster
        public AdditionalColumnMaster SaveAdditionalColumnMaster(AdditionalColumnMaster additionalColumnMaster)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (additionalColumnMaster.AdditionalColumnMaster_ID > 0)
                        {
                            session.SaveOrUpdate(additionalColumnMaster);
                        }
                        else
                        {
                            session.Save(additionalColumnMaster);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            additionalColumnMaster.ReturnValue = 2;
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
            return additionalColumnMaster;
        }
        public bool DeleteAdditionalColumnMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    AdditionalColumnMaster additionalColumnMaster = GetAdditionalColumnMaster(Id, session);
                    if (additionalColumnMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(additionalColumnMaster);
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
        public AdditionalColumnMaster GetAdditionalColumnMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            AdditionalColumnMaster additionalColumnMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                additionalColumnMaster = session.Get<AdditionalColumnMaster>(PrimaryKey);
                if (additionalColumnMaster.AdditionalColumnMaster_ID > 0)
                {
                    additionalColumnMaster = SetAdditionalColumnMaster(additionalColumnMaster, session);
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
            return additionalColumnMaster;
        }
        public AdditionalColumnMaster GetNewAdditionalColumnMaster()
        {
            AdditionalColumnMaster additionalColumnMaster = new AdditionalColumnMaster();
            additionalColumnMaster.IsActive = 1;
            return additionalColumnMaster;
        }
        protected AdditionalColumnMaster SetAdditionalColumnMaster(AdditionalColumnMaster additionalColumnMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster tableName = TypeDetailMaster.Get(additionalColumnMaster.TableName, session);
                    additionalColumnMaster.TableNameString = tableName.TypeName;

                    TypeDetailMaster addColName = TypeDetailMaster.Get(additionalColumnMaster.AdditionalColumnName, session);
                    additionalColumnMaster.AdditionalColumnNameString = addColName.TypeName;

                    TypeDetailMaster dataType = TypeDetailMaster.Get(additionalColumnMaster.DataType, session);
                    additionalColumnMaster.DataTypeString = dataType.TypeName;
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
            return additionalColumnMaster;
        }
        public IList GetAdditionalColumnGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    var top = PageNumber * RowLimit;
                    var sql = "SELECT TOP " + top + " TableName.TypeName AS TableName,";
                    sql += @" AdditionalColumnName.TypeName AS AdditionalColumnName,";
                    sql += @" DataType.TypeName AS DataType,";
                    sql += @" AdditionalColumnMaster.DisplayName,";
                    sql += @" AdditionalColumnMaster.IsActive,";
                    sql += @" AdditionalColumnMaster.AdditionalColumnMaster_ID";
                    sql += @" FROM Master.AdditionalColumnMaster";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS TableName ON ";
                    sql += @" AdditionalColumnMaster.TableName = TableName.TypeDetailMaster_ID";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS AdditionalColumnName ON ";
                    sql += @" AdditionalColumnMaster.AdditionalColumnName = AdditionalColumnName.TypeDetailMaster_ID";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DataType ON ";
                    sql += @" AdditionalColumnMaster.DataType = DataType.TypeDetailMaster_ID";

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
                        sql = "SELECT TOP " + except + " TableName.TypeName AS TableName,";
                        sql += @" AdditionalColumnName.TypeName AS AdditionalColumnName,";
                        sql += @" DataType.TypeName AS DataType,";
                        sql += @" AdditionalColumnMaster.DisplayName,";
                        sql += @" AdditionalColumnMaster.IsActive,";
                        sql += @" AdditionalColumnMaster.AdditionalColumnMaster_ID";
                        sql += @" FROM Master.AdditionalColumnMaster";
                        sql += @" INNER JOIN Master.TypeDetailMaster AS TableName ON ";
                        sql += @" AdditionalColumnMaster.TableName = TableName.TypeDetailMaster_ID";
                        sql += @" INNER JOIN Master.TypeDetailMaster AS AdditionalColumnName ON ";
                        sql += @" AdditionalColumnMaster.AdditionalColumnName = AdditionalColumnName.TypeDetailMaster_ID";
                        sql += @" INNER JOIN Master.TypeDetailMaster AS DataType ON ";
                        sql += @" AdditionalColumnMaster.DataType = DataType.TypeDetailMaster_ID";
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
        public int GetAdditionalColumnGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            int ReturnCount = 0;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    var sql = "SELECT TableName.TypeName AS TableName,";
                    sql += @" AdditionalColumnName.TypeName AS AdditionalColumnName,";
                    sql += @" DataType.TypeName AS DataType,";
                    sql += @" AdditionalColumnMaster.DisplayName,";
                    sql += @" AdditionalColumnMaster.IsActive,";
                    sql += @" AdditionalColumnMaster.AdditionalColumnMaster_ID";
                    sql += @" FROM Master.AdditionalColumnMaster";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS TableName ON ";
                    sql += @" AdditionalColumnMaster.TableName = TableName.TypeDetailMaster_ID";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS AdditionalColumnName ON ";
                    sql += @" AdditionalColumnMaster.AdditionalColumnName = AdditionalColumnName.TypeDetailMaster_ID";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DataType ON ";
                    sql += @" AdditionalColumnMaster.DataType = DataType.TypeDetailMaster_ID";

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
