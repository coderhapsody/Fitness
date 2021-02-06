using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using Catalyst.Extension;
using System.Data;

public partial class ContractActivation : System.Web.UI.Page
{
    ContractProvider contractProvider = UnityContainerHelper.Container.Resolve<ContractProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlFindBranch.DataSource = branchProvider.GetActiveBranches();
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();

            calContractDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvwMaster.Rows)
        {
            string contractNo = row.Cells[1].Text;
            CheckBox chkActivate = row.FindControl("chkActivate") as CheckBox;
            if (chkActivate != null && !String.IsNullOrEmpty(contractNo))
            {
                if (row.Enabled)
                {
                    if (chkActivate.Checked )
                        contractProvider.ActivateContract(contractNo);
                    else
                        contractProvider.DeActivateContract(contractNo);
                }
            }
        }

        gvwMaster.DataBind();
    }
    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = e.Row.DataItem as DataRowView;
            if (row != null)
            {
                CheckBox chkActivate = e.Row.FindControl("chkActivate") as CheckBox;

                if (chkActivate != null)
                {
                    chkActivate.Checked = !(row["ActiveDate"] is DBNull);
                }
                
                e.Row.Enabled = Convert.ToString(row["Status"]) != "Closed";                    
            }
        }
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }
    protected void sqldsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        e.Command.Parameters["@ContractDateFrom"].Value = calContractDateFrom.SelectedDate.Date;
        e.Command.Parameters["@ContractDateTo"].Value = calContractDateTo.SelectedDate.Date;
        e.Command.Parameters["@CustomerName"].Value = txtCustomerFirstName.Text;
    }
}