using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportMembershipBySalesConsultant : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);

        ddlMonth.DataSource = CommonHelper.GetMonthNames();
        ddlMonth.DataTextField = "Value";
        ddlMonth.DataValueField = "Key";
        ddlMonth.DataBind();
        ddlMonth.SelectedValue = DateTime.Today.Month.ToString();

        for (int year = DateTime.Today.Year - 3; year <= DateTime.Today.Year; year++)
            ddlYear.Items.Add(year.ToString());
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }
}