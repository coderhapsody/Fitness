using FitnessManagement.Data;
using FitnessManagement.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;

public partial class MasterSchool : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    SchoolProvider schoolProvider = UnityContainerHelper.Container.Resolve<SchoolProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
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
        txtName.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            schoolProvider.Delete(id);
            Refresh();
        }
        catch (Exception ex)
        {
            mvwForm.ActiveViewIndex = 0;
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
                    schoolProvider.Add(txtName.Text);
                    break;
                default:
                    schoolProvider.Update(RowID, txtName.Text);
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
            School school = schoolProvider.Get(id);
            txtName.Text = school.Name;
            txtName.Focus();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Refresh();
    }

    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@SchoolName"].Value = "%" + txtFindSchoolName.Text + "%";
    }
}