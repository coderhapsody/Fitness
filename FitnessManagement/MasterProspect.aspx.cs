using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using Telerik.Web.UI;

public partial class MasterProspect : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    ProspectProvider prospectProvider = UnityContainerHelper.Container.Resolve<ProspectProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetGridViewPageSize(gvwMaster);
            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
            mypMonthYear.SelectedDate = DateTime.Today;
            FillDropDown();
        }
    }

    private void FillDropDown()
    {
        var sources = prospectProvider.GetProspectSources();
        ddlSource.Items.Clear();
        ddlSource.Items.Add(new DropDownListItem(String.Empty));
        foreach (var source in sources)
            ddlSource.Items.Add(new DropDownListItem(source, source));

    }

    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var hypFollowUp = e.Row.FindControl("hypFollowUp") as HyperLink;
            var prospectID = Convert.ToInt32(e.Row.Cells[0].Text);
            if (hypFollowUp != null)
            {
                hypFollowUp.Attributes.Add("onclick", String.Format("showPromptPopUp('ManageProspectFollowUp.aspx?ProspectID={0}', null, 600, 900)", prospectID));
            }
        }
    }

    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            mvwForm.SetActiveView(viwAddEdit);
            var prospect = prospectProvider.GetProspect(id);
            RowID = id;
            hidBranchID.Value = Convert.ToString(prospect.BranchID);

            ddlConsultant.DataSource = employeeProvider.GetSales(false);
            ddlConsultant.DataTextField = "UserName";
            ddlConsultant.DataValueField = "ID";
            ddlConsultant.DataBind();
            ddlConsultant.Items.Insert(0, new DropDownListItem(String.Empty));

            lblBranch.Text = prospect.Branch.Name;
            txtFirstName.Text = prospect.FirstName;
            txtLastName.Text = prospect.LastName;
            dtpDate.SelectedDate = prospect.Date;
            txtPhone1.Text = prospect.Phone1;
            txtPhone2.Text = prospect.Phone2;
            txtEmail.Text = prospect.Email;
            txtIdentityNo.Text = prospect.IdentityNo;
            txtSourceRef.Text = prospect.ProspectRef;
            ddlConsultant.SelectedValue = Convert.ToString(prospect.ConsultantID);
            ddlSource.SelectedValue = prospect.ProspectSource;
            txtNotes.Text = prospect.Notes;

            chkEnableFreeTrial.Checked = prospect.FreeTrialValidFrom.HasValue && prospect.FreeTrialValidTo.HasValue;
            if (chkEnableFreeTrial.Checked)
            {
                dtpFreeTrialFrom.SelectedDate = prospect.FreeTrialValidFrom.GetValueOrDefault(DateTime.Today);
                dtpFreeTrialTo.SelectedDate = prospect.FreeTrialValidTo.GetValueOrDefault(DateTime.Today);
            }

            txtFirstName.Focus();
        }
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            Array.ForEach(id, prospectID => prospectProvider.DeleteProspect(prospectID));
            mvwForm.SetActiveView(viwRead);
            gvwMaster.DataBind();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);            
        }
    }

    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwAddEdit);
        dtpDate.SelectedDate = DateTime.Today;
        hidBranchID.Value = ddlBranch.SelectedValue;
        lblBranch.Text = ddlBranch.SelectedItem.Text;

        ddlConsultant.DataSource = employeeProvider.GetAll(Convert.ToInt32(hidBranchID.Value));
        ddlConsultant.DataTextField = "UserName";
        ddlConsultant.DataValueField = "ID";
        ddlConsultant.DataBind();
        ddlConsultant.Items.Insert(0, new DropDownListItem(String.Empty));

        txtFirstName.Text =
            txtLastName.Text =
                txtEmail.Text =
                    txtIdentityNo.Text =
                        txtNotes.Text = txtPhone1.Text = txtPhone2.Text = txtSourceRef.Text = String.Empty;
        chkEnableFreeTrial.Checked = false;


        txtFirstName.Focus();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Page.Validate("AddEdit");
        if (Page.IsValid)
        {
            try
            {
                prospectProvider.AddOrUpdateProspect(RowID,
                    Convert.ToInt32(hidBranchID.Value),
                    txtFirstName.Text,
                    txtLastName.Text,
                    dtpDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                    txtIdentityNo.Text,
                    txtPhone1.Text,
                    txtPhone2.Text,
                    txtEmail.Text,
                    dtpDateOfBirth.SelectedDate,
                    Convert.ToInt32(ddlConsultant.SelectedValue),
                    ddlSource.SelectedItem.Text,
                    txtSourceRef.Text,
                    txtNotes.Text,
                    chkEnableFreeTrial.Checked,
                    dtpFreeTrialFrom.SelectedDate,
                    dtpFreeTrialTo.SelectedDate);

                mvwForm.SetActiveView(viwRead);
                gvwMaster.DataBind();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);                
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwRead);
        gvwMaster.DataBind();
    }

    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
        e.Command.Parameters["@SearchBy"].Value = ddlSearchBy.SelectedValue;
        e.Command.Parameters["@ProspectName"].Value = txtFindByProspectName.Text;
        e.Command.Parameters["@Month"].Value = mypMonthYear.SelectedDate.GetValueOrDefault(DateTime.Today).Month;
        e.Command.Parameters["@Year"].Value = mypMonthYear.SelectedDate.GetValueOrDefault(DateTime.Today).Year;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
}