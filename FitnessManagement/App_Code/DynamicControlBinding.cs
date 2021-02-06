using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catalyst;
using Catalyst.Extension;
using System.Collections;
using System.Configuration;


public static class DynamicControlBinding
{
    public static void ClearTextBox(params TextBox[] textBox)
    {
        textBox.ForEach(
            txt =>
            {
                try
                {
                    txt.Enabled = true;
                    txt.Text = String.Empty;
                }
                catch { }
            });
    }

    public static void ResetDropDownSelectedIndex(params DropDownList[] dropdown)
    {
        dropdown.ForEach(
            dd =>
            {
                try
                {
                    dd.SelectedIndex = 0;
                }
                catch { }
            });
    }

    public static void SetLabelTextWithCssClass(Label label, string text, string cssClass)
    {
        label.CssClass = cssClass;
        label.Text = text;
    }

    public static void BindDropDown<T>(DropDownList dropdown, string dataTextField, string dataValueField, bool addFirstEmptyItem = false)
    {
        dynamic obj = new StaticMemberDynamicObject(typeof(T));
        IQueryable<T> queryable = obj.All();
        dropdown.DataSource = queryable.ToList();
        dropdown.DataTextField = dataTextField;
        dropdown.DataValueField = dataValueField;
        dropdown.DataBind();

        if (addFirstEmptyItem)
        {
            dropdown.Items.Insert(0, String.Empty);
        }
    }

    public static void BindCheckBoxList(CheckBoxList checkboxlist, IEnumerable enumerable, string dataTextField, string dataValueField)
    {
        checkboxlist.DataSource = enumerable;
        checkboxlist.DataTextField = dataTextField;
        checkboxlist.DataValueField = dataValueField;
        checkboxlist.DataBind();
    }

    public static void BindDropDown(DropDownList dropdown, IEnumerable enumerable, string dataTextField, string dataValueField, bool addFirstEmptyItem = false)
    {
        dropdown.DataSource = enumerable;
        dropdown.DataTextField = dataTextField;
        dropdown.DataValueField = dataValueField;
        dropdown.DataBind();

        if (addFirstEmptyItem)
        {
            dropdown.Items.Insert(0, String.Empty);
        }
    }

    public static void BindGridView<T>(ASPnetControls.PagerV2_8 pager, GridView gridView, string sortExpression)
    {
        Type type = typeof(T);
        dynamic activeRecord = new StaticMemberDynamicObject(type);
        IQueryable<T> obj = activeRecord.All();
        pager.ItemCount = obj.Count();
        gridView.DataSource = activeRecord.GetPaged(sortExpression,
                                                    pager.CurrentIndex,
                                                    gridView.PageSize);
        gridView.DataBind();
    }

    public static void HideGridViewRowId(int columnIndex, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[columnIndex].Visible = e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.DataRow;
        }
        catch { }
    }

    public static string SetGridViewSortExpression(GridViewSortEventArgs e, string currentSort)
    {
        currentSort = e.SortExpression.Equals(currentSort) ? currentSort + " desc" : e.SortExpression;
        return currentSort;
    }

    public static GridViewRow[] GetGridViewCheckedRows(GridView gridView)
    {
        return gridView.Rows.Cast<GridViewRow>().Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked).ToArray();
    }

    public static int[] GetRowIdForDeletion(GridView gridView)
    {
        return gridView.Rows.Cast<GridViewRow>().Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked).Select(row => Convert.ToInt32(row.Cells[0].Text)).ToArray();
    }

    public static void SetUpControls(ASPnetControls.PagerV2_8 pager, GridView gridView)
    {
        pager.CurrentIndex = 1;
        gridView.PageSize = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationSettingKeys.PageSize]);
        pager.PageSize = gridView.PageSize;
    }

    public static void SetUpControls(ASPnetControls.PagerV2_8 pager)
    {
        pager.CurrentIndex = 1;
        pager.PageSize = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationSettingKeys.PageSize]);        
    }

    //public static void SetUpButtonsTooltip(WebControl addNewButton, WebControl deleteButton, WebControl saveButton, WebControl cancelButton, WebControl refreshButton, WebControl selectButton)
    //{
    //    if(addNewButton != null)
    //        addNewButton.ToolTip = ApplicationConfiguration.Value[ApplicationConfigurationKeys.TooltipAddNew];

    //    if(deleteButton != null)
    //        deleteButton.ToolTip = ApplicationConfiguration.Value[ApplicationConfigurationKeys.TooltipDelete];

    //    if(saveButton != null)
    //        saveButton.ToolTip = ApplicationConfiguration.Value[ApplicationConfigurationKeys.TooltipSave];

    //    if(cancelButton != null)
    //        cancelButton.ToolTip = ApplicationConfiguration.Value[ApplicationConfigurationKeys.TooltipCancel];

    //    if(refreshButton != null)
    //        refreshButton.ToolTip = ApplicationConfiguration.Value[ApplicationConfigurationKeys.TooltipRefresh];

    //    if(selectButton != null)
    //        selectButton.ToolTip = ApplicationConfiguration.Value[ApplicationConfigurationKeys.TooltipSelect];
    //}

    public static void ChangeBackgroundColorRowOnHover(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add(
                "onmouseover", 
                String.Format("this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='{0}'", 
                ConfigurationManager.AppSettings[ApplicationSettingKeys.GridViewRolloverColor]));

            e.Row.Attributes.Add(
                "onmouseout", 
                "this.style.backgroundColor=this.originalstyle;");
        }
    }
   
}

//public static class Extension
//{
//    public static void AddCLEditor(this Page page)
//    {
//        page.Header.Controls.Add(
//           new Literal() { Text = @"<link rel='stylesheet' href='CLEditor/jquery.cleditor.css' />" });

//        page.Header.Controls.Add(
//            new Literal() { Text = @"<script type='text/javascript' language='javascript' src='CLEditor/jquery.cleditor.min.js'></script>" });
//    }
//}