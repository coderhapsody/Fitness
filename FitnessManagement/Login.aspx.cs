using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

public partial class Login : System.Web.UI.Page
{
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            var userNameTextBox = loginApp.FindControl("UserName");
            userNameTextBox.Focus();
            WebFormHelper.InjectSubmitScript(this, null, "Logging in, please wait...", (loginApp.FindControl("LoginButton") as Button), true);                        
        }
    }

    protected void loginApp_LoggedIn(object sender, EventArgs e)
    {
        userProvider.AddSuccessLogInHistory(loginApp.UserName);       

        FormsAuthentication.SetAuthCookie(loginApp.UserName, false);
        MembershipUser user = Membership.GetUser(loginApp.UserName);
        user.LastLoginDate = DateTime.Now;
        Membership.UpdateUser(user);
        //FormsAuthentication.RedirectFromLoginPage(loginApp.UserName, true);
        Response.Redirect(FormsAuthentication.DefaultUrl);
    }

    protected void loginApp_LoginError(object sender, EventArgs e)
    {
        userProvider.AddFailedLogInHistory(loginApp.UserName);       
    }
}