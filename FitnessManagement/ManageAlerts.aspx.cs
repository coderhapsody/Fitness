using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catalyst;
using FitnessManagement.Data;

public partial class ManageAlerts : System.Web.UI.Page, IBasicCommand
{
    #region Template
    public string Sort { get { return ViewState["_Sort"].ToString(); } set { ViewState["_Sort"] = value; } }
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            ddlFilter.SelectedIndex = 0;
            DynamicControlBinding.SetUpControls(pgrMaster, gvwMaster);                        
            ((IBasicCommand)this).Refresh();

            
        }

        WebFormHelper.InjectSubmitScript(this, "Save this data ?", "Saving, please wait...", btnSave, true);
    }

    protected void Buttons_Command(object sender, CommandEventArgs e)
    {
        DynamicMethodInvoker.InvokeBasicCommand(e.CommandArgument.ToString(), this);
    }

    void IBasicCommand.AddNew()
    {
        mvwForm.SetActiveView(viwAddEdit);
        RowID = 0;
        CalendarPopup1.SelectedValue = DateTime.Today;
        CalendarPopup2.SelectedValue = DateTime.Today.AddDays(7);
        chkActive.Checked = true;
        chkInfinite.Checked = false;
        DynamicControlBinding.ClearTextBox(txtDescription);
        txtDescription.Focus();
    }

    void IBasicCommand.Delete()
    {
        int[] arrayOfID = DynamicControlBinding.GetRowIdForDeletion(gvwMaster);

        try
        {
            using (var ctx = new FitnessDataContext())
            {
                ctx.Alerts.DeleteAllOnSubmit(ctx.Alerts.Where(row => arrayOfID.Contains(row.ID)));
                ctx.SubmitChanges();
            }
        }
        catch (Exception ex)
        {
            DynamicControlBinding.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
            ApplicationLogger.Write(ex);
        }
        ((IBasicCommand)this).Refresh();
    }

    void IBasicCommand.Refresh()
    {
        if (mvwForm.GetActiveView() != viwRead)
            mvwForm.SetActiveView(viwRead);
        gvwMaster.DataBind();
        pgrMaster.Visible = gvwMaster.Rows.Count > 0;
    }

    void IBasicCommand.Cancel()
    {
        ((IBasicCommand)this).Refresh();
    }

    void IBasicCommand.Save()
    {
        if (this.IsValid)
        {
            try
            {
                using (var ctx = new FitnessDataContext())
                {
                    Alert entity = RowID == 0 ? new Alert() : ctx.Alerts.Single(row => row.ID == RowID);
                    entity.Description = txtDescription.Text;
                    entity.StartDate = CalendarPopup1.SelectedValue.Value;
                    entity.EndDate = chkInfinite.Checked ? null : CalendarPopup2.SelectedValue;
                    entity.Active = chkActive.Checked;
                    if (RowID == 0)
                    {
                        EntityHelper.SetAuditFieldForInsert(entity, User.Identity.Name);
                        ctx.Alerts.InsertOnSubmit(entity);                        
                    }
                    else
                        EntityHelper.SetAuditFieldForUpdate(entity, User.Identity.Name);

                    ctx.SubmitChanges();                    
                }

                DynamicControlBinding.SetLabelTextWithCssClass(lblStatus, "Data has been saved.", LabelStyleNames.InfoMessage);
            }
            catch (Exception ex)
            {
                DynamicControlBinding.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
                ApplicationLogger.Write(ex);
            }
            finally
            {
                ((IBasicCommand)this).Refresh();
            }
        }
    }

    protected void Pager_Command(object sender, CommandEventArgs e)
    {
        pgrMaster.CurrentIndex = Convert.ToInt32(e.CommandArgument);
        ((IBasicCommand)this).Refresh();
    }

    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        if (this.IsPostBack)
        {
            e.Command.Parameters["@PageIndex"].Value = pgrMaster.CurrentIndex;
            e.Command.Parameters["@PageSize"].Value = pgrMaster.PageSize;
            e.Command.Parameters["@RecordCount"].Value = 0;
            e.Command.Parameters["@ShowOnlyActiveAlerts"].Value = ddlFilter.SelectedValue;
        }
    }

    protected void sdsMaster_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        pgrMaster.ItemCount = Convert.ToInt32(e.Command.Parameters["@RecordCount"].Value);
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.HideGridViewRowId(0, e);
        DynamicControlBinding.ChangeBackgroundColorRowOnHover(e);
    }

    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("EditRow"))
        {
            RowID = Convert.ToInt32(e.CommandArgument);
            ((IBasicCommand)this).Read(RowID);
        }
    }

    void IBasicCommand.Read(int id)
    {
        mvwForm.SetActiveView(viwAddEdit);
        using (var ctx = new FitnessDataContext())
        {
            Alert entity = ctx.Alerts.Single(row => row.ID == id);
            txtDescription.Text = entity.Description;
            CalendarPopup1.SelectedValue = entity.StartDate;
            chkInfinite.Checked = !entity.EndDate.HasValue;
            CalendarPopup2.SelectedValue = entity.EndDate.HasValue ? entity.EndDate : null;
            chkActive.Checked = entity.Active;
            txtDescription.Focus();
        }
    }
}