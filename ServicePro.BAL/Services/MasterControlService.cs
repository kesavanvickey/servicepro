using NHibernate;
using NHibernate.Transform;
using ServicePro.BAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class MasterControlService
    {
        #region MasterControl
        public bool SaveMasterControl(MasterControl masterControl)
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
                        session.Save(masterControl);
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
        public bool DeleteMasterControl(string Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    MasterControl masterControl = GetMasterControl(Id, session);
                    if (masterControl != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(masterControl);
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
        public MasterControl GetMasterControl(string PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            MasterControl masterControl = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();
                using (session = NHibernateHelper.OpenSession())
                {
                    masterControl = session.Get<MasterControl>(PrimaryKey);
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
            return masterControl;
        }
        public MasterControl GetNewMasterControl()
        {
            MasterControl masterControl = new MasterControl();
            masterControl.IsActive = 1;
            return masterControl;
        }
        public MasterControl SetMasterControl(MasterControl masterControl, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    //Set Coding here
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
            return masterControl;
        }
        #endregion

        #region LoginCheck
        public IList LoginValidate(string UserId, string Password, int UserType)
        {
            var query = "";
            IList list = null;
            ISession session = null;
            ITransaction transaction = null;
            IQuery iquery = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (UserType == 1)
                        {
                            query += @" SELECT CompanyMaster.CompanyMaster_ID,";
                            query += @" EmployeeMaster.EmployeeMaster_ID,";
                            query += @" CompanyMaster.CompanyName,";
                            query += @" CompanyMaster.CompanyLogo,";
                            query += @" CompanyMaster.ReportSignature,";
                            query += @" CompanyMaster.ReportBottom,";
                            query += @" EmployeeMaster.EmployeeName,";
                            query += @" UserMaster.UserName,";
                            query += @" UserMaster.UserMaster_ID,";
                            query += @" UserMaster.Password,";
                            query += @" TypeDetailMaster.TypeName AS RollTypeName";
                            query += @" FROM ServicePro.UserMaster";
                            query += @" INNER JOIN Master.TypeDetailMaster ON TypeDetailMaster.TypeDetailMaster_ID = UserMaster.RollType";
                            query += @" INNER JOIN Master.EmployeeMaster ON EmployeeMaster.EmployeeMaster_ID = UserMaster.EmployeeMaster_ID";
                            query += @" INNER JOIN Master.CompanyMaster ON CompanyMaster.CompanyMaster_ID = EmployeeMaster.CompanyMaster_ID";
                            query += @" WHERE UserMaster.UserName = '"+UserId+"' AND UserMaster.Password = '"+Password+"'";
                        }
                        else if (UserType == 2)
                        {
                            query += @"SELECT * FROM Master.CompanyMaster WHERE UserName = '" + UserId + "' AND Password = '" + Password + "'";
                        }
                        iquery = session.CreateSQLQuery(query);
                        list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
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
            return list;
        }
        #endregion
    }
}
