using NHibernate;
using ServicePro.BAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class CompanyMasterService
    {
        #region CompanyMaster
        public CompanyMaster SaveCompanyMaster(CompanyMaster companyMaster)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (companyMaster.CompanyMaster_ID > 0)
                        {
                            session.SaveOrUpdate(companyMaster);
                        }
                        else
                        {
                            session.Save(companyMaster);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            companyMaster.ReturnValue = 2;
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
            return companyMaster;
        }
        public bool DeleteCompanyMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    CompanyMaster companyMaster = GetCompanyMaster(Id, session);
                    if (companyMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(companyMaster);
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
        public CompanyMaster GetCompanyMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            CompanyMaster companyMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();
                companyMaster = session.Get<CompanyMaster>(PrimaryKey);
                if(companyMaster.CompanyMaster_ID > 0)
                {
                    SetCompanyMaster(companyMaster, session);
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
            return companyMaster;
        }
        public CompanyMaster GetNewCompanyMaster()
        {
            CompanyMaster companyMaster = new CompanyMaster();
            companyMaster.IsActive = 1;
            return companyMaster;
        }
        protected CompanyMaster SetCompanyMaster(CompanyMaster companyMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    //List<string[]> whereList = new List<string[]>();
                    //whereList.Add(new string[] { "Ref_ID", companyMaster.CompanyMaster_ID.ToString() });
                    //whereList.Add(new string[] { "CodeTemplate", null });
                    //companyMaster.AddressMaster = Common.FetchList("ServicePro.AddressMaster", whereList, session);
                    //companyMaster.IDProofMaster = Common.FetchList("ServicePro.IDProofMaster", whereList, session);
                    //companyMaster.StorageMaster = Common.FetchList("ServicePro.StorageMaster", whereList, session);

                    companyMaster.ActivationMaster_Key = CommonUtil.Decrypt(companyMaster.ActivationMaster_Key);
                    companyMaster.Password = CommonUtil.Decrypt(companyMaster.Password);
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
            return companyMaster;
        }
        public IList GetCompanyMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
        public int GetCompanyMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
