using System;

public partial class ReportCustomerJoin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);

        }
    }
}