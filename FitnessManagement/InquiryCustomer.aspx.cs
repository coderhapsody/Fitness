using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using Catalyst.Extension;
using System.IO;
using System.Configuration;

public partial class InquiryCustomer : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    private CustomerNotesProvider customerNotesProvider =
        UnityContainerHelper.Container.Resolve<CustomerNotesProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlFindBranch.DataSource = branchProvider.GetActiveBranches();
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();

            chkUnknownBirthDateFrom.Checked = true;
            chkUnknownBirthDateTo.Checked = true;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
        e.Command.Parameters["@Name"].Value = txtFindName.Text;
        e.Command.Parameters["@Surname"].Value = txtFindSurname.Text;
        e.Command.Parameters["@IDCardNo"].Value = txtFindIDCardNo.Text;
        e.Command.Parameters["@DateOfBirthFrom"].Value = chkUnknownBirthDateFrom.Checked ? String.Empty : calFindBirthDateFrom.SelectedDate.ToString("yyyy-MM-dd");
        e.Command.Parameters["@DateOfBirthTo"].Value = chkUnknownBirthDateTo.Checked ? String.Empty : calFindBirthDateTo.SelectedDate.ToString("yyyy-MM-dd");
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);        
    }

    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string custBarcode = e.Row.Cells[0].Text;

            var notes = customerNotesProvider.GetTopNotes(custBarcode);
            var txt = new StringBuilder();
            txt.AppendLine("Notes for " + custBarcode + ": ").AppendLine();
            foreach (var note in notes)
                txt.AppendLine("* " + note.ChangedWhen.ToLongDateString() + " *").AppendLine(note.Notes).AppendLine();
            e.Row.ToolTip = txt.ToString();

            var hypInvoiceHistory = e.Row.FindControl("hypInvoiceHistory") as HyperLink;
            if (hypInvoiceHistory != null)
                hypInvoiceHistory.Attributes.Add("onclick", String.Format("showPromptPopUp('InvoiceHistory.aspx?barcode={0}', null, 600, 1100)", custBarcode));

            var hypNotes = e.Row.FindControl("hypNotes") as HyperLink;
            if (hypNotes != null)
                hypNotes.Attributes.Add("onclick", String.Format("showPromptPopUp('InputCustomerNotes.aspx?barcode={0}&candelete=0', null, 600, 900)", custBarcode));

            var hypDetail = e.Row.FindControl("hypDetail") as HyperLink;
            if (hypDetail != null)
                hypDetail.Attributes.Add("onclick", String.Format("showPromptPopUp('MasterCustomer.aspx?barcode={0}&mode=read', null, 600, 1000)", custBarcode));

            var hypPrint = e.Row.FindControl("hypPrint") as HyperLink;
            if (hypPrint != null)
                hypPrint.Attributes.Add("onclick", String.Format("showSimplePopUp('PrintPreview.aspx?RDL=CustomerInfo&CustomerCode={0}')", custBarcode));

            var hypCheckInHistory = e.Row.FindControl("hypCheckInHistory") as HyperLink;
            if (hypCheckInHistory != null)
            {
                hypCheckInHistory.Attributes.Add("onclick", String.Format("showPromptPopUp('CheckInHistory.aspx?barcode={0}', null, 600, 900)", custBarcode));
            }

        }
    }
}