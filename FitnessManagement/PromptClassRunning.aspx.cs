using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PromptClassRunning : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void gvwData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.HideGridViewRowId(0, e);
    }
    protected void sdsPrompt_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = Request["BranchID"];
        e.Command.Parameters["@Date"].Value = calDate.SelectedDate.ToString("yyyy-MM-dd");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwData.DataBind();
    }
    protected void gvwData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypSelect = e.Row.FindControl("hypSelect") as HyperLink;
            if (hypSelect != null)
            {
                string script = String.Format("javascript:window.opener.document.getElementById('{0}').value='{1}'; window.opener.document.getElementById('{2}').click(); window.close(); return false;",
                    Request["callback"],
                    e.Row.Cells[0].Text,
                    Request["button"]);
                hypSelect.Attributes.Add("onclick", script);
            }
        }
    }
}