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
    internal class StorageMasterService
    {
        #region StorageMaster
        public bool SaveStorageMaster(StorageMaster storageMaster)
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
                        if (storageMaster.StorageMaster_ID > 0)
                        {
                            session.SaveOrUpdate(storageMaster);
                        }
                        else
                        {
                            session.Save(storageMaster);
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
        public bool DeleteStorageMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    StorageMaster storageMaster = GetStorageMaster(Id, session);
                    if (storageMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(storageMaster);
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
        public StorageMaster GetStorageMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            StorageMaster storageMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                storageMaster = session.Get<StorageMaster>(PrimaryKey);
                if (storageMaster.StorageMaster_ID > 0)
                {
                    storageMaster = SetStorageMaster(storageMaster, session);
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
            return storageMaster;
        }
        public StorageMaster GetNewStorageMaster()
        {
            StorageMaster storageMaster = new StorageMaster();
            storageMaster.IsActive = 1;
            return storageMaster;
        }
        protected StorageMaster SetStorageMaster(StorageMaster storageMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster CodeTemplateCodeName = TypeDetailMaster.Get(Convert.ToInt32(storageMaster.CodeTemplate), session);
                    storageMaster.CodeTemplateName = CodeTemplateCodeName.TypeName;

                    TypeDetailMaster StorageTypeCodeName = TypeDetailMaster.Get(storageMaster.StorageType, session);
                    storageMaster.StorageTypeName = StorageTypeCodeName.TypeName;

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
            return storageMaster;
        }
        public IList GetStorageMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = " select CodeTemplate,";
                    sql += @" Ref_ID,";
                    sql += @" StorageMaster.StorageMaster_ID,";
                    sql += @" CodeTemplate +''+ Ref_ID as CodeTemplateName,";
                    sql += @" FileName,";
                    sql += @" detailStorageType.TypeName as StorageTypeName,";
                    sql += @" StorageType,";
                    sql += @" Extension,";
                    sql += @" ContentType,";
                    //sql += @" StorageMaster_Data,";
                    sql += @" CONVERT(VARCHAR,FileSize/1000) +' KB' as FileSize,";
                    sql += @" StorageMaster.IsActive";
                    sql += @" from ServicePro.StorageMaster";
                    sql += @" inner join Master.TypeDetailMaster as detailStorageType on detailStorageType.TypeDetailMaster_ID = StorageMaster.StorageType";
                    sql += @" left outer join Master.TypeDetailMaster as detailCodeTemplate on detailCodeTemplate.TypeDetailMaster_ID = StorageMaster.CodeTemplate";
                    

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
                                if (ColumnValue == "")
                                {
                                    sql += @"" + ColumnName + " IS NULL ";
                                }
                                else
                                {
                                    sql += @"" + ColumnName + " = '" + ColumnValue + "' ";
                                }
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
        public int GetStorageMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
        #endregion
    }
}
