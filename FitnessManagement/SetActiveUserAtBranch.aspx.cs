using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.Providers;
using System.Web.Security;

public partial class SetActiveUserAtBranch : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlUser.DataSource = from user in Membership.GetAllUsers().Cast<MembershipUser>()
                                 join emp in employeeProvider.GetAll() on user.UserName equals emp.UserName
                                 where !user.IsLockedOut
                                 select new
                                 {
                                     UserName = user.UserName,
                                     Name = employeeProvider.GetName(user.UserName) == String.Empty ? user.UserName : employeeProvider.GetName(user.UserName)
                                 };
            ddlUser.DataTextField = "Name";
            ddlUser.DataValueField = "UserName";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, "Select user");
            btnSave.Enabled = false;
        }
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUser.SelectedIndex > 0)
        {
            cblBranches.DataSource = userProvider.GetCurrentUserBranchID(ddlUser.SelectedValue);
            cblBranches.DataValueField = "ID";
            cblBranches.DataTextField = "Name";
            cblBranches.DataBind();
            foreach (var item in userProvider.GetSelectedBranches(ddlUser.SelectedValue))
                cblBranches.Items.FindByValue(item.ID.ToString()).Selected = true;
        }
        else
        { 
            cblBranches.DataSource = null;
            cblBranches.DataBind();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            IList<int> branchesID = new List<int>();
            foreach (ListItem item in cblBranches.Items)
                if (item.Selected)
                    branchesID.Add(Convert.ToInt32(item.Value));

            userProvider.UpdateUserAtBranch(ddlUser.SelectedValue, branchesID);

            WebFormHelper.SetLabelTextWithCssClass(
                lblMessage,
                String.Format("Settings for <b>{0}</b> saved.", ddlUser.SelectedValue),
                LabelStyleNames.AlternateMessage);
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblMessage,
                ex.Message,
                LabelStyleNames.ErrorMessage);
        }
    }
}