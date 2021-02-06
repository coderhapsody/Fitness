using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportMonthlyReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
        DataBindingHelper.PopulateCustomerStatus(ddlStatus, false);
        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("OK"));
    }
}