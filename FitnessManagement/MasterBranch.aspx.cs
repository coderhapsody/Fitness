using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Data;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class MasterBranch : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetGridViewPageSize(gvwMaster);
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
        WebFormHelper.ClearTextBox(txtName, txtAddress, txtCode, txtEmail, txtMerchantCode, txtPhone);

        txtName.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            branchProvider.Delete(id);
            Refresh();
        }
        catch (Exception ex)
        {
            mvwForm.ActiveViewIndex = 0;
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
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
                    branchProvider.Add(
                        txtCode.Text.ToUpper(),
                        txtName.Text,
                        txtAddress.Text,
                        txtPhone.Text,
                        txtEmail.Text,
                        txtMerchantCode.Text,
                        chkIsActive.Checked);
                    break;
                default:
                    branchProvider.Update(
                        RowID,
                        txtCode.Text.ToUpper(),
                        txtName.Text,
                        txtAddress.Text,
                        txtPhone.Text,
                        txtEmail.Text,
                        txtMerchantCode.Text,
                        chkIsActive.Checked);
                    break;
            }
            Refresh();
        }
        catch (Exception ex)
        {
            mvwForm.ActiveViewIndex = 0;
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
            Branch branch = branchProvider.Get(id);
            txtCode.Text = branch.Code;
            txtName.Text = branch.Name;
            txtAddress.Text = branch.Address;
            txtEmail.Text = branch.Email;
            txtPhone.Text = branch.Phone;
            txtMerchantCode.Text = branch.MerchantCode;
            chkIsActive.Checked = branch.IsActive;
            txtName.Focus();            
        }
    }
}