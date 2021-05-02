using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ServicePro.BAL;
using NHibernate.Transform;
using System.Collections;


namespace ServicePro.BAL
{
    public class Common
    {
        public static DataTable FetchTable(string TableName, List<string[]> WhereCondition)
        {
            DataTable dt = null;
            IList list = null;
            try
            {
                list = FetchList(TableName, WhereCondition);
                dt = CommonUtil.ConvertToDataTable(TableName, list);
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return dt;
        }
        public static IList FetchList(string TableName, List<string[]> WhereCondition, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IList result = null;
            try
            {
                if (TableName != null)
                {
                    if (IsNewSession) session = NHibernateHelper.OpenSession();

                    var query = "SELECT * FROM " + TableName + "";
                    if (WhereCondition != null)
                    {
                        query += @" WHERE ";
                        int count = WhereCondition.Count;
                        int checkCount = 1;
                        foreach (var list in WhereCondition)
                        {
                            for (var i = 1; i < list.Length; i++)
                            {
                                string ColumnName = null;
                                string ColumnValue = null;
                                ColumnName = list[0];
                                ColumnValue = list[1];
                                if (ColumnValue == null)
                                    query += @"" + ColumnName + " IS NULL";
                                else
                                    query += @"" + ColumnName + " = '" + ColumnValue + "'";
                            }
                            if (checkCount != count)
                            {
                                query += @" AND ";
                                checkCount++;
                            }
                        }
                    }
                    IQuery iquery = session.CreateSQLQuery(query);
                    result = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex.Message);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return result;
        }
        public static bool DuplicateCheck(string TableName, string PrimaryKeyColumn, int PrimaryId, List<string[]> ColumnList)
        {
            bool Duplicate = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                if (TableName != null && PrimaryKeyColumn != null)
                {
                    using (session = NHibernateHelper.OpenSession())
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            var query = "SELECT * FROM " + TableName + " WHERE " + PrimaryKeyColumn + " <> '" + PrimaryId + "'";
                            if (ColumnList != null)
                            {
                                foreach (var list in ColumnList)
                                {
                                    for (var i = 1; i < list.Length; i++)
                                    {
                                        string ColumnName = null;
                                        string ColumnValue = null;
                                        ColumnName = list[0];
                                        ColumnValue = list[1];
                                        query += @" AND " + ColumnName + " = '" + ColumnValue + "'";
                                    }
                                }
                            }
                            IQuery iquery = session.CreateSQLQuery(query);
                            var result = iquery.List();
                            if (result.Count > 0)
                            {
                                Duplicate = true;
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
            return Duplicate;
        }
        public static bool IsValueExist(string TableName, string ColumnName, string ColumnValue)
        {
            bool exist = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                if (TableName != null && ColumnName != null && ColumnValue != null)
                {
                    using (session = NHibernateHelper.OpenSession())
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            var query = "SELECT * FROM " + TableName + " WHERE " + ColumnName + " = '" + ColumnValue + "'";
                            IQuery iquery = session.CreateSQLQuery(query);
                            var result = iquery.List();
                            if (result.Count > 0)
                            {
                                exist = true;
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
            return exist;
        }
        public static DataTable GetType(string TypeMasterName, bool Active, int CompanyId, ISession session = null)
        {
            bool SessionAlreadyCreated = session == null ? true : false;
            DataTable dt = null;
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();
                using (transaction = session.BeginTransaction())
                {
                    string sql = @" SELECT Type.TypeMaster_ID,";
                    sql += @" Type.TypeMasterName,";
                    sql += @" Detail.TypeDetailMaster_ID,";
                    sql += @" Detail.TypeName,";
                    sql += @" Detail.Description";
                    sql += @" FROM Master.TypeMaster AS Type";
                    sql += @" INNER JOIN Master.TypeDetailMaster AS Detail";
                    sql += @" ON Detail.TypeMaster_ID = Type.TypeMaster_ID";

                    if (!string.IsNullOrEmpty(TypeMasterName))
                    {
                        sql += @" WHERE Type.TypeMasterName = '" + TypeMasterName + "' AND";
                    }
                    else
                    {
                        sql += @" WHERE";
                    }

                    int IsActive = 0;
                    if (Active == true)
                        IsActive = 1;
                    sql += @" Type.IsActive = '" + IsActive + "' AND";
                    sql += @" Type.CompanyMaster_ID = '" + CompanyId + "'";
                    sql += @" ORDER BY Type.TypeMasterName";

                    IQuery iquery = session.CreateSQLQuery(sql);
                    list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                    dt = CommonUtil.ConvertToDataTable("TypeTable", list);
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (SessionAlreadyCreated)
                    session.Dispose();
            }
            return dt;
        }
        public static IList GetGridList(string TableName, List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool SessionAlreadyCreated = session == null ? true : false;
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            IList list = null;
            try
            {
                if (TableName != null)
                {
                    if (IsNewSession) session = NHibernateHelper.OpenSession();

                    using (transaction = session.BeginTransaction())
                    {
                        var top = PageNumber * RowLimit;
                        var sql = "SELECT TOP " + top + " * FROM " + TableName + " ";

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

                        if (LikeColumnList != null && !string.IsNullOrEmpty(SearchValue))
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

                        if (top != RowLimit)
                        {
                            var except = top - RowLimit;
                            sql += @" EXCEPT ";
                            sql += @" SELECT TOP " + except + " * FROM " + TableName + " ";

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

                            if (LikeColumnList != null && !string.IsNullOrEmpty(SearchValue))
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
                        }

                        if (SortBy != null && SortingOrder != null)
                        {
                            sql += @" ORDER BY " + SortBy + " " + SortingOrder + " ";
                        }
                        IQuery iquery = session.CreateSQLQuery(sql);
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
                if (SessionAlreadyCreated)
                    session.Dispose();
            }
            return list;
        }
        public static int GetGridListCount(string TableName, List<string[]> WhereCondition = null, List<string[]> LikeColumnList = null, string SearchValue = null, string SortBy = null, string SortingOrder = null, int PageNumber = 0, int RowLimit = 0, ISession session = null)
        {
            bool SessionAlreadyCreated = session == null ? true : false;
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            int ReturnCount = 0;
            try
            {
                if (TableName != null)
                {
                    if (IsNewSession) session = NHibernateHelper.OpenSession();

                    using (transaction = session.BeginTransaction())
                    {
                        var sql = "SELECT * FROM " + TableName + " ";

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
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            finally
            {
                if (SessionAlreadyCreated)
                    session.Dispose();
            }
            return ReturnCount;
        }
        public static int GetMaxValueByColumnName(string TableName, string ColumnName)
        {
            ISession session = null;
            ITransaction transaction = null;
            int result = 0;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        var query = "SELECT ISNULL(MAX(" + ColumnName + "),0)+1 AS " + ColumnName + " FROM " + TableName + "";
                        IQuery iquery = session.CreateSQLQuery(query);
                        result = iquery.UniqueResult<int>();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex);
            }
            return result;
        }
        public static DataTable GetCodeTemplateForComboBox(string TableName, string PrimaryKeyColumnName, string CodeTemplateName, string CodeColumnName, string AddColumnName, string getReturnColumnName)
        {
            DataTable dt = null;
            IList result = null;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                if (TableName != null)
                {
                    using (session = NHibernateHelper.OpenSession())
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            string sql = " SELECT " + PrimaryKeyColumnName + ",";
                            if (AddColumnName != null)
                            {
                                sql += @" detail.TypeName +''+ CONVERT(varchar," + CodeColumnName + ") +' - '+ " + AddColumnName + " as " + getReturnColumnName + "";
                            }
                            else
                            {
                                sql += @" detail.TypeName +''+ CONVERT(varchar," + CodeColumnName + ") as " + getReturnColumnName + "";
                            }
                            sql += @" FROM " + TableName + "";
                            sql += @" INNER join Master.TypeDetailMaster as detail on detail.TypeDetailMaster_ID = " + TableName + "." + CodeTemplateName + "";
                            IQuery iquery = session.CreateSQLQuery(sql);
                            result = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex.Message);
            }
            finally
            {
                session.Dispose();
            }
            return dt = CommonUtil.ConvertToDataTable("", result);
        }
        public static int GetTypeMasterIdByName(string TypeName, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            int id = 0;
            IList list = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    var sql = " select TypeMaster.TypeMaster_ID";
                    sql += @" from Master.TypeDetailMaster";
                    sql += @" inner join Master.TypeMaster on TypeMaster.TypeMaster_ID = TypeDetailMaster.TypeMaster_ID";
                    sql += @" where TypeMaster.TypeMasterName = '" + TypeName + "'";

                    IQuery iquery = session.CreateSQLQuery(sql);
                    list = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                    Hashtable hashColumn = (Hashtable)list[0];
                    id = Convert.ToInt32(hashColumn["TypeMaster_ID"]);
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
            return id;
        }
        public static string GetSingleColumnValue(string TableName, string GetColumnName, List<string[]> WhereCondition = null, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            string returnColumn = "";
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    var sql = " select top 1 " + GetColumnName + "";
                    sql += @" from " + TableName + "";

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

                    IQuery iquery = session.CreateSQLQuery(sql);
                    returnColumn = iquery.UniqueResult().ToString();
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
            return returnColumn;
        }

        public static Invoice GetReportInvoice(int serviceItemId, string CreatedUserId, ISession session = null)
        {
            Invoice invoice = new Invoice();
            bool IsNewSession = session == null ? true : false;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                IList paymentReceivedList = Common.GetReportPaymentReceived(serviceItemId, session);
                IList serviceItemDetailList = Common.GetReportServiceItemDetail(serviceItemId, session);
                Hashtable reportParameter = (Hashtable)Common.GetReportTable(serviceItemId, session)[0];

                if (reportParameter.Count > 0 && serviceItemDetailList.Count > 0)
                {
                    //Parameters
                    if (reportParameter["CompanyName"] != null)
                        invoice.CompanyName = reportParameter["CompanyName"].ToString();
                    if (reportParameter["Address1"] != null)
                        invoice.CompanyAddress = reportParameter["Address1"].ToString();
                    if (reportParameter["Address2"] != null)
                        invoice.CompanyAddress += " , " + reportParameter["Address2"].ToString();
                    if (reportParameter["Address3"] != null)
                        invoice.CompanyAddress += " , " + reportParameter["Address3"].ToString();
                    if (reportParameter["ContactNo1"] != null)
                        invoice.CompanyContactNo = reportParameter["ContactNo1"].ToString();
                    if (reportParameter["ContactNo2"] != null)
                        invoice.CompanyContactNo += " , " + reportParameter["ContactNo2"].ToString();

                    invoice.CustomerId = reportParameter["CustomerID"].ToString();
                    invoice.ServiceItemId = reportParameter["ServiceItemID"].ToString();
                    invoice.PrintDateTime = DateTime.Now.ToString();

                    invoice.CustomerName = reportParameter["CustomerName"].ToString();
                    invoice.ItemName = reportParameter["ItemName"].ToString();
                    invoice.ItemReceivedDateTime = reportParameter["ItemOrderDate"].ToString();

                    if (reportParameter["ItemExpectedDeliverDate"] != null)
                        invoice.ItemDeliverDateTime = reportParameter["ItemExpectedDeliverDate"].ToString();

                    if (reportParameter["ReceivedDateTime"] != null)
                        invoice.DeliveredDateTime = reportParameter["ReceivedDateTime"].ToString();

                    //Detail Table
                    Decimal detailTotal = 0;
                    IList<InvoiceDetail> detailList = new List<InvoiceDetail>();
                    InvoiceDetail invoiceDetail = new InvoiceDetail();
                    foreach (var list in serviceItemDetailList)
                    {
                        Hashtable hTable = (Hashtable)list;
                        invoiceDetail = new InvoiceDetail();
                        invoiceDetail.Type = "ItemDetail";
                        if (hTable["Comments"] != null)
                            invoiceDetail.Comments = hTable["Comments"].ToString();
                        if (hTable["StatusType"] != null)
                            invoiceDetail.StatusType = hTable["StatusType"].ToString();
                        invoiceDetail.Amount = hTable["Amount"].ToString();
                        detailTotal += Convert.ToDecimal("0" + hTable["Amount"].ToString());
                        detailList.Add(invoiceDetail);
                    }

                    invoice.ItemDetailTotalAmount = detailTotal.ToString();

                    if (paymentReceivedList.Count > 0)
                    {
                        //Payment Table
                        Decimal paymentTotalAmount = 0;
                        foreach (var pList in paymentReceivedList)
                        {
                            Hashtable pTable = (Hashtable)pList;
                            invoiceDetail = new InvoiceDetail();
                            invoiceDetail.Type = "Payment";
                            if (pTable["PaymentReferenceNo"] != null)
                                invoiceDetail.RefNo = pTable["PaymentReferenceNo"].ToString();
                            if (pTable["PaymentType"] != null)
                                invoiceDetail.PaymentType = pTable["PaymentType"].ToString();
                            invoiceDetail.ReceivedDateTime = pTable["ReceivedDateTime"].ToString();
                            string paymentAmount = pTable["Amount"].ToString();
                            invoiceDetail.Amount = paymentAmount;
                            paymentTotalAmount += Convert.ToDecimal("0" + pTable["Amount"]);
                            detailList.Add(invoiceDetail);
                        }
                        invoice.PaidAmount = paymentTotalAmount.ToString();
                        invoice.Balance = (detailTotal - paymentTotalAmount).ToString();
                    }

                    invoice.InvoiceDetail = new List<InvoiceDetail>();
                    invoice.InvoiceDetail = detailList;
                    invoice.Created_UserId = CreatedUserId;
                    invoice = Invoice.Save(invoice, session);
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
            return invoice;
        }

        public static IList GetReportTable(int ServiceItemID, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IList result = null;
            try
            {
                if (ServiceItemID > 0)
                {
                    if (IsNewSession) session = NHibernateHelper.OpenSession();

                    var sql = " select detailCustomerCode.TypeName +''+ CONVERT(VARCHAR,CustomerMaster.CustomerCode) as CustomerID,";
                    sql += @" CustomerMaster.CustomerName,";
                    sql += @" ServiceItemMaster.Brand +' '+ ServiceItemMaster.Model as ItemName,";
                    sql += @" detailServiceCode.TypeName +''+ CONVERT(VARCHAR,ServiceItemMaster.ServiceItemMaster_ID) as ServiceItemID,";
                    sql += @" CONVERT(VARCHAR,ServiceItemMaster.ItemOrderDate) as ItemOrderDate,";
                    sql += @" CONVERT(VARCHAR,ServiceItemMaster.ItemExpectedDeliverDate) as ItemExpectedDeliverDate,";
                    sql += @" ISNULL(ItemReceivedHandler.ItemReceivedHandler_ID,0)as ItemReceivedHandler_ID,";
                    sql += @" CONVERT(VARCHAR,ItemReceivedHandler.ReceivedDateTime) as ReceivedDateTime,";
                    sql += @" AddressMaster.Address1,";
                    sql += @" AddressMaster.Address2,";
                    sql += @" AddressMaster.Address3,";
                    sql += @" AddressMaster.ContactNo1,";
                    sql += @" AddressMaster.ContactNo2,";
                    sql += @" CompanyMaster.CompanyName,";
                    sql += @" CompanyMaster.CompanyLogo";
                    sql += @" from ServicePro.ServiceItemMaster";
                    sql += @" inner join ServicePro.CustomerMaster on CustomerMaster.CustomerMaster_ID = ServiceItemMaster.CustomerMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster as detailCustomerCode on detailCustomerCode.TypeDetailMaster_ID = CustomerMaster.CustomerCodeTemplate";
                    sql += @" inner join Master.TypeDetailMaster as detailServiceCode on detailServiceCode.TypeDetailMaster_ID = ServiceItemMaster.ServiceCodeTemplate";
                    sql += @" left outer join ServicePro.ItemReceivedHandler on ItemReceivedHandler.ServiceItemMaster_ID = ServiceItemMaster.ServiceItemMaster_ID";
                    sql += @" inner join Master.CompanyMaster on CompanyMaster.CompanyMaster_ID = detailCustomerCode.CompanyMaster_ID";
                    sql += @" inner join ServicePro.AddressMaster on AddressMaster.Ref_ID = CompanyMaster.CompanyMaster_ID";
                    sql += @" AND AddressMaster.CodeTemplate IS NULL";
                    sql += @" inner join Master.TypeDetailMaster as detailAddressType on detailAddressType.TypeDetailMaster_ID = AddressMaster.AddressType";
                    sql += @" where ServiceItemMaster.ServiceItemMaster_ID = '" + ServiceItemID + "'";
                    sql += @" and ServiceItemMaster.IsActive = '1'";
                    sql += @" and AddressMaster.IsActive = '1'";
                    sql += @" and detailAddressType.TypeName = 'Billing'";

                    IQuery iquery = session.CreateSQLQuery(sql);
                    result = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex.Message);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return result;
        }
        public static IList GetReportServiceItemDetail(int ServiceItemID, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IList result = null;
            try
            {
                if (ServiceItemID > 0)
                {
                    if (IsNewSession) session = NHibernateHelper.OpenSession();

                    var sql = " select Comments,";
                    sql += @" TypeName as StatusType,";
                    sql += @" PaymentReceivable.Amount";
                    sql += @" from ServicePro.ServiceItemDetail";
                    sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = ServiceItemDetail.ServiceItemMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = ServiceItemDetail.StatusType";
                    sql += @" inner join ServicePro.PaymentReceivable on PaymentReceivable.ServiceItemDetail_ID = ServiceItemDetail.ServiceItemDetail_ID";
                    sql += @" where ServiceItemMaster.ServiceItemMaster_ID = '" + ServiceItemID + "'";
                    sql += @" and ServiceItemMaster.IsActive = '1'";
                    sql += @" and ServiceItemDetail.IsActive = '1'";


                    IQuery iquery = session.CreateSQLQuery(sql);
                    result = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex.Message);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return result;
        }
        public static IList GetReportPaymentReceived(int ServiceItemID, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            IList result = null;
            try
            {
                if (ServiceItemID > 0)
                {
                    if (IsNewSession) session = NHibernateHelper.OpenSession();

                    var sql = " select PaymentReceived.PaymentReferenceNo,";
                    sql += @" TypeName as PaymentType,";
                    sql += @" PaymentReceived.ReceivedDateTime,";
                    sql += @" PaymentReceived.Amount";
                    sql += @" from ServicePro.PaymentReceived";
                    sql += @" inner join ServicePro.ServiceItemMaster on ServiceItemMaster.ServiceItemMaster_ID = PaymentReceived.ServiceItemMaster_ID";
                    sql += @" inner join Master.TypeDetailMaster on TypeDetailMaster.TypeDetailMaster_ID = PaymentReceived.PaymentType";
                    sql += @" where ServiceItemMaster.ServiceItemMaster_ID = '" + ServiceItemID + "'";
                    sql += @" and PaymentReceived.IsActive = '1'";
                    sql += @" and ServiceItemMaster.IsActive = '1'";

                    IQuery iquery = session.CreateSQLQuery(sql);
                    result = iquery.SetResultTransformer(Transformers.AliasToEntityMap).List();
                }
            }
            catch (Exception ex)
            {
                CommonUtil.LogError(ex.Message);
            }
            finally
            {
                if (IsNewSession)
                    session.Dispose();
            }
            return result;
        }
    }
}