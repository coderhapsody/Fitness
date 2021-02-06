using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

public partial class PromptInvoice : System.Web.UI.Page
{
    #region Template
    public string Sort { get { return ViewState["_Sort"].ToString(); } set { ViewState["_Sort"] = value; } }
    #endregion
    
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["_Prompt.CallbackWidget"] = Request.QueryString["callback"];
        if(!this.IsPostBack)
        {
            FillDropDown();
        }
    }

    private void FillDropDown()
    {
        ddlBranch.DataSource = userProvider.GetCurrentActiveBranches(User.Identity.Name);
        ddlBranch.DataTextField = "Name";
        ddlBranch.DataValueField = "ID";
        ddlBranch.DataBind();
        ddlBranch.Enabled = ddlBranch.Items.Count > 0;
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
                    String.Format("javascript:window.opener.document.getElementById('{0}').value='{1}'; window.close(); return false;", ViewState["_Prompt.CallbackWidget"].ToString(), e.Row.Cells[1].Text));
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwPrompt.DataBind();
    }
    protected void sdsPrompt_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
        e.Command.Parameters["@InvoiceType"].Value = ddlInvoiceType.SelectedValue;
        e.Command.Parameters["@DateFrom"].Value = calDateFrom.SelectedDate.Date;
        e.Command.Parameters["@DateTo"].Value = calDateTo.SelectedDate.Date;
    }
}