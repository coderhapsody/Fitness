using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using Telerik.Web.UI;

public partial class ManageProspectFollowUp : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    private ProspectProvider prospectProvider = UnityContainerHelper.Container.Resolve<ProspectProvider>();
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        RowID = Convert.ToInt32(Request.QueryString["ProspectID"]);
        if (!Page.IsPostBack)
        {
            FillDropDown();
            dtpDate.SelectedDate = DateTime.Today;
            dtpDate.Enabled = false;

            var prospect = prospectProvider.GetProspect(RowID);
            lblProspectName.Text = String.Format("{0} {1}", prospect.FirstName, prospect.LastName);

            btnAddFollowUp.Enabled = !prospectProvider.IsDeadProspect(RowID);

            ddlFollowUpBy.DataSource = employeeProvider.GetAll(prospect.BranchID);
            ddlFollowUpBy.DataTextField = "UserName";
            ddlFollowUpBy.DataValueField = "ID";
            ddlFollowUpBy.DataBind();
            ddlFollowUpBy.Items.Insert(0, new DropDownListItem(String.Empty));
            try
            {
                ddlFollowUpBy.SelectedValue = Convert.ToString(prospect.ConsultantID);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }

    private void FillDropDown()
    {
        var followUpVias = prospectProvider.GetFollowUpVias();
        ddlFollowUpVia.Items.Clear();
        ddlFollowUpVia.Items.Add(new DropDownListItem(String.Empty));
        foreach (var followUpVia in followUpVias)
            ddlFollowUpVia.Items.Add(new DropDownListItem(followUpVia, followUpVia));

        var outcomes = prospectProvider.GetFollowUpOutcomes();
        ddlOutcome.Items.Clear();
        ddlOutcome.Items.Add(new DropDownListItem(String.Empty));
        foreach (var outcome in outcomes)
            ddlOutcome.Items.Add(new DropDownListItem(outcome, outcome));


    }

    protected void btnAddFollowUp_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            try
            {
                prospectProvider.ValidateFollowUp(
                    Convert.ToInt32(Request.QueryString["ProspectID"]), 
                    ddlOutcome.SelectedValue);
               
                prospectProvider.AddOrUpdateFollowUp(
                    0,
                    Convert.ToInt32(Request.QueryString["ProspectID"]),
                    Convert.ToInt32(ddlFollowUpBy.SelectedValue),
                    dtpDate.SelectedDate.GetValueOrDefault(DateTime.Today),
                    ddlFollowUpVia.SelectedValue,
                    txtResult.Text,
                    ddlOutcome.SelectedValue);

                gvwMaster.DataBind();

                ddlFollowUpVia.SelectedIndex = 0;
                txtResult.Text = String.Empty;
                ddlOutcome.SelectedIndex = 0;

                btnAddFollowUp.Enabled = !prospectProvider.IsDeadProspect(RowID);
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
            }
        }
    }

    protected void grdMaster_ItemCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRow")
        {
            try
            {
                int followUpID = Convert.ToInt32(e.CommandArgument);
                prospectProvider.DeleteFollowUp(followUpID);
                gvwMaster.DataBind();
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
            }
        }
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }
}