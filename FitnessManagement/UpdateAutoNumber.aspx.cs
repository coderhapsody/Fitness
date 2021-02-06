using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class UpdateAutoNumber : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    AutoNumberProvider autoNumberProvider = UnityContainerHelper.Container.Resolve<AutoNumberProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlBranch.DataSource = branchProvider.GetAll();
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items.Insert(0, "Select branch");
            btnSave.Enabled = false;
        }
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedIndex > 0)
            gvwMaster.DataSource = autoNumberProvider.GetByBranch(Convert.ToInt32(ddlBranch.SelectedValue));
        else
            gvwMaster.DataSource = null;
        gvwMaster.DataBind();        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gvwMaster.Rows)
            {
                int lastNumber = Convert.ToInt32((row.Cells[1].FindControl("txtLastNumber") as TextBox).Text);
                string formCode = row.Cells[0].Text;
                autoNumberProvider.Update(formCode, Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(row.Cells[2].Text), lastNumber);
            }

            WebFormHelper.SetLabelTextWithCssClass(
                lblMessage,
                String.Format("Auto number settings for branch <b>{0}</b> saved" ,ddlBranch.SelectedItem.Text),
                LabelStyleNames.AlternateMessage);

        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblMessage,
                ex.Message,
                LabelStyleNames.ErrorMessage);
        }
        
        
    }
}