using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;

public partial class MasterItem : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    ItemProvider itemProvider = UnityContainerHelper.Container.Resolve<ItemProvider>();
    ItemTypeProvider itemTypeProvider = UnityContainerHelper.Container.Resolve<ItemTypeProvider>();
    ItemAccountProvider itemAccountProvider = UnityContainerHelper.Container.Resolve<ItemAccountProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetGridViewPageSize(gvwMaster);
            FillDropDown();
        }
    }

    private void FillDropDown()
    {
        ddlFindItemType.DataSource = itemTypeProvider.GetAll();
        ddlFindItemType.DataTextField = "Description";
        ddlFindItemType.DataValueField = "ID";
        ddlFindItemType.DataBind();
        ddlFindItemType.Items.Insert(0, new ListItem("All", 0.ToString()));

        ddlItemType.DataSource = itemTypeProvider.GetAll();
        ddlItemType.DataTextField = "Description";
        ddlItemType.DataValueField = "ID";
        ddlItemType.DataBind();
        ddlItemType.Items.Insert(0, new ListItem(String.Empty));

        ddlAccount.Items.Add(String.Empty);
        foreach (var account in itemAccountProvider.GetValuedAccounts())
        {
            ddlAccount.Items.Add(
                new ListItem(
                    String.Format("{0} - {1}", account.AccountNo, account.Description),
                    account.ID.ToString()));
        }
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwAddEdit);
        RowID = 0;
        WebFormHelper.ClearTextBox(txtBarcode, txtDescription, txtUnitPrice);
        ddlAccount.SelectedIndex = 0;
        ddlItemType.SelectedIndex = 0;
        chkIsActive.Checked = true;        
        txtBarcode.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
        itemProvider.Delete(id);
        Refresh();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwRead);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            switch (RowID)
            {
                case 0:
                    itemProvider.Add(
                        txtBarcode.Text,
                        txtDescription.Text,
                        Convert.ToInt32(ddlAccount.SelectedValue),
                        Convert.ToInt32(ddlItemType.SelectedValue),
                        Convert.ToDecimal(txtUnitPrice.Text),
                        chkIsActive.Checked);
                    break;
                default:
                    itemProvider.Update(
                        RowID,
                        txtBarcode.Text,
                        txtDescription.Text,
                        Convert.ToInt32(ddlAccount.SelectedValue),
                        Convert.ToInt32(ddlItemType.SelectedValue),
                        Convert.ToDecimal(txtUnitPrice.Text),
                        chkIsActive.Checked);
                    break;
            }
            Refresh();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    private void Refresh()
    {
        mvwForm.SetActiveView(viwRead);
        gvwMaster.DataBind();
    }

    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            RowID = id;
            mvwForm.SetActiveView(viwAddEdit);
            Item item = itemProvider.Get(id);
            txtDescription.Text = item.Description;
            txtBarcode.Text = item.Barcode;
            txtUnitPrice.Text = item.UnitPrice.ToString();
            ddlAccount.SelectedValue = item.ItemAccountID.ToString();
            ddlItemType.SelectedValue = item.ItemTypeID.ToString();
            chkIsActive.Checked = item.IsActive;
            txtBarcode.Focus();
        }
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
        e.Command.Parameters["@Description"].Value = txtFindDescription.Text;
        e.Command.Parameters["@ItemTypeID"].Value = ddlFindItemType.SelectedValue;
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
}