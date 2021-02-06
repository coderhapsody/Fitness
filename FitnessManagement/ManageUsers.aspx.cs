using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Catalyst.Extension;
using Catalyst;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;

public partial class ManageUsers : System.Web.UI.Page, IBasicCommand
{
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        //this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);
        if (!this.IsPostBack)
        {
            mvwForm.ActiveViewIndex = 0;
            ViewState["_Sort"] = "UserName";
            ViewState["_SortDirection"] = SortDirection.Ascending;        
         
            ddlFindRoles.DataSource = Roles.GetAllRoles();
            ddlFindRoles.DataBind();

            ddlRole.DataSource = Roles.GetAllRoles();
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, String.Empty);
            ddlRole.SelectedIndex = 0;

            ddlHomeBranch.DataSource = branchProvider.GetActiveBranches();
            ddlHomeBranch.DataTextField = "Name";
            ddlHomeBranch.DataValueField = "ID";
            ddlHomeBranch.DataBind();
            ddlHomeBranch.Items.Insert(0, String.Empty);
        }
    }

    protected void Buttons_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandArgument.ToString().Equals("DoResetPassword"))
            DoResetPassword();
        else if(e.CommandArgument.ToString().Equals("CancelResetPassword"))
            CancelResetPassword();
        else
            DynamicMethodInvoker.InvokeBasicCommand(e.CommandArgument.ToString(), this);
    }

    private void CancelResetPassword()
    {
        mvwForm.ActiveViewIndex = 0;
    }

    private void DoResetPassword()
    {
        try
        {
            string userName = ViewState["_UserName"].ToString();
            UserManagement.UnlockUser(userName);
            string newPassword = UserManagement.ResetPassword(userName);
            lblNewPassword.Text = String.Format("New Password for <strong>{0}</strong> is <strong style='color:green;'>{1}</strong>", userName, newPassword);
            btnDoReset.Enabled = false;
        }
        catch (Exception ex)
        {
            DynamicControlBinding.SetLabelTextWithCssClass(lblNewPassword, String.Format("<b>Error:</b> {0}", ex.Message), LabelStyleNames.ErrorMessage);            
        }
    }

    void IBasicCommand.Save()
    {
        Page.Validate("AddEdit");
        if(Page.IsValid)
        {
            string userName = ViewState["_UserName"].ToString();
            try
            {
                if(String.IsNullOrEmpty(userName))
                {
                    UserManagement.CreateUserWithRole(
                        txtUserName.Text,
                        txtPassword.Text,
                        txtEmail.Text,
                        ddlRole.SelectedValue);
                    employeeProvider.Add(
                        txtUserName.Text,
                        txtBarcode.Text,
                        txtUserName.Text,
                        Convert.ToInt32(ddlHomeBranch.SelectedValue),
                        txtEmail.Text,
                        false);

                    IList<int> branchesID = new List<int>();
                    branchesID.Add(Convert.ToInt32(ddlHomeBranch.SelectedValue));
                    userProvider.UpdateUserAtBranch(txtUserName.Text, branchesID);
                }
                else
                {
                    UserManagement.UpdateUserDetail(
                        txtUserName.Text,
                        txtEmail.Text,
                        ddlRole.SelectedValue);
                    employeeProvider.Update(
                        txtUserName.Text,
                        txtBarcode.Text,
                        txtUserName.Text,
                        Convert.ToInt32(ddlHomeBranch.SelectedValue),
                        txtEmail.Text);
                }
                userProvider.UpdateUserAtBranch(txtUserName.Text, new List<int>() { Convert.ToInt32(ddlHomeBranch.SelectedValue) });
            }
            catch (Exception ex)
            {
                lblStatus.Text = String.Format("Cannot add/edit user information: {0}", ex.Message);
            }
            mvwForm.ActiveViewIndex = 0;
            ((IBasicCommand) this).Refresh();
        }
    }

    void IBasicCommand.Refresh()
    {
        ViewState["_Users"] = UserManagement.GetAllMembershipUserByRole(ddlFindRoles.SelectedItem.Text);
        BindToGrid(ViewState["_Sort"].ToString(), (SortDirection)ViewState["_SortDirection"]);
    }

    void IBasicCommand.Cancel()
    {
        mvwForm.ActiveViewIndex = 0;
        ((IBasicCommand)this).Refresh();
    }

    void IBasicCommand.Delete()
    {
        gvwMaster.Rows.Cast<GridViewRow>().Where(gridViewRow => (gridViewRow.Cells[gridViewRow.Cells.Count - 1].Controls[1] as CheckBox).Checked).ForEach(
            gridViewRow =>
            {
                string userName = gridViewRow.Cells[0].Text;
                try
                {
                    if (userName.Equals("admin") || userName.Equals(UserManagement.GetCurrentUserName()))
                    {
                        throw new Exception(String.Format("User {0} cannot be deleted.", userName));
                    }
                    UserManagement.DeleteUser(userName);
                    employeeProvider.Delete(userName);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = String.Format("Cannot delete user <strong>{0}</strong>: {1}", userName, ex.Message);
                }
            });
        ((IBasicCommand)this).Refresh();
    }

    void IBasicCommand.AddNew()
    {
        ddlRole.SelectedIndex = 0;
        txtUserName.Text = String.Empty;
        txtPassword.Text = String.Empty;
        txtConfirmPassword.Text = String.Empty;
        txtEmail.Text = String.Empty;
        mvwForm.ActiveViewIndex = 1;
        ViewState["_UserName"] = String.Empty;
        rqvConfirmPassword.Enabled = rqvPassword.Enabled = txtConfirmPassword.Enabled = txtPassword.Enabled = true;
        txtPassword.BackColor = txtConfirmPassword.BackColor = System.Drawing.Color.White;
        txtBarcode.Text = String.Empty;
        ddlHomeBranch.SelectedIndex = 0;
        cuvPassword.Enabled = rqvConfirmPassword.Enabled = rqvPassword.Enabled = true;
        txtUserName.Focus();
    }

    private void BindToGrid(string sortExpression, SortDirection direction)
    {
        var users = ViewState["_Users"] == null ? UserManagement.GetAllMembershipUserByRole(ddlFindRoles.SelectedItem.Text) : 
                                                  ViewState["_Users"] as List<MembershipUser>;

        switch (direction)
        {
            case SortDirection.Ascending:
                users = users.OrderBy(sortExpression).ToList();
                break;
            case SortDirection.Descending:
                users = users.OrderBy(sortExpression + " desc").ToList();                
                break;
        }

        gvwMaster.DataSource = from user in users select new { user.UserName, user.CreationDate, user.LastLoginDate, user.IsLockedOut, user.Email };
        gvwMaster.DataBind();

        ViewState["_Users"] = users;
    }


    protected void gvwMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        string currentSort = ViewState["_Sort"].ToString();
        SortDirection currentSortDirection = (SortDirection)ViewState["_SortDirection"];

        e.SortDirection = e.SortExpression.Equals(currentSort) && currentSortDirection == e.SortDirection ? 
                                 SortDirection.Descending : SortDirection.Ascending;

        ViewState["_Sort"] = e.SortExpression;
        ViewState["_SortDirection"] = e.SortDirection;

        BindToGrid(e.SortExpression, e.SortDirection);
    }

    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)    
    {        
        if (e.CommandName.Equals("EditRow"))
        {            
            string userName = e.CommandArgument.ToString();
            ViewState["_UserName"] = userName;
            mvwForm.ActiveViewIndex = 1;
            MembershipUser user = UserManagement.GetUserDetail(userName);
            Employee emp = employeeProvider.Get(userName);
            txtUserName.Text = user.UserName;
            txtEmail.Text = user.Email;
            ddlRole.SelectedValue = UserManagement.GetRoleByUserName(userName);
            rqvConfirmPassword.Enabled = rqvPassword.Enabled = txtConfirmPassword.Enabled = txtPassword.Enabled = false;
            txtPassword.BackColor = txtConfirmPassword.BackColor = System.Drawing.Color.Gray;
            try
            {
                ddlHomeBranch.SelectedValue = emp.HomeBranchID.ToString();
            }
            catch { }
            txtBarcode.Text = emp.Barcode;

            cuvPassword.Enabled = rqvConfirmPassword.Enabled = rqvPassword.Enabled = false;
            cuvPassword.IsValid = rqvConfirmPassword.IsValid = rqvPassword.IsValid = true;


            txtUserName.Focus();
        }
        else if (e.CommandName.Equals("ResetPassword"))
        {
            mvwForm.ActiveViewIndex = 2;
            lblResetUserName.Text = e.CommandArgument.ToString();
            ViewState["_UserName"] = lblResetUserName.Text;
        }
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.ChangeBackgroundColorRowOnHover(e);
    }


    void IBasicCommand.Read(int id)
    {
        // not used
    }
    protected void cuvPassword_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = args.Value.Length >= 6;
    }
}