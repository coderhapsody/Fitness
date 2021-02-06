using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Data;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class MasterCustomerStatus : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    CustomerStatusProvider customerStatusProvider = UnityContainerHelper.Container.Resolve<CustomerStatusProvider>();


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
        int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
        customerStatusProvider.Delete(id);
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
                    customerStatusProvider.Add(
                        txtDescription.Text,
                        colColor.Color,
                        colBgColor.Color);
                    break;
                default:
                    customerStatusProvider.Update(
                        RowID,
                        txtDescription.Text,
                        colColor.Color,
                        colBgColor.Color);
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
            CustomerStatus customerStatus = customerStatusProvider.Get(id);
            txtDescription.Text = customerStatus.Description;
            if (!String.IsNullOrEmpty(customerStatus.Color))
            {
                try
                {
                    colColor.Color = customerStatus.Color.Split('|')[0];
                    colBgColor.Color = customerStatus.Color.Split('|')[1];
                }
                catch { }
            }
            txtDescription.Focus();
        }
    }
}