using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ServicePro.BAL
{
    internal class InvoiceService
    {
        #region Invoice
        public Invoice SaveInvoice(Invoice invoice, ISession session = null)
        {
            ITransaction transaction = null;
            bool IsNewSession = session == null ? true : false;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();
                using (transaction = session.BeginTransaction())
                {
                    if (invoice.InvoiceID > 0)
                    {
                        session.SaveOrUpdate(invoice);
                    }
                    else
                    {
                        session.Save(invoice);
                    }

                    foreach(var obj in invoice.InvoiceDetail)
                    {
                        obj.InvoiceID = invoice.InvoiceID;
                        if(obj.InvoiceDetail_ID == 0)
                        {
                            session.Save(obj);
                        }
                        else
                        {
                            session.SaveOrUpdate(obj);
                        }
                    }

                    transaction.Commit();
                    if (transaction.WasCommitted == true)
                    {
                        invoice.ReturnValue = 2;
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
                if (IsNewSession)
                    session.Dispose();
            }
            return invoice;
        }
        public bool DeleteInvoice(int Id)
        {
            bool deleteSuccess = false;
            ISession session = null;
            ITransaction transaction = null;
            try
            {
                using (session = NHibernateHelper.OpenSession())
                {
                    Invoice invoice = GetInvoice(Id, session);
                    if (invoice != null)
                    {
                        using (transaction = session.BeginTransaction())
                        {
                            session.Delete(invoice);
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
        public Invoice GetInvoice(int PrimaryKey, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            Invoice invoice = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();
                invoice = session.Get<Invoice>(PrimaryKey);
                if (invoice.InvoiceID > 0)
                {
                    SetInvoice(invoice, session);
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
        protected Invoice SetInvoice(Invoice invoice, ISession session = null)
        {
            bool IsNewSession = session == null ? true : false;
            ITransaction transaction = null;
            try
            {
                if (IsNewSession) session = NHibernateHelper.OpenSession();

                using (transaction = session.BeginTransaction())
                {
                    List<string[]> list = new List<string[]>();
                    list.Add(new string[] { "InvoiceID", invoice.InvoiceID.ToString() });
                    invoice.InvoiceDetail = new List<InvoiceDetail>();
                    IList lt = Common.FetchList("ServicePro.InvoiceDetail", list, session);
                    if (lt != null)
                    {
                        foreach (var ht in lt)
                        {
                            Hashtable table = (Hashtable)ht;
                            InvoiceDetail detail = new InvoiceDetail();
                            detail.InvoiceDetail_ID = (int)table["InvoiceDetail_ID"];
                            detail.InvoiceID = (int)table["InvoiceID"];
                            detail.Amount = table["Amount"] == null ? "" : table["Amount"].ToString();
                            detail.Comments = table["Comments"] == null ? "" : table["Comments"].ToString();
                            detail.PaymentType = table["PaymentType"] == null ? "" : table["PaymentType"].ToString();
                            detail.ReceivedDateTime = table["ReceivedDateTime"] == null ? "" : table["ReceivedDateTime"].ToString();
                            detail.RefNo = table["RefNo"] == null ? "" : table["RefNo"].ToString();
                            detail.StatusType = table["StatusType"] == null ? "" : table["StatusType"].ToString();
                            detail.Type = table["Type"].ToString();
                            invoice.InvoiceDetail.Add(detail);
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
                if (IsNewSession)
                    session.Dispose();
            }
            return invoice;
        }
        #endregion
    }
}


