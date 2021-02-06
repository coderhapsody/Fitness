using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;
using FitnessManagement.Data;
using System.IO;

public partial class ProcessBillingResult : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    BillingProvider billingProvider = UnityContainerHelper.Container.Resolve<BillingProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlBranch.DataSource = branchProvider.GetAll();
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();

            ddlYear.DataSource = billingProvider.GetYears();
            ddlYear.DataBind();
        }
    }

    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = Convert.ToInt32(ddlBranch.SelectedValue);
        e.Command.Parameters["@ProcessYear"].Value = Convert.ToInt32(ddlYear.SelectedValue);
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string batchNo = e.Row.Cells[1].Text;
            HyperLink hypBillingDetail = e.Row.FindControl("hypBillingDetail") as HyperLink;
            if (hypBillingDetail != null)
            {
                hypBillingDetail.Attributes.Add("onclick", String.Format("showPromptPopUp('ViewBillingDetailHistory.aspx?BatchNo={0}', null, 600, 1000)", batchNo));
            }

            HyperLink hypProcessResult = e.Row.FindControl("hypProcessResult") as HyperLink;
            if (hypProcessResult != null)
            {
                hypProcessResult.Attributes.Add("onclick", String.Format("showPromptPopUp('ProcessBillingResult.aspx?BatchNo={0}', null, 600, 900)", batchNo));
            }
        }
    }
    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ProcessResult")
        {
            mvwForm.ActiveViewIndex = 1;
            string batchNo = Convert.ToString(e.CommandArgument);
            BillingHeader billing = billingProvider.GetBillingInfo(batchNo);
            lblBatchNo.Text = billing.BatchNo;
            lblBillingType.Text = billing.BillingType.Description;
            lblProcessDate.Text = billing.ProcessDate.ToString("ddd, dd-MMM-yyyy HH:mm");
            lblFileName.Text = billing.FileName;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwForm.ActiveViewIndex = 0;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string fileName = new FileInfo(fupDecline.FileName).Name;
        File.Delete(Server.MapPath("~/Temp/") + fileName);
        fupDecline.SaveAs(Server.MapPath("~/Temp/") + fileName);
        int rejectionsCount = billingProvider.ProcessBillingResult(lblBatchNo.Text, Server.MapPath("~/Temp/") + fileName);
        ClientScript.RegisterStartupScript(this.GetType(), "_billing",
            String.Format("alert('{0} rejection(s) billing data has been processed for Billing Batch No. {1} ');", rejectionsCount, lblBatchNo.Text),
            true);
        mvwForm.ActiveViewIndex = 0;
        gvwMaster.DataBind();
    }
}