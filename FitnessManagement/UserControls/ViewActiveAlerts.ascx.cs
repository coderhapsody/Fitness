using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ViewActiveAlerts : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (repAlerts.Items.Count == 0)
        //{
        //    WebFormHelper.SetLabelTextWithCssClass(
        //        lblMessage,
        //        "<h3>No alerts for today</h3>",
        //        LabelStyleNames.AlternateMessage);
        //}
    }
    protected void sdsAlert_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@Date"].Value = DateTime.Today.ToString("yyyy-MM-dd");
    }
}