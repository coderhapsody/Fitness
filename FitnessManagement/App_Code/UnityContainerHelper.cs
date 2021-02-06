using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.UI;

/// <summary>
/// Sets / gets unity container to 
/// </summary>
public static class UnityContainerHelper
{
    public static IUnityContainer Container
    {
        get
        {
            return HttpContext.Current.Application["container"] as UnityContainer;
        }
        set
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["container"] = value;
            HttpContext.Current.Application.UnLock();
        }
    }
}