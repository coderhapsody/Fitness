using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catalyst.Patterns;
using FitnessManagement.Data;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;

public partial class _Default : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        using (var ctx = new FitnessDataContext())
        {
            ctx.Connection.Open();
            lblBrowser.Text = String.Format("{0} - {1} {2}", Request.Browser.Type, Request.Browser.Version, Request.Browser.Platform);
            lblDatabase.Text = ctx.Connection.Database;
            lblDatabaseServer.Text = ctx.Connection.DataSource;
            lblServerVersion.Text = ctx.Connection.ServerVersion;
            lblSecurityProvider.Text = UserManagement.GetSecurityProviderName();
        }

        bulBranch.DataSource = userProvider.GetCurrentActiveBranches(User.Identity.Name);
        bulBranch.DataTextField = "Name";
        bulBranch.DataBind();        
    }
}