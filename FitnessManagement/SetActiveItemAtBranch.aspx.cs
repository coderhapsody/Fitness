using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

public partial class SetActiveItemAtBranch : System.Web.UI.Page
{
    ItemProvider itemProvider = UnityContainerHelper.Container.Resolve<ItemProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            foreach (var item in itemProvider.GetAll())
                ddlItem.Items.Add(new ListItem(String.Format("{0} - {1}", item.Barcode, item.Description), item.ID.ToString()));
            ddlItem.Items.Insert(0, "Select item");
            btnSave.Enabled = false;
        }
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItem.SelectedIndex > 0)
        {
            cblBranches.DataSource = branchProvider.GetAll();
            cblBranches.DataValueField = "ID";
            cblBranches.DataTextField = "Name";
            cblBranches.DataBind();

            foreach (ListItem item in cblBranches.Items)
                item.Selected = false;

            int[] branchesID = itemProvider.GetBranchesByItem(Convert.ToInt32(ddlItem.SelectedValue)).ToArray();
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

            itemProvider.UpdateItemsAtBranch(
                Convert.ToInt32(ddlItem.SelectedValue),
                branchesID);

            WebFormHelper.SetLabelTextWithCssClass(
                lblMessage,
                String.Format("Settings for <b>{0}</b> saved.", ddlItem.SelectedItem.Text),
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