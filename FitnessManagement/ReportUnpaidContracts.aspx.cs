using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

public partial class ReportUnpaidContracts : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!this.IsPostBack)
        {
            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
        }
    }
}