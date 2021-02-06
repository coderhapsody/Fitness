using FitnessManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary
/// Summary description for SecurityProvider
/// </summary>
public class SecurityProvider
{
    private FitnessDataContext context;

    public SecurityProvider(FitnessDataContext context)
	{
        this.context = context;
	}

    public void UpdateRoleMenu(int menuID, string[] roles)
    {
        context.RoleMenus.DeleteAllOnSubmit(
            context.RoleMenus.Where(rm => rm.MenuID == menuID));
        foreach(string roleName in roles)
        {
            RoleMenu rm = new RoleMenu
            {
                MenuID = menuID,
                RoleName = roleName
            };
            context.RoleMenus.InsertOnSubmit(rm);
        }
        context.SubmitChanges();
    }

    public Menu GetMenu(int id)
    {
        return context.Menus.SingleOrDefault(m => m.ID == id);
    }

    public void AddMenu(string title, string navigationTo, int seq, int parentMenuID, bool isActive)
    {
        Menu menu = new Menu
        {
            Title = title,
            NavigationTo = navigationTo,
            Seq = seq,
            ParentMenuID = parentMenuID == 0 ? (int?)null : parentMenuID,
            IsActive = isActive
        };
        EntityHelper.SetAuditFieldForInsert(menu, HttpContext.Current.User.Identity.Name);
        context.Menus.InsertOnSubmit(menu);
        context.SubmitChanges();

    }

    public void UpdateMenu(int id, string title, string navigationTo, int seq, int parentMenuID, bool isActive)
    {
        Menu menu = context.Menus.SingleOrDefault(m => m.ID == id);
        if(menu != null)
        {
            menu.Title = title;
            menu.NavigationTo = navigationTo;
            menu.Seq = seq;
            menu.ParentMenuID = parentMenuID == 0 ? (int?)null : parentMenuID;
            menu.IsActive = isActive;
        };
        EntityHelper.SetAuditFieldForUpdate(menu, HttpContext.Current.User.Identity.Name);
        context.Menus.InsertOnSubmit(menu);
        context.SubmitChanges();
    }

    public void DeleteMenu(int id)
    {
        Menu menu = context.Menus.SingleOrDefault(m => m.ID == id);
        if (menu != null)
        {
            context.Menus.DeleteOnSubmit(menu);
            context.SubmitChanges();
        }
    }

    public IEnumerable<Menu> GetAllMenus()
    {
        return context.Menus.Where(m => m.IsActive);
    }

    public IEnumerable<Menu> GetAllMenus(int? parentMenuID)
    {
        if (parentMenuID.HasValue)
            return from menu in context.Menus
                   join roleMenu in context.RoleMenus.Where(rm => rm.RoleName ==  Roles.GetRolesForUser()[0]) on menu.ID equals roleMenu.MenuID
                   where menu.IsActive && menu.ParentMenuID.Value == parentMenuID.Value  
                   orderby menu.Seq
                   select menu;

        return from menu in context.Menus
               join roleMenu in context.RoleMenus.Where(rm => rm.RoleName == Roles.GetRolesForUser()[0]) on menu.ID equals roleMenu.MenuID
               where menu.IsActive && !menu.ParentMenuID.HasValue
               orderby menu.Seq
               select menu;
    }

    public IEnumerable<Menu> GetAllMenusIgnoringRole(int? parentMenuID)
    {
        if (parentMenuID.HasValue)
            return from menu in context.Menus                   
                   where menu.IsActive && menu.ParentMenuID.Value == parentMenuID.Value
                   orderby menu.Seq
                   select menu;

        return from menu in context.Menus               
               where menu.IsActive && !menu.ParentMenuID.HasValue
               orderby menu.Seq
               select menu;
    }

    public IEnumerable<string> GetRolesForMenu(int menuID)
    {
        return context.RoleMenus.Where(rm => rm.MenuID == menuID).Select(rm => rm.RoleName);
    }
}