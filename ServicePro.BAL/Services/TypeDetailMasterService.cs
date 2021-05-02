using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    class TypeDetailMasterService
    {
        #region TypeDetailMaster
        public bool SaveTypeDetailMaster(TypeDetailMaster typeDetailMaster)
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
                        if (typeDetailMaster.TypeDetailMaster_ID > 0)
                        {
                            session.SaveOrUpdate(typeDetailMaster);
                        }
                        else
                        {
                            session.Save(typeDetailMaster);
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
        public bool DeleteTypeDetailMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    TypeDetailMaster typeDetailMaster = GetTypeDetailMaster(Id, session);
                    if (typeDetailMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(typeDetailMaster);
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
        public TypeDetailMaster GetTypeDetailMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            TypeDetailMaster typeDetailMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();
                typeDetailMaster = session.Get<TypeDetailMaster>(PrimaryKey);
                if (typeDetailMaster.TypeDetailMaster_ID > 0)
                {
                    typeDetailMaster = SetTypeDetailMaster(typeDetailMaster, session);
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
            return typeDetailMaster;
        }
        public TypeDetailMaster GetNewTypeDetailMaster()
        {
            TypeDetailMaster typeDetailMaster = new TypeDetailMaster();
            typeDetailMaster.IsActive = 1;
            return typeDetailMaster;
        }
        protected TypeDetailMaster SetTypeDetailMaster(TypeDetailMaster typeDetailMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeMaster typeMaster = TypeMaster.Get(typeDetailMaster.TypeMaster_ID, session);
                    typeDetailMaster.TypeMasterName = typeMaster.TypeMasterName;
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
            return typeDetailMaster;
        }
        public IList GetTypeDetailMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
        public int GetTypeDetailMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
