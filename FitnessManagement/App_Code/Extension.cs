using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using System.Web.UI;

/// <summary>
/// Summary description for Extension
/// </summary>
public static class Extension
{
    public static string ToAgeString(this DateTime dob)
    {
        DateTime dt = DateTime.Now;

        int days = dt.Day - dob.Day;
        if (days < 0)
        {
            dt = dt.AddMonths(-1);
            days += DateTime.DaysInMonth(dt.Year, dt.Month);
        }

        int months = dt.Month - dob.Month;
        if (months < 0)
        {
            dt = dt.AddYears(-1);
            months += 12;
        }

        int years = dt.Year - dob.Year;

        return string.Format("{0} year{1}, {2} month{3} and {4} day{5}",
                             years, (years == 1) ? "" : "s",
                             months, (months == 1) ? "" : "s",
                             days, (days == 1) ? "" : "s");
    }

    public static void ApplyUserSecurity(this Page page, WebControl addNewButton, WebControl deleteButton, WebControl saveButton, WebControl grid)
    {
        FormAccessProvider formAccessProvider = UnityContainerHelper.Container.Resolve<FormAccessProvider>();
        FormAccess formAccess = formAccessProvider.Get(page.User.Identity.Name, VirtualPathUtility.GetFileName(page.AppRelativeVirtualPath));

        if (formAccess != null)
        {
            if (addNewButton != null)
                addNewButton.Enabled = formAccess != null && formAccess.CanAddNew;

            if (deleteButton != null)
                deleteButton.Enabled = formAccess != null && formAccess.CanDelete;

            if (saveButton != null)
                saveButton.Enabled = formAccess != null && formAccess.CanUpdate;

            if (grid != null)
                grid.Enabled = formAccess != null && formAccess.CanRead;
        }
    }
}