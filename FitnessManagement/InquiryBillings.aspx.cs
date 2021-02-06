using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class InquiryBillings : System.Web.UI.Page
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
                hypBillingDetail.Attributes.Add("onclick", String.Format("showPromptPopUp('ViewBillingDetailHistory.aspx?BatchNo={0}', null, 600, 900)", batchNo));
            }
        }
    }
}