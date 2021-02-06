using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class CheckIn : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlBranch.DataSource = branchProvider.GetActiveBranches(User.Identity.Name);
            ddlBranch.DataTextField = "Name";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();            
        }
    }
}