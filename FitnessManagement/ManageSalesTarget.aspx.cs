using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Catalyst.Extension;
using FitnessManagement.Data;

public partial class ManageSalesTarget : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    SalesTargetProvider salesTargetProvider = UnityContainerHelper.Container.Resolve<SalesTargetProvider>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            mvwForm.ActiveViewIndex = 0;
            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
            DataBindingHelper.PopulateActiveBranches(ddlFindBranch, User.Identity.Name, false);

            ddlMonth.DataSource = CommonHelper.GetMonthNames();
            ddlMonth.DataTextField = "Value";
            ddlMonth.DataValueField = "Key";
            ddlMonth.DataBind();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();

            for (int year = DateTime.Today.Year - 3; year <= DateTime.Today.Year; year++)
            {
                ddlYear.Items.Add(year.ToString());
                ddlFindYear.Items.Add(year.ToString());
            }
            ddlYear.SelectedValue = DateTime.Today.Year.ToString();
            ddlFindYear.SelectedValue = DateTime.Today.Year.ToString();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.Validate("AddEdit");
        if (this.IsValid)
        {
            try
            {
                switch (RowID)
                {
                    case 0:
                        salesTargetProvider.AddTarget(
                            Convert.ToInt32(ddlBranch.SelectedValue),
                            Convert.ToInt32(ddlYear.SelectedValue),
                            Convert.ToInt32(ddlMonth.SelectedValue),
                            txtFreshMemberUnit.Text.ToDefaultNumber<int>(),
                            txtUpgradeUnit.Text.ToDefaultNumber<int>(),
                            txtRenewalUnit.Text.ToDefaultNumber<int>(),
                            txtFreshMemberRevenue.Text.ToDefaultNumber<decimal>(),
                            txtUpgradeRevenue.Text.ToDefaultNumber<decimal>(),
                            txtRenewalRevenue.Text.ToDefaultNumber<decimal>(),
                            txtPilatesRevenue.Text.ToDefaultNumber<decimal>(),
                            txtVocalRevenue.Text.ToDefaultNumber<decimal>(),
                            txtEFTCollectionRevenue.Text.ToDefaultNumber<decimal>(),
                            txtDropOffUnit.Text.ToDefaultNumber<int>(),
                            txtCancelFees.Text.ToDefaultNumber<decimal>(),
                            txtFreezeUnit.Text.ToDefaultNumber<int>(),
                            txtFreezeFees.Text.ToDefaultNumber<decimal>(),
                            txtOtherRevenue.Text.ToDefaultNumber<decimal>());
                        break;
                    default:
                        salesTargetProvider.UpdateTarget(
                            RowID,
                            Convert.ToInt32(ddlBranch.SelectedValue),
                            Convert.ToInt32(ddlYear.SelectedValue),
                            Convert.ToInt32(ddlMonth.SelectedValue),
                            txtFreshMemberUnit.Text.ToDefaultNumber<int>(),
                            txtUpgradeUnit.Text.ToDefaultNumber<int>(),
                            txtRenewalUnit.Text.ToDefaultNumber<int>(),
                            txtFreshMemberRevenue.Text.ToDefaultNumber<decimal>(),
                            txtUpgradeRevenue.Text.ToDefaultNumber<decimal>(),
                            txtRenewalRevenue.Text.ToDefaultNumber<decimal>(),
                            txtPilatesRevenue.Text.ToDefaultNumber<decimal>(),
                            txtVocalRevenue.Text.ToDefaultNumber<decimal>(),
                            txtEFTCollectionRevenue.Text.ToDefaultNumber<decimal>(),
                            txtDropOffUnit.Text.ToDefaultNumber<int>(),
                            txtCancelFees.Text.ToDefaultNumber<decimal>(),
                            txtFreezeUnit.Text.ToDefaultNumber<int>(),
                            txtFreezeFees.Text.ToDefaultNumber<decimal>(),
                            txtOtherRevenue.Text.ToDefaultNumber<decimal>());
                        break;
                }
                mvwForm.ActiveViewIndex = 0;
                gvwMaster.DataBind();
            }
            catch(Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
            }
        }        
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }
    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            mvwForm.ActiveViewIndex = 1;
            SalesTarget salesTarget = salesTargetProvider.GetTarget(id);
            RowID = id;
            ddlBranch.SelectedValue = salesTarget.BranchID.ToString();
            ddlYear.SelectedValue = salesTarget.Year.ToString();
            ddlMonth.SelectedValue = salesTarget.Month.ToString();
            txtFreshMemberUnit.Text = salesTarget.FreshMemberUnit.ToString(); 
            txtFreshMemberRevenue.Text = salesTarget.FreshMemberRevenue.ToString(); 
            txtRenewalUnit.Text = salesTarget.RenewalUnit.ToString(); 
            txtRenewalRevenue.Text = salesTarget.RenewalRevenue.ToString(); 
            txtUpgradeUnit.Text = salesTarget.UpgradeUnit.ToString(); 
            txtUpgradeRevenue.Text = salesTarget.UpgradeRevenue.ToString(); 
            txtVocalRevenue.Text = salesTarget.VocalRevenue.ToString(); 
            txtPilatesRevenue.Text = salesTarget.PilatesRevenue.ToString();
            txtOtherRevenue.Text = salesTarget.OtherRevenue.ToString();
            txtEFTCollectionRevenue.Text = salesTarget.EFTCollectionRevenue.ToString();
            txtDropOffUnit.Text = salesTarget.DropOffUnit.ToString();            
            txtCancelFees.Text = salesTarget.CancelFees.ToString();
            txtFreezeFees.Text = salesTarget.FreezeFees.ToString();
            txtFreezeUnit.Text = salesTarget.FreezeUnit.ToString();
            txtFreshMemberUnit.Focus();
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        e.Command.Parameters["@Year"].Value = ddlFindYear.SelectedValue;
    }
    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        RowID = 0;
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
        mvwForm.ActiveViewIndex = 1;
        WebFormHelper.ClearTextBox(
            txtFreshMemberUnit,
            txtFreshMemberRevenue,
            txtRenewalUnit,
            txtRenewalRevenue,
            txtUpgradeUnit,
            txtUpgradeRevenue,
            txtVocalRevenue,
            txtPilatesRevenue,
            txtOtherRevenue,
            txtFreezeFees,
            txtFreezeUnit,
            txtEFTCollectionRevenue,
            txtDropOffUnit,
            txtCancelFees);            
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwForm.ActiveViewIndex = 0;
    }
    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
        salesTargetProvider.DeleteTarget(id);
    }
}