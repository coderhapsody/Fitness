using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catalyst;

public partial class ManageRoles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ViewState["_Sort"] = "RoleName";
            ViewState["_SortDirection"] = SortDirection.Ascending;
            BindToGrid(null);
        }
    }

    private void BindToGrid(string sortExpression,  SortDirection direction = SortDirection.Ascending)
    {
        var roles = UserManagement.GetAllRoles();
        gvwMaster.DataSource = from role in roles select new { RoleName = role };
        gvwMaster.DataBind();
    }

    protected void Buttons_Command(object sender, CommandEventArgs e)
    {
        DynamicMethodInvoker.DynamicInvoke(e.CommandArgument.ToString(), this);
    }

    private void Save()
    {
        this.Validate();
        if (this.IsValid)
        {
            try
            {
                UserManagement.CreateRole(txtRoleName.Text);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            txtRoleName.Text = String.Empty;
        }        
        BindToGrid(null);
    }

    protected void gridRoles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName.Equals("DeleteRow"))
        {
            string roleName = e.CommandArgument.ToString();
            try
            {
                if (Roles.GetUsersInRole(roleName).Length > 0)
                {
                    lblStatus.Text = String.Format("Cannot delete role name {0} since it already has users associated with it", roleName);
                }
                else
                {
                    try
                    {
                        UserManagement.DeleteRole(roleName);
                    }
                    catch (Exception ex)
                    {
                        ApplicationLogger.Write(ex);
                        DynamicControlBinding.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Write(ex);
                DynamicControlBinding.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
            }
        }
        BindToGrid(null);
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.ChangeBackgroundColorRowOnHover(e);
    }
}