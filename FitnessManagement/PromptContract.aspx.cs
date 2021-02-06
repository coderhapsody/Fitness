using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class PromptContract : System.Web.UI.Page
{
    #region Template
    public string Sort { get { return ViewState["_Sort"].ToString(); } set { ViewState["_Sort"] = value; } }
    #endregion

    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["_Prompt.CallbackWidget"] = Request.QueryString["callback"];
        if (!this.IsPostBack)
        {
            FillDropDown();
        }
    }

    private void FillDropDown()
    {
        ddlBranch.DataSource =  branchProvider.GetActiveBranches();
        ddlBranch.DataTextField = "Name";
        ddlBranch.DataValueField = "ID";
        ddlBranch.DataBind();

        ddlBranch.SelectedValue = userProvider.GetCurrentActiveBranches(User.Identity.Name).ToList()[0].ID.ToString();
    }

    protected void gvwPrompt_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    protected void gvwPrompt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypSelect = e.Row.FindControl("hypSelect") as HyperLink;
            if (hypSelect != null)
                hypSelect.Attributes.Add(
                    "onclick",
                    String.Format("javascript:window.opener.document.getElementById('{0}').value='{1}'; window.opener.document.getElementById('cphMainContent_btnDummy').click(); window.close(); return false;", ViewState["_Prompt.CallbackWidget"].ToString(), e.Row.Cells[2].Text));
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwPrompt.DataBind();
    }
    protected void sdsPrompt_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;    
    }
}