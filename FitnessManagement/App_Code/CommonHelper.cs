using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

/// <summary>
/// Static helper methods
/// </summary>
public static partial class CommonHelper
{
    public static string CreateApplicationURL(string relativeUrl)
    {
        return String.Format(relativeUrl[0] == '/' ?  "{0}{1}" : "{0}/{1}",
            System.Configuration.ConfigurationManager.AppSettings[ApplicationSettingKeys.ApplicationURL],
            relativeUrl);
    }

    /// <summary>
    /// Converts the current active date time format.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns></returns>
    public static DateTime ConvertToCurrentCulturedDateTime(string date)
    {
        return Convert.ToDateTime(date, System.Threading.Thread.CurrentThread.CurrentCulture);
    }

    /// <summary>
    /// Converts the indonesian date time format.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns></returns>
    public static DateTime ConvertFromCulturedDateTime(string date)
    {
        return Convert.ToDateTime(date, new CultureInfo("id-ID"));
    }

    /// <summary>
    /// Determines whether date is indonesian date format.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>
    ///   <c>true</c> if the date is indonesian date format <c>false</c>.
    /// </returns>
    public static bool IsCulturedDateTime(string date)
    {   
        DateTime dateTime;
        return DateTime.TryParse(date, new CultureInfo("id-ID"), DateTimeStyles.None, out dateTime);
    }

    /// <summary>
    /// Gets the current URL.
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentUrl()
    {
        string[] url = System.Web.HttpContext.Current.Request.Url.AbsolutePath.Split('/');
        string folderName = url[url.Length - 2];
        string menuName = url[url.Length - 1];
        return String.Format("/{0}/{1}",folderName,menuName);
    }

    public static IDictionary<int, string> GetMonthNames()
    {
        IDictionary<int, string> months = new Dictionary<int, string>();
        for (int i = 1; i <= 12; i++)
            months.Add(i, new DateTime(2012, i, 1).ToString("MMMM"));

        return months;
    }

    public static IDictionary<int, string> GetDayNames()
    {
        IDictionary<int, string> days = new Dictionary<int, string>();
        days.Add(1, "Monday");
        days.Add(2, "Tuesday");
        days.Add(3, "Wednesday");
        days.Add(4, "Thursday");
        days.Add(5, "Friday");
        days.Add(6, "Saturday");
        days.Add(7, "Sunday");

        return days;
    }

}