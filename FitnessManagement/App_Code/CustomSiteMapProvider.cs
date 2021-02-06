using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

/// <summary>
/// Summary description for CustomSiteMapProvider
/// </summary>
public class CustomSiteMapProvider : XmlSiteMapProvider
{    
	public CustomSiteMapProvider() 
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
    {
        using (FitnessManagement.Data.FitnessDataContext ctx = new FitnessManagement.Data.FitnessDataContext())
        {
            BranchProvider branchProvider = new BranchProvider(ctx);
            if (branchProvider.GetActiveBranches(HttpContext.Current.User.Identity.Name).Count() == 0)
                return false;
        }

        foreach (string role in node.Roles)
        {
            if (role == "" || role == "*")
                return true;
            if (Roles.IsUserInRole(role))
                return true;

        }

        return false;
    }
}