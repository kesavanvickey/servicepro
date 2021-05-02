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
    internal class WorkOrderMasterService
    {
        #region WorkOrderMaster
        public WorkOrderMaster SaveWorkOrderMaster(WorkOrderMaster workOrderMaster)
        {
            ITransaction transaction = null;
            ISession session = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        if (workOrderMaster.WorkOrderMaster_ID > 0)
                        {
                            session.SaveOrUpdate(workOrderMaster);
                        }
                        else
                        {
                            session.Save(workOrderMaster);
                        }

                        if(workOrderMaster.ServiceItemDetail != null)
                        {
                            session.SaveOrUpdate(workOrderMaster.ServiceItemDetail);
                        }

                        transaction.Commit();
                        if (transaction.WasCommitted == true)
                        {
                            workOrderMaster.ReturnValue = 2;
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
            return workOrderMaster;
        }
        public bool DeleteWorkOrderMaster(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    WorkOrderMaster workOrderMaster = GetWorkOrderMaster(Id, session);
                    if (workOrderMaster != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(workOrderMaster);
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
        public WorkOrderMaster GetWorkOrderMaster(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            WorkOrderMaster workOrderMaster = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                workOrderMaster = session.Get<WorkOrderMaster>(PrimaryKey);
                if (workOrderMaster.WorkOrderMaster_ID > 0)
                {
                    workOrderMaster = SetWorkOrderMaster(workOrderMaster, session);
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
            return workOrderMaster;
        }
        public WorkOrderMaster GetNewWorkOrderMaster()
        {
            WorkOrderMaster workOrderMaster = new WorkOrderMaster();
            workOrderMaster.ServiceItemDetail = ServiceItemDetail.GetNew();
            workOrderMaster.IsActive = 1;
            return workOrderMaster;
        }
        protected WorkOrderMaster SetWorkOrderMaster(WorkOrderMaster workOrderMaster, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    TypeDetailMaster workCodeTemplate = TypeDetailMaster.Get(workOrderMaster.WorkCodeTemplate, session);
                    workOrderMaster.WorkCodeTemplateName = workCodeTemplate.TypeName;

                    ServiceItemDetail getMasterId  = ServiceItemDetail.Get(workOrderMaster.ServiceItemDetail_ID, session);
                    workOrderMaster.ServiceItemDetail = getMasterId;

                    List<string[]> where = new List<string[]>();
                    where.Add(new string[] { "ServiceItemMaster_ID", workOrderMaster.ServiceItemDetail.ServiceItemMaster_ID.ToString() });
                    where.Add(new string[] { "IsActive", "1" });
                    workOrderMaster.ItemReceivedHandler = Convert.ToInt32("0" + Common.GetSingleColumnValue("ServicePro.ItemReceivedHandler", "ItemReceivedHandler_ID", where, session));
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
            return workOrderMaster;
        }
        public IList GetWorkOrderMasterGrid(List<string[]> WhereCondition = null, List<string[]> SearchColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                int top = Convert.ToInt32("0" + PageNumber) * Convert.ToInt32("0" + RowLimit);

                var sql = "select top " + top + " WorkOrderMaster.WorkOrderMaster_ID,";
                sql += @" WorkCodeTemplate,";
                sql += @" ServiceItemMaster.Brand +'-'+ ServiceItemMaster.Model as ServiceItem,";
                sql += @" ServiceItemDetail.Comments,";
                sql += @" EmployeeMaster.EmployeeName,";
                sql += @" TypeDetailMaster.TypeName as Status,";
                sql += @" CONVERT(VARCHAR,WorkOrderMaster.ServiceStartDate) as ServiceStartDate,";
                sql += @" CONVERT(VARCHAR,WorkOrderMaster.ServiceEndDate) as ServiceEndDate,";
                sql += @" WorkOrderMaster.IsActive";
                sql += @" from ServicePro.WorkOrderMaster";
                sql += @" inner join ServicePro.ServiceItemDetail on ServiceItemDetail.ServiceItemDetail_ID = WorkOrderMaster.ServiceItemDetail_ID";
                sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemDetail.StatusType";
                sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ServiceItemDetail.ServiceItemMaster_ID";
                sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = WorkOrderMaster.EmployeeMaster_ID";

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
                sql += "select top " + except + " WorkOrderMaster.WorkOrderMaster_ID,";
                sql += @" WorkCodeTemplate,";
                sql += @" ServiceItemMaster.Brand +'-'+ ServiceItemMaster.Model as ServiceItem,";
                sql += @" ServiceItemDetail.Comments,";
                sql += @" EmployeeMaster.EmployeeName,";
                sql += @" TypeDetailMaster.TypeName as Status,";
                sql += @" CONVERT(VARCHAR,WorkOrderMaster.ServiceStartDate) as ServiceStartDate,";
                sql += @" CONVERT(VARCHAR,WorkOrderMaster.ServiceEndDate) as ServiceEndDate,";
                sql += @" WorkOrderMaster.IsActive";
                sql += @" from ServicePro.WorkOrderMaster";
                sql += @" inner join ServicePro.ServiceItemDetail on ServiceItemDetail.ServiceItemDetail_ID = WorkOrderMaster.ServiceItemDetail_ID";
                sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemDetail.StatusType";
                sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ServiceItemDetail.ServiceItemMaster_ID";
                sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = WorkOrderMaster.EmployeeMaster_ID";

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
        public int GetWorkOrderMasterGridCount(List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            int ReturnCount = 0;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                    //Add Grid Count coding here

                    var sql = "select WorkOrderMaster.WorkOrderMaster_ID,";
                    sql += @" WorkCodeTemplate,";
                    sql += @" ServiceItemMaster.Brand +'-'+ ServiceItemMaster.Model as ServiceItem,";
                    sql += @" ServiceItemDetail.Comments,";
                    sql += @" EmployeeMaster.EmployeeName,";
                    sql += @" TypeDetailMaster.TypeName as Status,";
                    sql += @" CONVERT(VARCHAR,WorkOrderMaster.ServiceStartDate) as ServiceStartDate,";
                    sql += @" CONVERT(VARCHAR,WorkOrderMaster.ServiceEndDate) as ServiceEndDate,";
                    sql += @" WorkOrderMaster.IsActive";
                    sql += @" from ServicePro.WorkOrderMaster";
                    sql += @" inner join ServicePro.ServiceItemDetail on ServiceItemDetail.ServiceItemDetail_ID = WorkOrderMaster.ServiceItemDetail_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemDetail.StatusType";
                    sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ServiceItemDetail.ServiceItemMaster_ID";
                    sql += @" inner join Master.EmployeeMaster on EmployeeMaster.EmployeeMaster_ID = WorkOrderMaster.EmployeeMaster_ID";

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
