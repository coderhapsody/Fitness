using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterWorkspace : System.Web.UI.MasterPage
{    

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (BrowserCompatibility.IsUplevel)
        {
            Page.ClientTarget = "uplevel";            
        }

        //Page.Header.DataBind();
        //Page.DataBind();

        PopulateMenus();
    }

    private void PopulateMenus()
    {
        
    }


    protected override void AddedControl(Control control, int index)
    {
        // This is necessary because Safari and Chrome browsers don't display the Menu control correctly.
        if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
            this.Page.ClientTarget = "uplevel";

        base.AddedControl(control, index);
    } 
}
