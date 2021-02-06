using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.IO;

public partial class PrintPreview : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            rptReport.InteractivityPostBackMode = InteractivityPostBackMode.AlwaysAsynchronous;
            string reportName = Request.QueryString["RDL"];
            var keys = Request.QueryString.AllKeys.Where(param => param != "RDL");

            List<ReportParameter> parameters = new List<ReportParameter>();
            foreach (string key in keys)
                parameters.Add(new ReportParameter(key, Request.QueryString[key]));

            if (reportName.Contains("AgreementForm"))
            {
                string termsConditions = File.ReadAllText(MapPath("~/TemplateText/TermsConditions.txt"));
                string presigningNotice = File.ReadAllText(MapPath("~/TemplateText/PresigningNotice.txt"));
                parameters.Add(new ReportParameter("TermsCond", termsConditions));
                parameters.Add(new ReportParameter("PresigningNotice", presigningNotice));
                rptReport.InteractivityPostBackMode = InteractivityPostBackMode.AlwaysSynchronous;
            }
            else if (reportName == "SalesReceipt" || reportName == "SalesReceipt2")
            {
                string footerText = File.ReadAllText(MapPath("~/TemplateText/ReceiptFooterText.txt"));                
                parameters.Add(new ReportParameter("FooterText", footerText));
                rptReport.InteractivityPostBackMode = InteractivityPostBackMode.AlwaysSynchronous;
            }

            ShowReport(reportName, parameters);
        }
    }

    public void ShowReport(string reportName, List<ReportParameter> parameters)
    {
        //rptReport.ServerReport.ReportServerCredentials = new ReportServerCredentials(reportServerUserName, reportServerPassword, reportServerDomain);
        rptReport.Visible = true;
        rptReport.ProcessingMode = ProcessingMode.Remote;
        rptReport.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings[ApplicationSettingKeys.ReportServerURL]);
        rptReport.ServerReport.ReportPath = String.Format(@"/{0}/{1}", ConfigurationManager.AppSettings[ApplicationSettingKeys.ReportFolder], reportName);
        rptReport.ServerReport.SetParameters(parameters);
        rptReport.ServerReport.Refresh();        
    }
}