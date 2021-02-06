using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;

public partial class ReportClassRunningPerInstructor : System.Web.UI.Page
{
    private InstructorProvider instructorProvider = UnityContainerHelper.Container.Resolve<InstructorProvider>();
    
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


        ddlInstructor.DataSource = instructorProvider.GetAllInstructors();
        ddlInstructor.DataValueField = "ID";
        ddlInstructor.DataTextField = "Name";
        ddlInstructor.DataBind();
    }
}