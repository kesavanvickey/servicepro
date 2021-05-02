using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.BAL
{
    internal class AddressMasterService
    {
        #region AddressMaster
        public bool SaveAddressMaster(AddressMaster addressMaster)
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
                        if (addressMaster.AddressMaster_ID > 0)
                        {
                            session.SaveOrUpdate(addressMaster);
                        }
                        else
                        {
                            session.Save(addressMaster);
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
        public bool DeleteAddressMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    AddressMaster addressMaster = GetAddressMaster(Id, session);
                    if (addressMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(addressMaster);
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
        public AddressMaster GetAddressMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            AddressMaster addressMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                addressMaster = session.Get<AddressMaster>(PrimaryKey);
                if (addressMaster.AddressMaster_ID > 0)
                {
                    addressMaster = SetAddressMaster(addressMaster, session);
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
            return addressMaster;
        }
        public AddressMaster GetNewAddressMaster()
        {
            AddressMaster addressMaster = new AddressMaster();
            addressMaster.IsActive = 1;
            return addressMaster;
        }
        protected AddressMaster SetAddressMaster(AddressMaster addressMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster CodeTemplateCodeName = TypeDetailMaster.Get(Convert.ToInt32(addressMaster.CodeTemplate), session);
                    addressMaster.CodeTemplateName = CodeTemplateCodeName.TypeName;

                    TypeDetailMaster AddressTypeCodeName = TypeDetailMaster.Get(addressMaster.AddressType, session);
                    addressMaster.AddressTypeName = AddressTypeCodeName.TypeName;

                    TypeDetailMaster CityCodeName = TypeDetailMaster.Get(Convert.ToInt32(addressMaster.City), session);
                    addressMaster.CityName = CityCodeName.TypeName;

                    TypeDetailMaster StateCodeName = TypeDetailMaster.Get(Convert.ToInt32(addressMaster.State), session);
                    addressMaster.StateName = StateCodeName.TypeName;

                    TypeDetailMaster CountryCodeName = TypeDetailMaster.Get(Convert.ToInt32(addressMaster.Country), session);
                    addressMaster.CountryName = CountryCodeName.TypeName;

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
            return addressMaster;
        }
        public IList GetAddressMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = " SELECT AddressMaster.AddressMaster_ID,";
                    sql += @" AddressMaster.CodeTemplate,";
                    sql += @" AddressMaster.Ref_ID,";
                    sql += @" AddressTypeDetail.TypeName AS AddressTypeName,";
                    sql += @" AddressMaster.Address1,";
                    sql += @" AddressMaster.Address2,";
                    sql += @" AddressMaster.Address3,";
                    sql += @" CityDetail.TypeName AS CityName,";
                    sql += @" StateDetail.TypeName AS StateName,";
                    sql += @" CountryDetail.TypeName AS CountryName,";
                    sql += @" AddressMaster.Pincode,";
                    sql += @" AddressMaster.ContactNo1,";
                    sql += @" AddressMaster.ContactNo2,";
                    sql += @" AddressMaster.Email,";
                    sql += @" AddressMaster.AddressType,";
                    sql += @" AddressMaster.Created_UserID,";
                    sql += @" AddressMaster.Created_DateTime,";
                    sql += @" AddressMaster.Modified_DateTime,";
                    sql += @" AddressMaster.City,";
                    sql += @" AddressMaster.State,";
                    sql += @" AddressMaster.Country,";
                    sql += @" AddressMaster.IsActive";
                    sql += @" FROM ServicePro.AddressMaster";
                    sql += @" LEFT JOIN Master.TypeDetailMaster AS CodeTemplateDetail ON CodeTemplateDetail.TypeDetailMaster_ID = AddressMaster.CodeTemplate";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS AddressTypeDetail ON AddressTypeDetail.TypeDetailMaster_ID = AddressMaster.AddressType";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS CityDetail ON CityDetail.TypeDetailMaster_ID = AddressMaster.City";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS StateDetail ON StateDetail.TypeDetailMaster_ID = AddressMaster.State";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS CountryDetail ON CountryDetail.TypeDetailMaster_ID = AddressMaster.Country";
                    sql += @" LEFT OUTER JOIN Master.CompanyMaster ON CompanyMaster.CompanyMaster_ID = AddressMaster.Ref_ID";
                    sql += @" LEFT OUTER JOIN Master.EmployeeMaster ON EmployeeMaster.EmployeeMaster_ID = AddressMaster.Ref_ID";
                    sql += @" LEFT OUTER JOIN ServicePro.CustomerMaster ON CustomerMaster.CustomerMaster_ID = AddressMaster.Ref_ID";


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
        public int GetAddressMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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
