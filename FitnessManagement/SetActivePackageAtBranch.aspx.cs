using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

public partial class SetActivePackageAtBranch : System.Web.UI.Page
{
    PackageProvider packageProvider = UnityContainerHelper.Container.Resolve<PackageProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            foreach (var package in packageProvider.GetAll())
                ddlPackage.Items.Add(new ListItem(package.Name, package.ID.ToString()));
            ddlPackage.Items.Insert(0, "Select package");
            btnSave.Enabled = false;
        }
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPackage.SelectedIndex > 0)
        {
            cblBranches.DataSource = branchProvider.GetAll();
            cblBranches.DataValueField = "ID";
            cblBranches.DataTextField = "Name";
            cblBranches.DataBind();

            foreach (ListItem item in cblBranches.Items)
                item.Selected = false;

            int[] branchesID = packageProvider.GetBranchesByPackage(Convert.ToInt32(ddlPackage.SelectedValue)).ToArray();
            foreach (var branchID in branchesID)
                cblBranches.Items.FindByValue(branchID.ToString()).Selected = true;
        }
        else
        {
            cblBranches.DataSource = null;
            cblBranches.DataBind();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            IList<int> branchesID = new List<int>();
            foreach (ListItem item in cblBranches.Items)
                if (item.Selected)
                    branchesID.Add(Convert.ToInt32(item.Value));

            packageProvider.UpdatePackagesAtBranch(
                Convert.ToInt32(ddlPackage.SelectedValue),
                branchesID);

            WebFormHelper.SetLabelTextWithCssClass(
                lblMessage,
                String.Format("Settings for <b>{0}</b> saved.", ddlPackage.SelectedItem.Text),
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