using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Data;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

public partial class MasterInstructor : System.Web.UI.Page
{

    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    InstructorProvider instructorProvider = UnityContainerHelper.Container.Resolve<InstructorProvider>();

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
        txtCellPhone.Text = txtHomePhone.Text = txtEmail.Text = String.Empty;
        txtName.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
        foreach(var _id in id)
            instructorProvider.DeleteInstructor(_id);
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
                    instructorProvider.AddInstructor(
                        txtBarcode.Text,
                        txtName.Text,
                        calDate.SelectedDate,
                        ddlStatus.SelectedValue,
                        txtEmail.Text,
                        txtHomePhone.Text,
                        txtCellPhone.Text,
                        chkIsActive.Checked);                        
                    break;
                default:
                    instructorProvider.UpdateInstructor(
                        RowID,
                        txtBarcode.Text,
                        txtName.Text,
                        calDate.SelectedDate,
                        ddlStatus.SelectedValue,
                        txtEmail.Text,
                        txtHomePhone.Text,
                        txtCellPhone.Text,
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
            Instructor inst = instructorProvider.GetInstructor(id);
            txtBarcode.Text = inst.Barcode;
            txtName.Text = inst.Name;
            txtHomePhone.Text = inst.HomePhone;
            txtCellPhone.Text = inst.CellPhone;
            txtEmail.Text = inst.Email;
            ddlStatus.SelectedValue = inst.Status;
            chkIsActive.Checked = inst.IsActive;            
            txtName.Focus();
        }
    }
    protected void sqldsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        
    }
}