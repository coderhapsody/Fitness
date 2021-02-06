using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;

public partial class UserControls_ApplicationMenus : System.Web.UI.UserControl
{
    SecurityProvider securityProvider = UnityContainerHelper.Container.Resolve<SecurityProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        PopulateMenus();
    }


    private void PopulateMenus()
    {
        IEnumerable<FitnessManagement.Data.Menu> menus = securityProvider.GetAllMenus(null);
        foreach (FitnessManagement.Data.Menu menu in menus)
        {            
            MenuItem menuItem = new MenuItem(menu.Title, menu.ID.ToString());
            menuItem.NavigateUrl = "~/" + menu.NavigationTo;

            PopulateChildMenu(menuItem, menu.ID);

            menuDefault.Items.Add(menuItem);
        }

    }

    private void PopulateChildMenu(MenuItem parentMenu, int menuID)
    {
        IEnumerable<FitnessManagement.Data.Menu> childMenus = securityProvider.GetAllMenus(menuID);
        foreach (FitnessManagement.Data.Menu childMenu in childMenus)
        {
            MenuItem menuItem = new MenuItem(childMenu.Title, childMenu.ID.ToString());
            menuItem.NavigateUrl = "~/" + childMenu.NavigationTo;
            parentMenu.ChildItems.Add(menuItem);

            PopulateChildMenu(menuItem, childMenu.ID);
        }
    }
}