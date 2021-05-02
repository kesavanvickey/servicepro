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
    internal class IDProofMasterService
    {
        #region IDProofMaster
        public bool SaveIDProofMaster(IDProofMaster iDProofMaster)
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
                        if (iDProofMaster.IDProofMaster_ID > 0)
                        {
                            session.SaveOrUpdate(iDProofMaster);
                        }
                        else
                        {
                            session.Save(iDProofMaster);
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
        public bool DeleteIDProofMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    IDProofMaster iDProofMaster = GetIDProofMaster(Id, session);
                    if (iDProofMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(iDProofMaster);
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
        public IDProofMaster GetIDProofMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IDProofMaster iDProofMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                iDProofMaster = session.Get<IDProofMaster>(PrimaryKey);
                if (iDProofMaster.IDProofMaster_ID > 0)
                {
                    iDProofMaster = SetIDProofMaster(iDProofMaster, session);
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
            return iDProofMaster;
        }
        public IDProofMaster GetNewIDProofMaster()
        {
            IDProofMaster iDProofMaster = new IDProofMaster();
            iDProofMaster.IsActive = 1;
            return iDProofMaster;
        }
        protected IDProofMaster SetIDProofMaster(IDProofMaster iDProofMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster CodeTemplateCodeName = TypeDetailMaster.Get(Convert.ToInt32(iDProofMaster.CodeTemplate), session);
                    iDProofMaster.CodeTemplateName = CodeTemplateCodeName.TypeName;

                    TypeDetailMaster IDProofTypeCodeName = TypeDetailMaster.Get(iDProofMaster.IDProofType, session);
                    iDProofMaster.IDProofTypeName = IDProofTypeCodeName.TypeName;
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
            return iDProofMaster;
        }
        public IList GetIDProofMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = " SELECT IDProofMaster_ID,";
                    sql += @" CodeTemplate,";
                    sql += @" DetailCodeTemplate.TypeName AS CodeTemplateName,";
                    sql += @" Ref_ID,";
                    sql += @" IDProofType,";
                    sql += @" DetailIDProofType.TypeName AS IDProofTypeName,";
                    sql += @" IDProofMaster.IDProofData,";
                    sql += @" IDProofMaster.IsActive,";
                    sql += @" IDProofMaster.Created_UserID,";
                    sql += @" IDProofMaster.Created_DateTime";
                    sql += @" FROM ServicePro.IDProofMaster";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS DetailIDProofType ON DetailIDProofType.TypeDetailMaster_ID = IDProofMaster.IDProofType";
                    sql += @" LEFT JOIN Master.TypeDetailMaster AS DetailCodeTemplate ON DetailCodeTemplate.TypeDetailMaster_ID = IDProofMaster.CodeTemplate";

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
        public int GetIDProofMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
