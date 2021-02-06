using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using FitnessManagement.Configuration;

public partial class InquiryContract : System.Web.UI.Page
{
    CheckInConfiguration checkInConfiguration = new CheckInConfiguration();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            PopulateDropDown();

            calFindDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }
    }

    private void PopulateDropDown()
    {
        DataBindingHelper.PopulateActiveBranches(ddlFindBranch, User.Identity.Name, false);

        DataBindingHelper.PopulateBillingTypes(ddlFindBillingType, true);
        ddlFindBillingType.Items[0].Value = "0";

        DataBindingHelper.PopulatePackages(ddlFindPackage, true);
        ddlFindPackage.Items[0].Value = "0";

        ddlFindStatus.Items.Add(String.Empty);
        ddlFindStatus.Items.Add(new ListItem("Pending", ContractStatus.UNPAID));
        ddlFindStatus.Items.Add(new ListItem("Active", ContractStatus.PAID));
        ddlFindStatus.Items.Add(new ListItem("Closed", ContractStatus.CLOSED));
        ddlFindStatus.Items.Add(new ListItem("Void", ContractStatus.VOID));
        
        ddlFindStatus.SelectedIndex = 0;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        e.Command.Parameters["@ContractNo"].Value = txtFindContractNo.Text;
        e.Command.Parameters["@DateFrom"].Value = calFindDateFrom.SelectedDate.ToString("yyyy-MM-dd");
        e.Command.Parameters["@DateTo"].Value = calFindDateTo.SelectedDate.ToString("yyyy-MM-dd");
        e.Command.Parameters["@CustomerCode"].Value = txtFindBarcode.Text;
        e.Command.Parameters["@PackageID"].Value = ddlFindPackage.SelectedValue;
        e.Command.Parameters["@BillingTypeID"].Value = ddlFindBillingType.SelectedValue;
        e.Command.Parameters["@Status"].Value = ddlFindStatus.SelectedValue;
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.ChangeBackgroundColorRowOnHover(e); 
    }

    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string contractNo = e.Row.Cells[0].Text;
            string reportAgreementForm = checkInConfiguration.ReportAgreementForm;
            HyperLink hypPrint = e.Row.FindControl("hypPrint") as HyperLink;
            if (hypPrint != null)
                hypPrint.Attributes.Add("onclick", String.Format("showSimplePopUp('PrintPreview.aspx?RDL=" + reportAgreementForm +"&ContractNo={0}')", contractNo));

        }
    }
}