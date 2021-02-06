using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.Providers;

public partial class MasterTimeSlot : System.Web.UI.Page
{
    ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            mvwForm.ActiveViewIndex = 0;
            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
            DynamicControlBinding.BindDropDown(ddlDayOfWeek, CommonHelper.GetDayNames(), "Value", "Key", false);            
            ddlDayOfWeek.SelectedValue = (DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek).ToString();
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        mvwForm.ActiveViewIndex = 1;
        gvwData.DataBind();
    }

    protected void gvwData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.HideGridViewRowId(0, e);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        classProvider.AddTimeSlot(
            Convert.ToInt32(ddlBranch.SelectedValue),
            Convert.ToInt32(ddlDayOfWeek.SelectedValue),
            txtTimeStart.Text + "-" + txtTimeEnd.Text);
        gvwData.DataBind();
        txtTimeStart.Text = String.Empty;
        txtTimeEnd.Text = String.Empty;
        txtTimeStart.Focus();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int[] id = WebFormHelper.GetRowIdForDeletion(gvwData);
        classProvider.DeleteTimeSlot(id);
        gvwData.DataBind();
    }
    protected void sdsTimeSlot_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
        e.Command.Parameters["@DayOfWeek"].Value = ddlDayOfWeek.SelectedValue;
    }
}