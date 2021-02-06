using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Data;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;
using Catalyst.Extension;

public partial class MasterClassRoom : System.Web.UI.Page
{

    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();


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
        txtName.Text = String.Empty;
        txtCode.Text = String.Empty;
        chkIsActive.Checked = true;
        RowID = 0;
        txtName.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            classProvider.DeleteClass(id);
            Refresh();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblMessage,
                ex.Message,
                LabelStyleNames.ErrorMessage);
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
                    classProvider.AddRoom(
                        txtCode.Text,
                        txtName.Text,                        
                        chkIsActive.Checked);
                    break;
                default:
                    classProvider.UpdateRoom(
                        RowID,
                        txtCode.Text,
                        txtName.Text,                        
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
            ClassRoom room = classProvider.GetRoom(id);
            txtCode.Text = room.Code;
            txtName.Text = room.Name;            
            chkIsActive.Checked = room.IsActive;
            txtName.Focus();
        }
    }
}