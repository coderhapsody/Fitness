using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Data;
using FitnessManagement.Providers;
using FitnessManagement.ViewModels;
using Microsoft.Practices.Unity;
using System.Data;

public partial class ApprovalChangeStatusDocument : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    DocumentProvider documentProvider = UnityContainerHelper.Container.Resolve<DocumentProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            calFindFromDate.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            calFindToDate.SelectedDate = DateTime.Today;

            ddlDocumentType.DataSource = documentProvider.GetAllDocumentTypes();
            ddlDocumentType.DataTextField = "Description";
            ddlDocumentType.DataValueField = "ID";
            ddlDocumentType.DataBind();            
            ddlDocumentType.SelectedIndex = 0;

            ddlFindDocumentType.DataSource = documentProvider.GetAllDocumentTypes();
            ddlFindDocumentType.DataTextField = "Description";
            ddlFindDocumentType.DataValueField = "ID";
            ddlFindDocumentType.DataBind();
            ddlFindDocumentType.Items.Insert(0, new ListItem(String.Empty, "0"));

            ddlFindBranch.DataSource = userProvider.GetCurrentActiveBranches(User.Identity.Name);
            ddlFindBranch.DataTextField = "Name";
            ddlFindBranch.DataValueField = "ID";
            ddlFindBranch.DataBind();
            ddlFindBranch.SelectedIndex = 0;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.Validate("AddEdit");
        if (this.IsValid)
        {
            int documentTypeID = Convert.ToInt32(ddlDocumentType.SelectedValue);
            try
            {
                if (documentProvider.CanChangeStatus(txtCustomerCode.Text))
                {

                    switch (RowID)
                    {
                        case 0:
                            documentProvider.Add(
                                Convert.ToInt32(ddlFindBranch.SelectedValue),
                                DateTime.Today,
                                calStartDate.SelectedDate,
                                chkEndDate.Checked ? calEndDate.SelectedDate : (DateTime?)null,
                                txtCustomerCode.Text,
                                documentTypeID,
                                txtNotes.Text);
                            break;
                        default:
                            documentProvider.Update(
                                RowID,
                                DateTime.Today,
                                calStartDate.SelectedDate,
                                chkEndDate.Checked ? calEndDate.SelectedDate : (DateTime?)null,
                                txtCustomerCode.Text,
                                documentTypeID,
                                txtNotes.Text);
                            break;
                    }

                    mvwForm.ActiveViewIndex = 0;
                    gvwMaster.DataBind();
                }
                else
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblStatus,
                        "Cannot change customer status for any unpaid contract",
                        LabelStyleNames.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblStatus,
                    ex.Message,
                    LabelStyleNames.ErrorMessage);
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwForm.ActiveViewIndex = 0;
        gvwMaster.DataBind();
    }
    protected void btnVoid_Click(object sender, EventArgs e)
    {
        try
        {
            var doc = documentProvider.GetChangeStatusDocument(RowID);
            if(doc != null)
            {
                documentProvider.Void(RowID);
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "_notification",
                    String.Format("alert('Document No. {0} has been marked as VOID');", doc.DocumentNo),
                    true);
            }
            btnSave.Enabled = false;
            btnApprove.Enabled = false;
            btnVoid.Enabled = false;
            btnCancel_Click(sender, e);
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblStatus,
                ex.Message,
                LabelStyleNames.ErrorMessage);
        }
    }
    protected void lnbDelete_Click(object sender, EventArgs e)
    {

    }
    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        mvwForm.ActiveViewIndex = 1;
        RowID = 0;
        lblBranch.Text = ddlFindBranch.SelectedItem.Text;
        WebFormHelper.ClearTextBox(txtCustomerCode, txtNotes);
        ddlDocumentType.Enabled = true;
        ddlDocumentType.SelectedIndex = 0;
        calStartDate.SelectedDate = DateTime.Today;
        calEndDate.SelectedDate = DateTime.Today;
        btnApprove.Enabled = false;
        btnVoid.Enabled = false;
        lblApprovalStatus.Text = "Not Approved";
        lblDocumentNo.Text = "(Generated by System)";
        hypPromptCustomer.Attributes["onclick"] = String.Format("showPromptPopUp('PromptCustomer.aspx?BranchID={0}', this.previousSibling.previousSibling.id, 550, 900);", ddlFindBranch.SelectedValue);
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            var doc = documentProvider.GetChangeStatusDocument(RowID);
            if (doc != null)
            {
                documentProvider.Approve(RowID);
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "_notification",
                    String.Format("alert('Document No. {0} has been marked as APPROVED');", doc.DocumentNo),
                    true);
                lblApprovalStatus.Text = "Approved";
            }
            mvwForm.ActiveViewIndex = 0;
            gvwMaster.DataBind();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblStatus,
                ex.Message,
                LabelStyleNames.ErrorMessage);
        }
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }
    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("EditRow"))
        {
            mvwForm.ActiveViewIndex = 1;
            RowID = Convert.ToInt32(e.CommandArgument);
            ddlDocumentType.Enabled = false;
            ChangeStatusDocument doc = documentProvider.GetChangeStatusDocument(RowID);
            lblBranch.Text = doc.Branch.Name;
            txtCustomerCode.Text = doc.Customer.Barcode;
            txtNotes.Text = doc.Notes;
            ddlDocumentType.SelectedValue = doc.DocumentTypeID.ToString();
            calStartDate.SelectedDate = doc.StartDate;
            chkEndDate.Checked = doc.EndDate.HasValue;
            if (doc.EndDate.HasValue)
                calEndDate.SelectedDate = doc.EndDate.Value;
            else
                calEndDate.Clear();
            lblDocumentNo.Text = String.Format("{0} - {1}", doc.DocumentNo, doc.Date.ToLongDateString());

            if (doc.VoidDate.HasValue)
                lblApprovalStatus.Text = "Void";
            else if (doc.ApprovedDate.HasValue)
                lblApprovalStatus.Text = "Approved";
            else
                lblApprovalStatus.Text = "Not Approved";

            btnVoid.Enabled = !doc.VoidDate.HasValue;
            btnApprove.Enabled = employeeProvider.CanApproveDocument(User.Identity.Name);
            btnApprove.Enabled = !doc.ApprovedDate.HasValue && !doc.VoidDate.HasValue && RowID > 0;
            btnSave.Enabled = !doc.VoidDate.HasValue && !doc.ApprovedDate.HasValue;            
        }
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {        
        e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        e.Command.Parameters["@DocumentTypeID"].Value = ddlFindDocumentType.SelectedValue;
        e.Command.Parameters["@DateFrom"].Value = calFindFromDate.SelectedDate.ToString("yyyy-MM-dd");
        e.Command.Parameters["@DateTo"].Value = calFindToDate.SelectedDate.ToString("yyyy-MM-dd");
        e.Command.Parameters["@CustomerCode"].Value = txtFindCustomerCode.Text;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void cuvCustomerCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !String.IsNullOrEmpty(args.Value) && customerProvider.IsExist(args.Value);
    }
    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = e.Row.DataItem as DataRowView;
            if (row != null)
            {
                if (!Convert.IsDBNull(row["ApprovedDate"]))
                    e.Row.Enabled = Convert.ToBoolean(row["IsLastState"]) == false;

                if (!e.Row.Enabled)
                {
                    e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    e.Row.ToolTip = "This document is already approved and cannot be changed.";
                }
            }
        }
    }
}