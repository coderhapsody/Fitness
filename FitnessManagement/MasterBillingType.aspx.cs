using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;

public partial class MasterBillingType : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    BillingTypeProvider billingTypeProvider = UnityContainerHelper.Container.Resolve<BillingTypeProvider>();
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();


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
        txtDescription.Text = String.Empty;
        txtDescription.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            if (!id.Contains(1))
                billingTypeProvider.Delete(id);
            else
                WebFormHelper.SetLabelTextWithCssClass(lblMessage, "<b>Manual Payment</b> cannot be deleted", LabelStyleNames.ErrorMessage);
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
                    billingTypeProvider.Add(
                        txtDescription.Text,
                        Convert.ToInt16(txtAutoPayDay.Text),
                        chkIsActive.Checked);
                    break;
                default:
                    if (chkIsActive.Checked == false && customerProvider.GetCustomersByBillingType(RowID).Count() > 0)
                    {
                        mvwForm.ActiveViewIndex = 0;
                        WebFormHelper.SetLabelTextWithCssClass(lblMessage, "Cannot set this billing type to inactive since there are customers use this billing type", LabelStyleNames.ErrorMessage);
                        return;
                    }
                    billingTypeProvider.Update(
                        RowID,
                        txtDescription.Text,
                        Convert.ToInt16(txtAutoPayDay.Text),
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
            BillingType billingType = billingTypeProvider.Get(id);
            txtDescription.Text = billingType.Description;
            txtAutoPayDay.Text = RowID > 1 ? billingType.AutoPayDay.ToString() : "N/A";
            chkIsActive.Checked = billingType.IsActive;
            txtDescription.Focus();            
            btnSave.Enabled = RowID > 1;
        }
    }
}