using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.ViewModels;
using FitnessManagement.Data;
using Catalyst.Extension;

public partial class ExistingMember : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion


    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();
    PackageProvider packageProvider = UnityContainerHelper.Container.Resolve<PackageProvider>();
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();
    ItemProvider itemProvider = UnityContainerHelper.Container.Resolve<ItemProvider>();
    ItemTypeProvider itemTypeProvider = UnityContainerHelper.Container.Resolve<ItemTypeProvider>();
    PaymentTypeProvider paymentTypeProvider = UnityContainerHelper.Container.Resolve<PaymentTypeProvider>();
    InvoiceProvider invoiceProvider = UnityContainerHelper.Container.Resolve<InvoiceProvider>();
    MonthlyClosingProvider monthlyClosingProvider = UnityContainerHelper.Container.Resolve<MonthlyClosingProvider>();

    public List<InvoiceDetailViewModel> _InvoiceDetail
    {
        get
        {            
            return ViewState["InvoiceDetail"] as List<InvoiceDetailViewModel>;
        }
        set
        {
            if (ViewState["InvoiceDetail"] == null)
                ViewState["InvoiceDetail"] = new List<InvoiceDetailViewModel>();
            ViewState["InvoiceDetail"] = value;
        }
    }

    public bool ExcludePayment
    {
        get
        {
            try
            {
                return Convert.ToBoolean(ViewState["ExPayment"]);
            }
            catch { 
                ViewState["ExPayment"] = "0";  
                return false; 
            }
        }
        set
        {
            if (ViewState["ExPayment"] == null)
                ViewState["ExPayment"] = "0";
            ViewState["ExPayment"] = value;
        }
    }


    public List<PaymentDetailViewModel> _PaymentDetail
    {
        get
        {
            return ViewState["PaymentDetail"] as List<PaymentDetailViewModel>;
        }
        set
        {
            if (ViewState["PaymentDetail"] == null)
                ViewState["PaymentDetail"] = new List<PaymentDetailViewModel>();
            ViewState["PaymentDetail"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            FillDropDown();
            _InvoiceDetail = new List<InvoiceDetailViewModel>();
            _PaymentDetail = new List<PaymentDetailViewModel>();
            CalculateTotalInvoiceAndPayment();
            ExcludePayment = !String.IsNullOrEmpty(Request["ExPayment"]);
            pnlPayment.Visible = !ExcludePayment;
        }
    }

    private void CalculateTotalInvoiceAndPayment()
    {
        lblTotalInvoice.Text = (_InvoiceDetail.Any() ?
            _InvoiceDetail.Sum(row => ((row.UnitPrice * row.Quantity) - row.Discount / 100 * (row.UnitPrice * row.Quantity))) - 0 : 0).ToString("###,##0.00");

        lblTotalPayment.Text = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M).ToString("###,##0.00");
    }

    private void FillDropDown()
    {
        ddlBranch.DataSource = userProvider.GetCurrentActiveBranches(User.Identity.Name);
        ddlBranch.DataTextField = "Name";
        ddlBranch.DataValueField = "ID";
        ddlBranch.DataBind();
        ddlBranch.Enabled = ddlBranch.Items.Count > 0;


        ddlSales.DataSource = employeeProvider.GetSales();
        ddlSales.DataTextField = "FirstName";
        ddlSales.DataValueField = "ID";
        ddlSales.DataBind();
        ddlSales.Items.Insert(0, String.Empty);

        ddlItemType.DataSource = itemTypeProvider.GetAll();
        ddlItemType.DataTextField = "Description";
        ddlItemType.DataValueField = "ID";
        ddlItemType.DataBind();

        ddlPaymentType.DataSource = paymentTypeProvider.GetAll();
        ddlPaymentType.DataTextField = "Description";
        ddlPaymentType.DataValueField = "ID";
        ddlPaymentType.DataBind();
    }

    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(txtDiscount.Text))
                txtDiscount.Text = "0";

            int itemID = Convert.ToInt32(ddlItem.SelectedValue);
            Item item = itemProvider.Get(itemID);
            _InvoiceDetail.Add(
                new InvoiceDetailViewModel()
                {
                    ID = _InvoiceDetail.Any() ? _InvoiceDetail.Max(inv => inv.ID) + 1 : 1,
                    InvoiceID = RowID,
                    ItemID = item.ID,
                    ItemBarcode = item.Barcode,
                    ItemDescription = item.Description,
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    UnitPrice = Convert.ToDecimal(txtUnitPrice.Text),
                    Discount = Convert.ToDecimal(txtDiscount.Text),
                    IsTaxable = chkIsTaxable.Checked,
                    NetAmount = ((Convert.ToInt32(txtQuantity.Text) *
                                 Convert.ToDecimal(txtUnitPrice.Text) -
                                 (Convert.ToDecimal(txtDiscount.Text) / 100) * (Convert.ToInt32(txtQuantity.Text) *
                                 Convert.ToDecimal(txtUnitPrice.Text))) /
                                 (chkIsTaxable.Checked ? 1.1M : 1M)),
                    Total = (Convert.ToInt32(txtQuantity.Text) *
                                 Convert.ToDecimal(txtUnitPrice.Text) -
                                 (Convert.ToDecimal(txtDiscount.Text) / 100) * (Convert.ToInt32(txtQuantity.Text) *
                                 Convert.ToDecimal(txtUnitPrice.Text)))
                });
            WebFormHelper.ClearTextBox(txtQuantity, txtDiscount, txtUnitPrice);
            ddlItemType.SelectedIndex = 0;
            ddlItem.SelectedIndex = 0;
            gvwOtherPurchase.DataSource = _InvoiceDetail;
            gvwOtherPurchase.DataBind();
            tsmScriptManager.SetFocus(ddlItem);
            CalculateTotalInvoiceAndPayment();
        }
        catch
        {
        }
    }

    protected void gvwOtherPurchase_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    protected void gvwOtherPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteItem")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            _InvoiceDetail.RemoveAll(inv => inv.ID == id);
            gvwOtherPurchase.DataSource = _InvoiceDetail;
            gvwOtherPurchase.DataBind();
        }
        CalculateTotalInvoiceAndPayment();
    }

    protected void btnAddPayment_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4 &&
           Convert.ToInt32(ddlCreditCardType.SelectedValue) == 0)
        {
            return;
        }
        _PaymentDetail.Add(
            new PaymentDetailViewModel()
            {
                ID = _PaymentDetail.Any() ? _PaymentDetail.Max(pay => pay.ID) + 1 : 1,
                PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue),
                PaymentType = ddlPaymentType.SelectedItem.Text,
                CreditCardTypeID = ddlCreditCardType.SelectedValue.ToDefaultNumber<int>() == 0 ? (int?) null : ddlCreditCardType.SelectedValue.ToDefaultNumber<int>(),
                CreditCardType = ddlCreditCardType.SelectedItem.Text,
                ApprovalCode = txtApprovalCode.Text,
                Amount = Convert.ToDecimal(txtPaymentAmount.Text),
                Notes= txtPaymentNotes.Text
            });
        gvwPayment.DataSource = _PaymentDetail;
        gvwPayment.DataBind();
        WebFormHelper.ClearTextBox(txtPaymentAmount, txtApprovalCode);
        CalculateTotalInvoiceAndPayment();
    }

    protected void gvwPayment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }
    protected void gvwPayment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeletePayment")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            _PaymentDetail.RemoveAll(pay => pay.ID == id);
            gvwPayment.DataSource = _PaymentDetail;
            gvwPayment.DataBind();
        }
        CalculateTotalInvoiceAndPayment();
    }

    private void _CalculateTotalInvoiceAndPayment(out decimal totalInvoice, out decimal totalPayment)
    {
        totalInvoice = (_InvoiceDetail.Any() ?
            _InvoiceDetail.Sum(row => ((row.UnitPrice * row.Quantity) - row.Discount / 100 * (row.UnitPrice * row.Quantity)))  : 0);

        totalPayment = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.Validate("ExistingMember");
        if (this.IsValid)
        {
            decimal totalInvoice = 0, totalPayment = 0;
            if (!ExcludePayment)
            {
                _CalculateTotalInvoiceAndPayment(out totalInvoice, out totalPayment);
                if (totalInvoice != totalPayment || (totalInvoice == 0 && totalPayment == 0))
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblStatus,
                        "Total Invoice is not equal Total Payment. Please verify this transaction again",
                        LabelStyleNames.ErrorMessage);
                    return;
                }
            }

            string invoiceNo = invoiceProvider.CreateExistingMemberInvoice(
                Convert.ToInt32(ddlBranch.SelectedValue),
                calDate.SelectedDate,
                txtCustomerCode.Text,
                Convert.ToInt32(ddlSales.SelectedValue),
                txtNotes.Text,
                0,
                _InvoiceDetail,
                _PaymentDetail);

            Response.Redirect(String.Format("ExistingMemberCompleted.aspx?InvoiceNo={0}", invoiceNo));
        }
    }
    protected void txtDiscountValue_TextChanged(object sender, EventArgs e)
    {
        CalculateTotalInvoiceAndPayment();
    }

    protected void cuvDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !monthlyClosingProvider.IsClosed(
            Convert.ToInt32(ddlBranch.SelectedValue),
            calDate.SelectedDate.Month,
            calDate.SelectedDate.Year);
    }
}