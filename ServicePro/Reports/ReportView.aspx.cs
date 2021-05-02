using Microsoft.Reporting.WebForms;
using ServicePro.BAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServicePro.Views.Home
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!IsPostBack)
            //    {
            //        if (Request.QueryString["ServiceItemID"] != null)
            //        {
            //            if(Global.UserId != null)
            //            {
            //                int serviceItemID = Convert.ToInt32(Request.QueryString["ServiceItemID"]);
            //                DataTable paymentReceivedTable = Common.GetReportPaymentReceived(serviceItemID, null);
            //                DataTable serviceItemDetailTable = Common.GetReportServiceItemDetail(serviceItemID, null);
            //                ReportViewer1.Reset();
            //                ReportViewer1.LocalReport.ReportPath = "Reports/Report.rdlc";
            //                ReportViewer1.LocalReport.EnableExternalImages = true;
            //                ReportViewer1.LocalReport.DataSources.Clear();
            //                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ItemDetail", serviceItemDetailTable));
            //                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PaymentReceivedDataSet", paymentReceivedTable));

            //                Hashtable reportParameter = (Hashtable)Common.GetReportTable(serviceItemID, null)[0];

            //                //List<string[]> whereList = new List<string[]>();
            //                //whereList.Add(new string[] { "Ref_ID", Global.CompanyId.ToString() });
            //                //whereList.Add(new string[] { "CodeTemplate", null });
            //                //Hashtable companyInfo = (Hashtable)Common.FetchList("ServicePro.AddressMaster", whereList, null)[0];

            //                string imageURL = new Uri(Server.MapPath(Global.CompanyLogo)).AbsoluteUri;
            //                /* Report Parameters */
            //                ReportParameter[] param = new ReportParameter[11];
            //                param[0] = new ReportParameter("CompanyName", Global.CompanyName, true);
            //                param[1] = new ReportParameter("CompanyAddress", reportParameter["Address1"].ToString() + " " + reportParameter["Address2"].ToString() + " " + reportParameter["Address3"].ToString(), true);
            //                param[2] = new ReportParameter("CompanyContactNo", reportParameter["ContactNo1"].ToString() + " , " + reportParameter["ContactNo2"].ToString(), true);
            //                param[3] = new ReportParameter("CustomerID", reportParameter["CustomerID"].ToString(), true);
            //                param[4] = new ReportParameter("ServiceItemID", reportParameter["ServiceItemID"].ToString(), true);
            //                param[5] = new ReportParameter("CustomerName", reportParameter["CustomerName"].ToString(), true);
            //                param[6] = new ReportParameter("ServiceItemName", reportParameter["ItemName"].ToString(), true);
            //                param[7] = new ReportParameter("ItemReceivedDateTime", reportParameter["ItemOrderDate"].ToString(), true);
            //                param[8] = new ReportParameter("ExpectedDeliverDateTime", reportParameter["ItemExpectedDeliverDate"].ToString(), true);
            //                param[9] = new ReportParameter("CompanyLogo", imageURL, true);
            //                if (reportParameter["ReceivedDateTime"] != null)
            //                {
            //                    param[10] = new ReportParameter("ItemHandlerReceivedDateTime", reportParameter["ReceivedDateTime"].ToString(), true);
            //                }
            //                else
            //                {
            //                    param[10] = new ReportParameter("ItemHandlerReceivedDateTime", "", true);
            //                }

            //                ReportViewer1.LocalReport.SetParameters(param);
            //                ReportViewer1.DataBind();
            //                ReportViewer1.LocalReport.Refresh();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    CommonUtil.LogError(ex);
            //}
        }
    }
}