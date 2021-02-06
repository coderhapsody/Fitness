using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportDailyPayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
            calDateFrom.SelectedDate = DateTime.Today;
            calDateTo.SelectedDate = DateTime.Today;
        }
    }
}