using System;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public static class RadHelper
{
    public static int[] GetRowIdForDeletion(RadGrid radGrid)
    {
        return
            radGrid.Items.Cast<GridDataItem>()
                    .Where(row => (row.FindControl("chkDelete") as CheckBox).Checked)
                    .Select(row => Convert.ToInt32(row.GetDataKeyValue("ID")))
                    .ToArray();
    }

    public static void SetUpGrid(RadGrid radGrid)
    {
        radGrid.Columns[0].Display = false;
        radGrid.PageSize = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationSettingKeys.PageSize]);            
    }

    public static void SetUpDatePickers(params RadDatePicker[] datePickers)
    {
        foreach (var radDatePicker in datePickers)
        {
            radDatePicker.MinDate = DateTime.MinValue;
            radDatePicker.MaxDate = DateTime.MaxValue;
            radDatePicker.SelectedDate = DateTime.Today;
        }
    }
}
