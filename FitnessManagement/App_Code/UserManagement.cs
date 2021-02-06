using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using FitnessManagement.Data;

public static class UserManagement
{
    public static List<MembershipUser> GetAllMembershipUserByRole(string roleName)
    {
        try
        {
            var users = Roles.GetUsersInRole(roleName);
            IEnumerable<MembershipUser> query = from user in users                        
                    orderby user
                    select Membership.GetUser(user);
                
                
            return query.ToList();    
            
        }
        catch (Exception ex)
        {
            ApplicationLogger.Write(ex);
        }

        return null;
    }

    public static void DeleteRole(string roleName)
    {
        Roles.DeleteRole(roleName, true);
    }

    public static void DeleteUser(string userName)
    {
        Membership.DeleteUser(userName, true);
    }

    public static void CreateUserWithRole(string userName, string password, string email, string roleName)
    {
        try
        {
            var newUser = Membership.CreateUser(userName, password, email);
            newUser.UnlockUser();
            Roles.AddUserToRole(userName, roleName);            
        }
        catch (Exception ex)
        {
            ApplicationLogger.Write(ex);
        }
    }

    public static string GetRoleByUserName(string userName)
    {
        return Roles.GetRolesForUser(userName).FirstOrDefault();
    }

    public static List<string> GetAllRoles()
    {
        return Roles.GetAllRoles().ToList();            
    }

    public static string GetCurrentUserName()
    {
        return Membership.GetUser().UserName;            
    }

    public static void UnlockUser(string userName)
    {
        try
        {
            var user = Membership.GetUser(userName);
            user.UnlockUser();
        }
        catch
        {
        }
    }

    public static void LockUser(string userName)
    {
        try
        {
            var user = Membership.GetUser(userName);                
        }
        catch
        {
        }
    }

    public static void CreateRole(string roleName)
    {
        Roles.CreateRole(roleName);
    }

    public static MembershipUser GetUserDetail(string userName)
    {
        return Membership.GetUser(userName);
    }

    public static void UpdateUserDetail(string userName, string newEmail, string newRoleName)
    {
        MembershipUser user = Membership.GetUser(userName);
        user.Email = newEmail;            
        Membership.UpdateUser(user);
        Roles.RemoveUserFromRole(userName, GetRoleByUserName(userName));
        Roles.AddUserToRole(userName, newRoleName);            
    }

    public static string GetSecurityProviderName()
    {
        return Membership.Provider.Description;
    }

    public static string ResetPassword(string userName)
    {
        return Membership.GetUser(userName).ResetPassword();
    }


}
