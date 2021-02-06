using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

public partial class Closing : System.Web.UI.Page
{
    MonthlyClosingProvider monthlyClosingProvider = UnityContainerHelper.Container.Resolve<MonthlyClosingProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlYear.DataSource =  monthlyClosingProvider.GetClosingYears();
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Today.Year.ToString();

            ddlMonth.DataSource = CommonHelper.GetMonthNames();
            ddlMonth.DataValueField = "Key";
            ddlMonth.DataTextField = "Value";
            ddlMonth.DataBind();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();

            ddlBranch.DataSource = branchProvider.GetActiveBranches(HttpContext.Current.User.Identity.Name);
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();

            WebFormHelper.InjectSubmitScript(this, "Are you sure want to process closing ?", "Processing...", btnProses, true);
            WebFormHelper.InjectSubmitScript(this, "Are you sure want to undo process closing ?", "Processing...", btnUnProses, true);
        }

        RefreshGrid();
    }

    private void RefreshGrid()
    {
        gvwMaster.DataSource = monthlyClosingProvider.GetAllClosing(
            Convert.ToInt32(ddlBranch.SelectedValue),
            Convert.ToInt32(ddlYear.SelectedValue));
        gvwMaster.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RefreshGrid();
    }

    protected void btnProses_Click(object sender, EventArgs e)
    {
        monthlyClosingProvider.DoMonthlyClosing(
            Convert.ToInt32(ddlBranch.SelectedValue),
            Convert.ToInt32(ddlMonth.SelectedValue),
            Convert.ToInt32(ddlYear.SelectedValue));
        RefreshGrid();
    }
    protected void btnUnProses_Click(object sender, EventArgs e)
    {
        monthlyClosingProvider.UndoMonthlyClosing(
            Convert.ToInt32(ddlBranch.SelectedValue),
            Convert.ToInt32(ddlMonth.SelectedValue),
            Convert.ToInt32(ddlYear.SelectedValue), null);
        RefreshGrid();
    }
}