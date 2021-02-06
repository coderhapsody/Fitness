using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using Catalyst.Extension;

public partial class MasterDocumentType : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    DocumentTypeProvider docTypeProvider = UnityContainerHelper.Container.Resolve<DocumentTypeProvider>();
    CustomerStatusProvider customerStatusProvider = UnityContainerHelper.Container.Resolve<CustomerStatusProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetGridViewPageSize(gvwMaster);
            DataBindingHelper.PopulateCustomerStatus(ddlCustomerStatus, true);
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
        int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
        docTypeProvider.Delete(id);
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
                    docTypeProvider.Add(
                        txtDescription.Text,
                        chkIsLastState.Checked,
                        ddlCustomerStatus.SelectedValue.ToDefaultNumber<int>());
                    break;
                default:
                    docTypeProvider.Update(
                        RowID,
                        txtDescription.Text,
                        chkIsLastState.Checked,
                        ddlCustomerStatus.SelectedValue.ToDefaultNumber<int>());
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
            DocumentType docType = docTypeProvider.Get(id);
            txtDescription.Text = docType.Description;
            chkIsLastState.Checked = docType.IsLastState;
            ddlCustomerStatus.SelectedValue = !docType.ChangeCustomerStatusIDTo.HasValue ? String.Empty : docType.ChangeCustomerStatusIDTo.Value.ToString();
            txtDescription.Focus();
        }
    }
}