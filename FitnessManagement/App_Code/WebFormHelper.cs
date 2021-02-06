using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using Catalyst.Extension;
using Catalyst;
using System.Configuration;

public static class WebFormHelper
{

    public static void HideGridViewRowId(GridViewRowEventArgs e, int columnIndex = 0)
    {        
        try
        {
            e.Row.Cells[columnIndex].Visible = e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.DataRow;
        }
        catch { }
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

    public static int[] GetRowIdForDeletion(GridView gridView)
    {
        return gridView.Rows.Cast<GridViewRow>().Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked).Select(row => Convert.ToInt32(row.Cells[0].Text)).ToArray();
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

    public static void ClearState(params Control[] controls)
    {
        controls.ForEach(
            ctl =>
            {
                if (ctl.GetType() == typeof(TextBox))
                    (ctl as TextBox).Text = String.Empty;
                    
            });
    }

    public static void ClearDropDownList(params DropDownList[] dropDownList)
    {
        dropDownList.ForEach(
            ddl =>
            {
                try
                {
                    ddl.Enabled = true;
                    ddl.Text = String.Empty;
                }
                catch
                {}
            }
        );
    }

    public static void ClearCheckBox(params CheckBox[] checkBox)
    {
        checkBox.ForEach(
            chb =>
            {
                try
                {
                    chb.Enabled = true;
                    chb.Checked = false;
                }
                catch 
                {}
            }
            );
    }
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

    public static void SetGridViewPageSize(GridView gridView)
    {
        gridView.PageSize = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationSettingKeys.PageSize]);
    }



    public static void ChangeBackgroundColorRowOnHover(GridViewRowEventArgs e)
    {
        string colorCode = ConfigurationManager.AppSettings[ApplicationSettingKeys.GridViewRolloverColor];
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add(
                "onmouseover",
                String.Format("this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='{0}'", colorCode));

            e.Row.Attributes.Add(
                "onmouseout",
                "this.style.backgroundColor=this.originalstyle;");
        }
    }

    public static void InjectSubmitScript(Page page, string processingMessage, Button submitButton, bool validate)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (validate)
        {
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        }
        sb.Append("this.value = '" + processingMessage + "';");
        sb.Append("this.disabled = true;");
        sb.Append("var i=0;");
        sb.Append("var length=document.forms[0].elements.length;");
        sb.Append("do { ");
        sb.Append("  var el = document.forms[0].elements[i];");
        sb.Append("  if(el.type.toLowerCase()=='button' || el.type.toLowerCase()=='submit')");
        sb.Append("	 {");
        sb.Append("    el.disabled = true;");
        sb.Append("  }");
        sb.Append("  i++;");
        sb.Append("} while( i != length - 1);");
        sb.Append(page.ClientScript.GetPostBackEventReference(submitButton, String.Empty));
        sb.Append(";");
        submitButton.Attributes.Add("onclick", sb.ToString());
    }

    public static void InjectSubmitScript(Page page, string confirmMessage, string processingMessage, Button submitButton, bool validate)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (!String.IsNullOrEmpty(confirmMessage))
        {
            sb.Append("if (confirm('" + confirmMessage + "') == false) { return false; } ");
        }
        if (validate)
        {
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        }
        sb.Append("this.value = '" + processingMessage + "';");
        sb.Append("this.disabled = true;");
        sb.Append("var i=0;");
        sb.Append("var length=document.forms[0].elements.length;");
        sb.Append("do { ");
        sb.Append("  var el = document.forms[0].elements[i];");
        sb.Append("  if(el.type.toLowerCase()=='button' || el.type.toLowerCase()=='submit')");
        sb.Append("	 {");
        sb.Append("    el.disabled = true;");
        sb.Append("  }");
        sb.Append("  i++;");
        sb.Append("} while( i != length - 1);");
        sb.Append(page.ClientScript.GetPostBackEventReference(submitButton, String.Empty));
        sb.Append(";");
        submitButton.Attributes.Add("onclick", sb.ToString());
    }

    public static void BindToControl(Repeater repeater, object bindableObject)
    {
        repeater.DataSource = bindableObject;
        repeater.DataBind();
    }

    public static void BindToControl(CompositeDataBoundControl dataBoundControl, object bindableObject)
    {
        dataBoundControl.DataSource = bindableObject;
        dataBoundControl.DataBind();
    }

    public static void BindToControl(ListControl listControl, object bindableObject, string dataTextField, string dataValueField)
    {
        listControl.DataSource = bindableObject;
        listControl.DataTextField = dataTextField;
        listControl.DataValueField = dataValueField;
        listControl.DataBind();
    }

    public static void BindToControl(ListControl listControl, object bindableObject, string dataTextField, string dataValueField, string dataTextFormatString)
    {
        listControl.DataSource = bindableObject;
        listControl.DataTextField = dataTextField;
        listControl.DataValueField = dataValueField;
        listControl.DataTextFormatString = dataTextFormatString;
        listControl.DataBind();
    }

    public static void FreezeTextBox(WebControl textBoxControl)
    {
        textBoxControl.Attributes.Add("onkeypress", "javascript:return NoType(event);");
    }

    public static void NumberOnlyTextBox(WebControl textBoxControl)
    {
        textBoxControl.Attributes.Add("onkeypress", "javascript:return NumbersOnly(event);");
    }

    public static void NumberOnlyTextBoxes(params TextBox[] textboxes)
    {
        foreach (var textbox in textboxes)
            WebFormHelper.NumberOnlyTextBox(textbox);
    }
}
