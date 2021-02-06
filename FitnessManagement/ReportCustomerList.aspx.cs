using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class ReportCustomerList : System.Web.UI.Page
{
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
            ddlMonth.DataSource = CommonHelper.GetMonthNames();
            ddlMonth.DataTextField = "Value";
            ddlMonth.DataValueField = "Key";
            ddlMonth.DataBind();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();

            int minYear, maxYear;
            try
            {
                customerProvider.GetMinMaxCustomerJoinYear(out minYear, out maxYear);
            }
            catch
            {
                minYear = DateTime.Today.Year;
                maxYear = minYear;
            }

            for (int year = minYear; year <= maxYear; year++)
                ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
            ddlYear.SelectedValue = DateTime.Today.Year.ToString();

        }
    }
}