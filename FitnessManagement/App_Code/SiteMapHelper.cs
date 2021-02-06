using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for SiteMapHelper
/// </summary>
public static class SiteMapHelper
{
    public static IEnumerable<KeyValuePair<string, string>> GetUrls()
    {
        SiteMapDataSource sm = new SiteMapDataSource();
        return from SiteMapNode node in sm.Provider.RootNode.GetAllNodes()
               where node.HasChildNodes == false &&
                     Convert.ToBoolean(node["isTransactionForm"]) == true
               select new KeyValuePair<string, string>(node.Url.Substring(node.Url.LastIndexOf("/") + 1), node.Title);
    }
}