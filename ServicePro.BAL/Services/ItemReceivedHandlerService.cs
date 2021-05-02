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
    internal class ItemReceivedHandlerService
    {
        #region ItemReceivedHandler
        public ItemReceivedHandler SaveItemReceivedHandler(ItemReceivedHandler itemReceivedHandler)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (itemReceivedHandler.ItemReceivedHandler_ID > 0)
                        {
                            session.SaveOrUpdate(itemReceivedHandler);
                        }
                        else
                        {
                            session.Save(itemReceivedHandler);
                        }
                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            itemReceivedHandler.ReturnValue = 2;
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
            return itemReceivedHandler;
        }
        public bool DeleteItemReceivedHandler(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    ItemReceivedHandler itemReceivedHandler = GetItemReceivedHandler(Id, session);
                    if (itemReceivedHandler != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(itemReceivedHandler);
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
        public ItemReceivedHandler GetItemReceivedHandler(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ItemReceivedHandler obj = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                obj = session.Get<ItemReceivedHandler>(PrimaryKey);
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
            return obj;
        }
        public ItemReceivedHandler GetNewItemReceivedHandler()
        {
            ItemReceivedHandler itemReceivedHandler = new ItemReceivedHandler();
            itemReceivedHandler.IsActive = 1;
            return itemReceivedHandler;
        }
        protected ItemReceivedHandler SetItemReceivedHandler(ItemReceivedHandler itemReceivedHandler, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    
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
            return itemReceivedHandler;
        }
        public IList GetItemReceivedHandlerGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    int top = Convert.ToInt32("0" + PageNumber) * Convert.ToInt32("0" + RowLimit);

                    var sql = "select top " + top + " ServiceItemMaster.Brand +''+ ServiceItemMaster.Model as ServiceItem,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" CustomerMaster.CustomerName,";
                    sql += @" CONVERT(VARCHAR,ItemReceivedHandler.ReceivedDateTime) as ReceivedDateTime,";
                    sql += @" ItemReceivedHandler.IsActive,";
                    sql += @" ItemReceivedHandler.Comments,";
                    sql += @" ItemReceivedHandler.ItemReceivedHandler_ID";
                    sql += @" from ServicePro.ItemReceivedHandler";
                    sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ItemReceivedHandler.ServiceItemMaster_ID";
                    sql += @" left outer join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ItemReceivedHandler.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = ItemReceivedHandler.EmployeeMaster_ID";
                    
                    if (SearchColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        sql += @" WHERE ";
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

                    sql += @" EXCEPT ";
                    var except = top - RowLimit;
                    sql += "select top " + except + " ServiceItemMaster.Brand +''+ ServiceItemMaster.Model as ServiceItem,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" CustomerMaster.CustomerName,";
                    sql += @" CONVERT(VARCHAR,ItemReceivedHandler.ReceivedDateTime) as ReceivedDateTime,";
                    sql += @" ItemReceivedHandler.IsActive,";
                    sql += @" ItemReceivedHandler.Comments,";
                    sql += @" ItemReceivedHandler.ItemReceivedHandler_ID";
                    sql += @" from ServicePro.ItemReceivedHandler";
                    sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ItemReceivedHandler.ServiceItemMaster_ID";
                    sql += @" left outer join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ItemReceivedHandler.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = ItemReceivedHandler.EmployeeMaster_ID";

                    if (SearchColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        sql += @" WHERE ";
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
        public int GetItemReceivedHandlerGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
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

                    var sql = "select ServiceItemMaster.Brand +''+ ServiceItemMaster.Model as ServiceItem,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" CustomerMaster.CustomerName,";
                    sql += @" CONVERT(VARCHAR,ItemReceivedHandler.ReceivedDateTime) as ReceivedDateTime,";
                    sql += @" ItemReceivedHandler.IsActive,";
                    sql += @" ItemReceivedHandler.Comments,";
                    sql += @" ItemReceivedHandler.ItemReceivedHandler_ID";
                    sql += @" from ServicePro.ItemReceivedHandler";
                    sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ItemReceivedHandler.ServiceItemMaster_ID";
                    sql += @" left outer join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ItemReceivedHandler.CustomerMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = ItemReceivedHandler.EmployeeMaster_ID";

                    if (LikeColumnList != null && !string.IsNullOrEmpty(SearchValue))
                    {
                        sql += @" WHERE ";
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
                    IList list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                    ReturnCount = list.Count;
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
