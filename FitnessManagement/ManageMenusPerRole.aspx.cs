using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;


public partial class ManageMenusPerRole : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    SecurityProvider securityProvider = UnityContainerHelper.Container.Resolve<SecurityProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {            
            PopulateMenus();
            btnSave.Visible = false;
        }

        
    }

    private void PopulateMenus()
    {
        IEnumerable<FitnessManagement.Data.Menu> menus = securityProvider.GetAllMenusIgnoringRole(null);
        foreach (FitnessManagement.Data.Menu menu in menus)
        {
            TreeNode menuItem = new TreeNode(menu.Title, menu.ID.ToString());            

            PopulateChildMenu(menuItem, menu.ID);

            tvwMenus.Nodes.Add(menuItem);
            menuItem.Expanded = false;
        }

    }

    private void PopulateChildMenu(TreeNode parentMenu, int menuID)
    {
        IEnumerable<FitnessManagement.Data.Menu> childMenus = securityProvider.GetAllMenusIgnoringRole(menuID);
        foreach (FitnessManagement.Data.Menu childMenu in childMenus)
        {
            TreeNode menuItem = new TreeNode(childMenu.Title, childMenu.ID.ToString());            
            parentMenu.ChildNodes.Add(menuItem);

            PopulateChildMenu(menuItem, childMenu.ID);
        }
    }

    protected void tvwMenus_SelectedNodeChanged(object sender, EventArgs e)
    {
        int menuID = Convert.ToInt32(tvwMenus.SelectedValue);
        RowID = menuID;

        cblRoles.DataSource = UserManagement.GetAllRoles();
        cblRoles.DataBind();

        btnSave.Visible = true;
        
        IEnumerable<string> roles = securityProvider.GetRolesForMenu(menuID);
        foreach (string role in roles)
            cblRoles.Items.FindByText(role).Selected = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string[] selectedRoles = cblRoles.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => item.Value).ToArray();
        securityProvider.UpdateRoleMenu(RowID, selectedRoles);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "_save", "alert('Menu settings saved')", true);
    }
}