using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using Catalyst.Extension;

public partial class MasterContract : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();
    ContractProvider contractProvider = UnityContainerHelper.Container.Resolve<ContractProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    BillingTypeProvider billingTypeProvider = UnityContainerHelper.Container.Resolve<BillingTypeProvider>();
    PackageProvider packageProvider = UnityContainerHelper.Container.Resolve<PackageProvider>();
    BankProvider bankProvider = UnityContainerHelper.Container.Resolve<BankProvider>();
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();
    AreaProvider areaProvider = UnityContainerHelper.Container.Resolve<AreaProvider>();
    MonthlyClosingProvider monthlyClosingProvider = UnityContainerHelper.Container.Resolve<MonthlyClosingProvider>();
    ItemProvider itemProvider = UnityContainerHelper.Container.Resolve<ItemProvider>();
    SchoolProvider schoolProvider = UnityContainerHelper.Container.Resolve<SchoolProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, null, btnSave, gvwMaster);

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            FillDropDown();
            WebFormHelper.SetGridViewPageSize(gvwMaster);
        }
    }

    private void FillDropDown()
    {
        ddlArea.DataSource = areaProvider.GetAll();
        ddlArea.DataTextField = "Description";
        ddlArea.DataValueField = "ID";
        ddlArea.DataBind();
        ddlArea.Items.Insert(0, new ListItem(String.Empty, "0"));

        ddlFindBranch.DataSource = branchProvider.GetActiveBranches();
        ddlFindBranch.DataTextField = "Name";
        ddlFindBranch.DataValueField = "ID";
        ddlFindBranch.DataBind();


        ddlBillingType.DataSource = billingTypeProvider.GetActiveBillingTypes();
        ddlBillingType.DataTextField = "Description";
        ddlBillingType.DataValueField = "ID";
        ddlBillingType.DataBind();
        ddlBillingType.Items.Insert(0, String.Empty);

        ddlPackage.DataSource = packageProvider.GetPackagesInBranch(Convert.ToInt32(ddlFindBranch.SelectedValue));
        ddlPackage.DataTextField = "Name";
        ddlPackage.DataValueField = "ID";
        ddlPackage.DataBind();
        ddlPackage.Items.Insert(0, String.Empty);

        ddlBillingBank.DataSource = bankProvider.GetActiveBanks();
        ddlBillingBank.DataTextField = "Name";
        ddlBillingBank.DataValueField = "ID";
        ddlBillingBank.DataBind();
        ddlBillingBank.Items.Insert(0, String.Empty);


        ddlCardExpiredMonth.DataSource = CommonHelper.GetMonthNames();
        ddlCardExpiredMonth.DataTextField = "Value";
        ddlCardExpiredMonth.DataValueField = "Key";
        ddlCardExpiredMonth.DataBind();
        ddlCardExpiredMonth.SelectedValue = DateTime.Today.Month.ToString();

        ddlCardExpiredYear.DataSource = Enumerable.Range(2005, DateTime.Today.Year + 10 - 2005);
        ddlCardExpiredYear.DataBind();
        ddlCardExpiredYear.SelectedValue = DateTime.Today.Year.ToString();

        ddlMonthlyDuesItem.DataSource = itemProvider.GetMonthlyDuesItem();
        ddlMonthlyDuesItem.DataTextField = "Description";
        ddlMonthlyDuesItem.DataValueField = "ID";
        ddlMonthlyDuesItem.DataBind();
        ddlMonthlyDuesItem.Items.Insert(0, String.Empty);


        DataBindingHelper.PopulateCreditCardTypes(ddlBillingCardType, true);
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwAddEdit);
        RowID = 0;

        ResetFields();

        btnVoid.Enabled = false;
        btnCloseContract.Enabled = false;
        chkIsTransfer.Enabled = true;

        calDateOfBirth.SelectedDate = DateTime.Today;

        WebFormHelper.ClearTextBox(txtNotes, txtCustomerBarcode, txtCustomerFirstName, txtCustomerLastName, txtDuesAmount, 
            txtFatherEmail, txtFatherName, txtFatherPhone, txtMotherEmail, txtMotherName, txtMotherPhone, txtBillingCardHolderID, txtBillingCardHolderName, txtBillingCardNo);        
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetFields();
        mvwForm.SetActiveView(viwRead);
        gvwMaster.DataBind();
    }

    private void ResetFields()
    {
        chkGenerateNewBarcodeCustomer.Visible = true;
        hypLookUpCustomer.Visible = true;
        chkIsTransfer.Checked = false;
        lblBranch.Text = ddlFindBranch.SelectedItem.Text;
        lblContractNo.Text = "(Generated by System)";
        calDate.SelectedDate = DateTime.Today;
        calEffectiveDate.SelectedDate = DateTime.Today;
        chkGenerateNewBarcodeCustomer.Checked = true;
        ddlPackage.SelectedIndex = 0;
        ddlBillingType.SelectedIndex = 0;
        ddlArea.SelectedIndex = 0;
        txtSchoolID.Text = txtSchoolName.Text = String.Empty;
        lblStatus.Text = "Pending";
        txtCustomerBarcode.ReadOnly = false;
        gvwPackage.DataSource = null;
        gvwPackage.DataBind();
        btnVoid.Enabled = false;
        lblActiveDate.Text = "Not Active";
        calExpiredDate.Enabled = false;
        calNextDuesDate.SelectedDate = calEffectiveDate.SelectedDate.AddMonths(1);
        WebFormHelper.ClearTextBox(txtFatherName, txtFatherPhone, txtHomePhone, txtIDCardNoFather, txtIDCardNoMother, txtMailingAddress, txtMailingZipCode, txtMotherName, txtMotherPhone, txtNotes, txtAddress, txtZipCode);
        chkFather.Checked = false;
        chkMother.Checked = false;
        chkFatherBirthDateUnknown.Checked = true;
        chkMotherBirthDateUnknown.Checked = true;
        ddlMonthlyDuesItem.SelectedIndex = 0;
        ddlRenewalOrUpgrade.SelectedIndex = 0;

        lblClosedDate.Text = "(Generated by System)";
        lblVoidDate.Text = "(Generated by System)";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Validate("AddEdit");
            if (IsValid)
            {
                if (calDateOfBirth.SelectedDate > DateTime.Today)
                {
                    DynamicControlBinding.SetLabelTextWithCssClass(
                                lblMessageAddEdit,
                                "Invalid date of birth",
                                LabelStyleNames.ErrorMessage);
                    return;
                }

                if (ddlBillingType.SelectedItem.Text.ToUpper() == "AUTO PAYMENT")
                {
                    cuvCreditCardNo.Validate();
                    if (cuvCreditCardNo.IsValid)
                    {
                        if (ddlBillingCardType.SelectedIndex == 0 ||
                            ddlBillingBank.SelectedIndex == 0 ||
                           String.IsNullOrEmpty(txtBillingCardHolderID.Text) ||
                           String.IsNullOrEmpty(txtBillingCardHolderName.Text) ||
                           String.IsNullOrEmpty(txtBillingCardNo.Text))
                        {
                            DynamicControlBinding.SetLabelTextWithCssClass(
                                lblMessageAddEdit,
                                "Auto payment information is incomplete",
                                LabelStyleNames.ErrorMessage);
                            return;
                        }
                    }

                    if (txtDuesAmount.Text.ToDefaultNumber<decimal>() == 0 ||
                        ddlMonthlyDuesItem.SelectedValue.ToDefaultNumber<int>() == 0)
                    {
                        DynamicControlBinding.SetLabelTextWithCssClass(
                                lblMessageAddEdit,
                                "Both <b>Dues Amount</b> and <b>Monthly Dues Item</b> fields must be specified if billing type is manual payment",
                                LabelStyleNames.ErrorMessage);
                        return;
                    }
                    
                }

                if (chkGenerateNewBarcodeCustomer.Checked)
                {
                    if (chkFather.Checked && (
                        String.IsNullOrEmpty(txtFatherName.Text)))
                    {
                        DynamicControlBinding.SetLabelTextWithCssClass(
                            lblStatusParent,
                            "Parent Information (Father) is incomplete",
                            LabelStyleNames.ErrorMessage);
                        return;
                    }

                    if (chkMother.Checked && (
                        String.IsNullOrEmpty(txtMotherName.Text)))
                    {
                        DynamicControlBinding.SetLabelTextWithCssClass(
                            lblStatusParent,
                            "Parent Information (Mother) is incomplete",
                            LabelStyleNames.ErrorMessage);
                        return;
                    }
                }


                switch (RowID)
                {
                    case 0:
                        contractProvider.Add(
                            calDate.SelectedDate,
                            !chkGenerateNewBarcodeCustomer.Checked,
                            txtCustomerBarcode.Text,  
                            chkIsTransfer.Checked,
                            txtCustomerFirstName.Text,
                            txtCustomerLastName.Text,
                            calDateOfBirth.SelectedDate,
                            Convert.ToInt32(ddlFindBranch.SelectedValue),
                            Convert.ToInt32(ddlPackage.SelectedValue),
                            null,
                            calEffectiveDate.SelectedDate,
                            Convert.ToInt32(ddlBillingType.SelectedValue),
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingCardType.SelectedValue),
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingBank.SelectedValue),
                            txtBillingCardNo.Text,
                            txtBillingCardHolderName.Text,
                            txtBillingCardHolderID.Text,
                            new DateTime(Convert.ToInt32(ddlCardExpiredYear.SelectedValue), 
                                         Convert.ToInt32(ddlCardExpiredMonth.SelectedValue), 
                                         DateTime.DaysInMonth(Convert.ToInt32(ddlCardExpiredYear.SelectedValue), 
                                                              Convert.ToInt32(ddlCardExpiredMonth.SelectedValue))),
                            'P',
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1  ? 0 : Convert.ToInt32(ddlMonthlyDuesItem.SelectedValue),
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : txtDuesAmount.Text.ToDefaultNumber<decimal>(),
                            calNextDuesDate.SelectedDate,
                            calExpiredDate.SelectedDate,
                            txtHomePhone.Text,
                            txtCellPhone.Text,
                            txtMailingAddress.Text,
                            txtMailingZipCode.Text,
                            txtAddress.Text,
                            txtZipCode.Text,
                            Convert.ToInt32(ddlArea.SelectedValue),
                            txtSchoolID.Text.ToDefaultNumber<int>(),
                            chkFather.Checked,
                            txtFatherName.Text,
                            txtIDCardNoFather.Text,
                            chkFatherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateFather.SelectedDate,
                            txtFatherPhone.Text,
                            txtFatherEmail.Text,
                            chkMother.Checked,
                            txtMotherName.Text,
                            txtIDCardNoMother.Text,
                            chkMotherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateMother.SelectedDate,
                            txtMotherPhone.Text,
                            txtMotherEmail.Text,
                            txtNotes.Text,
                            ddlRenewalOrUpgrade.SelectedValue);
                        break;
                    default:
                        contractProvider.Update(
                            RowID,                            
                            Convert.ToInt32(ddlPackage.SelectedValue),
                            calDate.SelectedDate,
                            calEffectiveDate.SelectedDate,
                            Convert.ToInt32(ddlBillingType.SelectedValue),
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingCardType.SelectedValue),
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlBillingBank.SelectedValue) ,
                            txtBillingCardNo.Text,
                            txtBillingCardHolderName.Text,
                            txtBillingCardHolderID.Text,
                            new DateTime(Convert.ToInt32(ddlCardExpiredYear.SelectedValue),
                                         Convert.ToInt32(ddlCardExpiredMonth.SelectedValue),
                                         DateTime.DaysInMonth(Convert.ToInt32(ddlCardExpiredYear.SelectedValue),
                                                              Convert.ToInt32(ddlCardExpiredMonth.SelectedValue))),
                            txtHomePhone.Text,
                            txtCellPhone.Text,
                            txtMailingAddress.Text,
                            txtMailingZipCode.Text,
                            txtAddress.Text,
                            txtZipCode.Text,
                            Convert.ToInt32(ddlArea.SelectedValue),
                            txtSchoolID.Text.ToDefaultNumber<int>(),
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : Convert.ToInt32(ddlMonthlyDuesItem.SelectedValue),
                            Convert.ToInt32(ddlBillingType.SelectedValue) == 1 ? 0 : txtDuesAmount.Text.ToDefaultNumber<decimal>(),
                            calNextDuesDate.SelectedDate,
                            calExpiredDate.SelectedDate,
                            chkFather.Checked,
                            txtFatherName.Text,
                            txtIDCardNoFather.Text,
                            chkFatherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateFather.SelectedDate,
                            txtFatherPhone.Text,
                            txtFatherEmail.Text,
                            chkMother.Checked,
                            txtMotherName.Text,
                            txtIDCardNoMother.Text,
                            chkMotherBirthDateUnknown.Checked ? (DateTime?)null : calBirthDateMother.SelectedDate,
                            txtMotherPhone.Text,
                            txtMotherEmail.Text,
                            txtNotes.Text);
                        break;
                }
                Refresh();
            }
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    private void Refresh()
    {
        mvwForm.SetActiveView(viwRead);
        gvwMaster.DataBind();
    }

    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);                
                chkGenerateNewBarcodeCustomer.Visible = false;
                Contract contract = contractProvider.Get(id);
                chkGenerateNewBarcodeCustomer.Checked = false;
                lblBranch.Text = contract.Branch.Name;
                lblContractNo.Text = contract.ContractNo;
                calDate.SelectedDate = contract.Date;
                calDateOfBirth.SelectedDate = contract.Customer.DateOfBirth.HasValue ? contract.Customer.DateOfBirth.Value : DateTime.Today;
                txtCustomerBarcode.Text = contract.Customer.Barcode;
                txtCustomerBarcode.ReadOnly = true;
                lblCustomerName.Text = String.Format("{0} {1}", contract.Customer.FirstName.Trim(), contract.Customer.LastName.Trim());
                ddlPackage.SelectedValue = contract.PackageID.ToString();
                ddlPackage_SelectedIndexChanged(sender, null);
                txtHomePhone.Text = contract.Customer.HomePhone;
                txtCellPhone.Text = contract.Customer.CellPhone1;
                calEffectiveDate.SelectedDate = contract.EffectiveDate;
                
                ddlBillingType.SelectedValue = contract.BillingTypeID.ToString();

                ddlBillingCardType.SelectedValue = contract.Customer.CreditCardTypeID.ToString();
                ddlBillingBank.SelectedValue = contract.Customer.BankID.ToString();
                txtBillingCardNo.Text = contract.Customer.CardNo;
                txtBillingCardHolderName.Text = contract.Customer.CardHolderName;
                txtBillingCardHolderID.Text = contract.Customer.CardHolderID;

                if (contract.Customer.ExpiredDate.HasValue)
                {
                    ddlCardExpiredMonth.SelectedValue = contract.Customer.ExpiredDate.Value.Month.ToString();
                    ddlCardExpiredYear.SelectedValue = contract.Customer.ExpiredDate.Value.Year.ToString();
                }

                txtMailingAddress.Text = contract.Customer.MailingAddress;
                txtMailingZipCode.Text = contract.Customer.MailingZipCode;
                txtAddress.Text = contract.Customer.Address;
                txtZipCode.Text = contract.Customer.ZipCode;

                if (ddlArea.Items.FindByValue(contract.Customer.AreaID.ToString()) != null)
                    ddlArea.SelectedValue = contract.Customer.AreaID.ToString();
                else
                    ddlArea.SelectedIndex = 0;

                
                if(contract.Customer.SchoolID.HasValue)
                {
                    txtSchoolID.Text = contract.Customer.SchoolID.Value.ToString();
                    txtSchoolName.Text = contract.Customer.School.Name;
                }
                else
                {
                    txtSchoolID.Text = String.Empty;
                    txtSchoolName.Text = String.Empty;
                }                    

                lblStatus.Text = contractProvider.DecodeStatus(Convert.ToChar(contract.Status));
                txtNotes.Text = contract.Notes;

                lblActiveDate.Text = contract.ActiveDate.HasValue ? contract.ActiveDate.Value.ToString("dddd, dd MMMM yyyy") : "Not Active";

                btnVoid.Enabled = contract.Status == "A";

                if (employeeProvider.Get(User.Identity.Name).CanEditActiveContract)
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = contract.Status == "P";

                lblClosedDate.Text = contract.ClosedDate.HasValue ? contract.ClosedDate.Value.ToString("dddd, dd MMMM yyyy") : "This contract has not been closed";
                lblVoidDate.Text = contract.VoidDate.HasValue ? contract.VoidDate.Value.ToString("dddd, dd MMMM yyyy") : "This contract has not been void";
                calExpiredDate.Enabled = true;
                calExpiredDate.SelectedDate = contract.ExpiredDate;

                ddlMonthlyDuesItem.SelectedValue = Convert.ToString(contract.BillingItemID);
                calNextDuesDate.SelectedDate = contract.NextDuesDate.Value;
                txtDuesAmount.Text = contract.DuesAmount.ToString("###,##0.00");

                Person father = contract.Customer.Persons.SingleOrDefault(p => p.Connection == "F");
                chkFather.Checked = father != null;
                if (father != null)
                {
                    txtFatherName.Text = father.Name;
                    txtFatherPhone.Text = father.Phone1;
                    txtIDCardNoFather.Text = father.IDCardNo;
                    txtFatherEmail.Text = father.Email;
                    chkFatherBirthDateUnknown.Checked = !father.BirthDate.HasValue;
                    if (father.BirthDate.HasValue)
                        calBirthDateFather.SelectedValue = father.BirthDate.Value;
                }

                Person mother = contract.Customer.Persons.SingleOrDefault(p => p.Connection == "M");
                chkMother.Checked = mother != null;
                if (mother != null)
                {
                    txtMotherName.Text = mother.Name;
                    txtMotherPhone.Text = mother.Phone1;
                    txtIDCardNoMother.Text = mother.IDCardNo;
                    txtMotherEmail.Text = mother.Email;
                    chkMotherBirthDateUnknown.Checked = !mother.BirthDate.HasValue;
                    if (mother.BirthDate.HasValue)
                        calBirthDateMother.SelectedValue = mother.BirthDate.Value;
                }

                btnVoid.Enabled = !contract.VoidDate.HasValue;
                btnCloseContract.Enabled = !contract.VoidDate.HasValue;

                btnVoid.Enabled = !contract.ClosedDate.HasValue;
                btnCloseContract.Enabled = !contract.ClosedDate.HasValue;

                if (!String.IsNullOrEmpty(contract.ContractType))
                {
                    chkIsTransfer.Checked = contract.ContractType == "T";

                    if(contract.ContractType != "T")
                        ddlRenewalOrUpgrade.SelectedValue = contract.ContractType;
                }
                ddlRenewalOrUpgrade.Enabled = false;
                chkIsTransfer.Enabled = false;

                ClientScript.RegisterStartupScript(this.GetType(), "_cust", "$(document).ready(function() { $('#customer').show(); });", true);
                hypLookUpCustomer.Visible = false;
            }
        }
        catch (Exception ex)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        e.Command.Parameters["@ContractNo"].Value = txtFindContractNo.Text;
        e.Command.Parameters["@CustomerName"].Value = txtFindCustomerName.Text;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void cuvExistingCustomer_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (chkGenerateNewBarcodeCustomer.Checked)
            args.IsValid = true;
        else
            args.IsValid = txtCustomerBarcode.Text.Trim().Length > 0 && customerProvider.IsExist(txtCustomerBarcode.Text);
    }
    protected void cuvNewCustomer_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (chkGenerateNewBarcodeCustomer.Checked)
            args.IsValid = txtCustomerFirstName.Text.Trim().Length > 0 && txtCustomerLastName.Text.Trim().Length > 0;
        else
            args.IsValid = true;
    }
    protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
        var package = packageProvider.GetDetail(Convert.ToInt32(ddlPackage.SelectedValue));
        gvwPackage.DataSource = package;
        gvwPackage.DataBind();
    }

    protected void gvwPackage_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }

    protected void btnVoid_Click(object sender, EventArgs e)
    {
        try
        {
            var list = contractProvider.GetActiveInvoices(lblContractNo.Text).ToList();
            string invoices = String.Empty;
            list.ForEach(inv => invoices += @"<li>" + inv.InvoiceNo + @"</li");
            if (list.Any())
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblMessageAddEdit,
                    @"This contract has invoice already, please void invoice first: <ul>" + invoices + "</ul>",
                    LabelStyleNames.ErrorMessage);
            }
            else
            {
                contractProvider.VoidContract(lblContractNo.Text);
                mvwForm.SetActiveView(viwRead);
                WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    @"Contract <b> " + lblContractNo.Text + "</b> has been processed as VOID",
                    LabelStyleNames.AlternateMessage);
                btnVoid.Enabled = false;
                btnCloseContract.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }
    protected void calEffectiveDate_DateChanged(object sender, EventArgs e)
    {
        calNextDuesDate.SelectedDate = calEffectiveDate.SelectedDate.AddMonths(1);
    }
    protected void cuvCreditCardNo_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = args.Value.Trim().Length == 16 && ValidationHelper.IsValidCreditCardNumber(args.Value);        
    }
    protected void cuvDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !monthlyClosingProvider.IsClosed(
            Convert.ToInt32(ddlFindBranch.SelectedValue),
            calDate.SelectedDate.Month,
            calDate.SelectedDate.Year) && calDate.SelectedDate > DateTime.Today;
    }
    protected void btnCloseContract_Click(object sender, EventArgs e)
    {
        try
        {
            contractProvider.CloseContract(lblContractNo.Text);
            btnVoid.Enabled = false;
            btnCloseContract.Enabled = false;
            WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    @"Contract <b> " + lblContractNo.Text + "</b> has been processed as CLOSED",
                    LabelStyleNames.AlternateMessage);
            mvwForm.SetActiveView(viwRead);
        }
        catch (Exception ex)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }
}