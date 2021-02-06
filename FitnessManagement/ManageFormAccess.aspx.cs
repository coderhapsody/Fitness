using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;

public partial class ManageFormAccess : System.Web.UI.Page
{
    FormAccessProvider formAccessProvider = UnityContainerHelper.Container.Resolve<FormAccessProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            lblUserName.Text = "<b>User Name:</b> " + Request.QueryString["UserName"];

            List<string> formUrls = new List<string>();
            foreach (var node in SiteMapHelper.GetUrls())
            {
                lstMenu.Items.Add(new ListItem(node.Value, node.Key));
                formUrls.Add(node.Key);
            }


            lstMenu.Rows = formUrls.Count + 1;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            formAccessProvider.Save(
                Request.QueryString["Username"],
                lstMenu.SelectedValue,
                cblAccess.Items.Cast<ListItem>().Single(a => a.Value == "A").Selected,
                cblAccess.Items.Cast<ListItem>().Single(a => a.Value == "U").Selected,
                cblAccess.Items.Cast<ListItem>().Single(a => a.Value == "D").Selected,
                cblAccess.Items.Cast<ListItem>().Single(a => a.Value == "R").Selected);

            WebFormHelper.SetLabelTextWithCssClass(
                lblStatus,
                String.Format("Forms access for user {0} has been saved.", Request["UserName"]),
                LabelStyleNames.AlternateMessage);
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblStatus,
                ex.Message,
                LabelStyleNames.ErrorMessage);
        }
    }
    protected void lstMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
        var formAccess = formAccessProvider.Get(Request.QueryString["UserName"], lstMenu.SelectedValue);
        if (formAccess != null)
        {
            cblAccess.Items.Cast<ListItem>().Single(item => item.Value == "A").Selected = formAccess.CanAddNew;
            cblAccess.Items.Cast<ListItem>().Single(item => item.Value == "D").Selected = formAccess.CanDelete;
            cblAccess.Items.Cast<ListItem>().Single(item => item.Value == "U").Selected = formAccess.CanUpdate;
            cblAccess.Items.Cast<ListItem>().Single(item => item.Value == "R").Selected = formAccess.CanRead;
        }
        else
        {
            cblAccess.Items.Cast<ListItem>().ToList().ForEach(item => item.Selected = false);
        }
    }
}