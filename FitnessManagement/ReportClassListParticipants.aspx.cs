using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.Providers;
using System.Data;

public partial class ReportClassListParticipants : System.Web.UI.Page
{
    ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();
    InstructorProvider instructorProvider = UnityContainerHelper.Container.Resolve<InstructorProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            var branches = branchProvider.GetActiveBranches(User.Identity.Name);
            PopulateBranches(branches);
        }
    }

    private void PopulateBranches(IEnumerable<Branch> branches)
    {
        ddlBranch.DataSource = branches;
        ddlBranch.DataTextField = "Name";
        ddlBranch.DataValueField = "ID";
        ddlBranch.DataBind();

        calDate.SelectedDate = DateTime.Today;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwSchedule.DataBind();
    }

    protected void sdsSchedule_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        if (this.IsPostBack)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@Date"].Value = calDate.SelectedDate.Date;
        }
    }
    protected void gvwSchedule_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.HideGridViewRowId(0, e);
    }
    protected void gvwSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypViewReport = e.Row.FindControl("hypViewReport") as HyperLink;
            hypViewReport.Attributes.Add("onclick",
                "showSimplePopUp('PrintPreview.aspx?RDL=PaidClassListParticipant&ClassRunningID=" + e.Row.Cells[0].Text + "');");
        }
    }
}