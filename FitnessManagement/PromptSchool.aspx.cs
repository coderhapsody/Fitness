using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PromptSchool : System.Web.UI.Page
{
    #region Template
    public string Sort { get { return ViewState["_Sort"].ToString(); } set { ViewState["_Sort"] = value; } }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["_Prompt.CallbackWidget1"] = Request.QueryString["callback1"];
        ViewState["_Prompt.CallbackWidget2"] = Request.QueryString["callback2"];
        if (!this.IsPostBack)
        {
            txtFindSchoolName.Focus();
        }
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
                    String.Format("javascript:window.opener.document.getElementById('{0}').value='{1}'; window.opener.document.getElementById('{2}').value='{3}'; window.close(); window.opener.document.getElementById('{2}').focus(); return false;", 
                    ViewState["_Prompt.CallbackWidget1"].ToString(), e.Row.Cells[0].Text,
                    ViewState["_Prompt.CallbackWidget2"].ToString(), e.Row.Cells[1].Text));
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwPrompt.DataBind();
    }
    protected void sdsPrompt_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@SchoolName"].Value = "%" + txtFindSchoolName.Text + "%";
    }
}