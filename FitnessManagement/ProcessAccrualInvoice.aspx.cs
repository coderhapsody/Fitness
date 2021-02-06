using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;


public partial class ProcessAccrualInvoice : System.Web.UI.Page
{
    private InvoiceProvider invoiceProvider = UnityContainerHelper.Container.Resolve<InvoiceProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillDropDown();
        }
    }

    private void FillDropDown()
    {
        DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
        
        WebFormHelper.BindDropDown(ddlMonth, CommonHelper.GetMonthNames(), "value", "key");
        ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
    
        ddlYear.Items.Clear();
        ddlYear.Items.Add(Convert.ToString(DateTime.Today.Year - 1));
        ddlYear.Items.Add(Convert.ToString(DateTime.Today.Year));
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        if (Page.IsPostBack)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@Month"].Value = ddlMonth.SelectedValue;
            e.Command.Parameters["@Year"].Value = ddlYear.SelectedValue;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            int[] invoiceIDs = gvwMaster.Rows.Cast<GridViewRow>()
                                        .Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked)
                                        .Select(row => Convert.ToInt32(row.Cells[0].Text)).ToArray();
            int totalInvoice = invoiceProvider.ProcessFirstAccrualInvoices(Convert.ToInt32(ddlBranch.SelectedValue), invoiceIDs, DateTime.Today);
            gvwMaster.DataBind();

            WebFormHelper.SetLabelTextWithCssClass(lblStatus,
                String.Format("{0} invoice(s) have been processed successfully.", totalInvoice),
                LabelStyleNames.InfoMessage);
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }
}