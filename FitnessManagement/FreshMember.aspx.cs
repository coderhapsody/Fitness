using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;
using FitnessManagement.ViewModels;
using FitnessManagement.Data;
using eWorld.UI;
using Catalyst.Extension;
using System.Globalization;

public partial class FreshMember : System.Web.UI.Page
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
    BillingTypeProvider billingTypeProvider = UnityContainerHelper.Container.Resolve<BillingTypeProvider>();
    InvoiceProvider invoiceProvider = UnityContainerHelper.Container.Resolve<InvoiceProvider>();
    ContractProvider contractProvider = UnityContainerHelper.Container.Resolve<ContractProvider>();
    MonthlyClosingProvider monthlyClosingProvider = UnityContainerHelper.Container.Resolve<MonthlyClosingProvider>();

    public static FreshMember page;

    public List<PackageDetailViewModel> _PackageDetail
    {
        get
        {
            foreach (var item in ViewState["PackageDetail"] as List<PackageDetailViewModel>)
                item.NetAmount = (item.Quantity * item.UnitPrice - (item.Discount / 100 * item.Quantity * item.UnitPrice)) / (item.IsTaxed ? 1.1M : 1M);

            return ViewState["PackageDetail"] as List<PackageDetailViewModel>;
        }
        set
        {
            if (ViewState["PackageDetail"] == null)
                ViewState["PackageDetail"] = new List<PackageDetailViewModel>();

            foreach (var item in value)
                item.NetAmount = (item.Quantity * item.UnitPrice - (item.Discount / 100 * item.Quantity * item.UnitPrice)) / (item.IsTaxed ? 1.1M : 1M);


            ViewState["PackageDetail"] = value;            
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
            _PaymentDetail = new List<PaymentDetailViewModel>();
            _PackageDetail = new List<PackageDetailViewModel>();
            CalculateTotalInvoiceAndPayment();            
            page = this;
        }
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

        ddlPaymentType.DataSource = paymentTypeProvider.GetAll();
        ddlPaymentType.DataTextField = "Description";
        ddlPaymentType.DataValueField = "ID";
        ddlPaymentType.DataBind();

    }

    //[System.Web.Services.WebMethod]
    //public static void PM_LoadPackageDetail(string contractNo)
    //{
    //    using (FitnessDataContext ctx = new FitnessDataContext())
    //    {
    //        ContractProvider contractProvider = new ContractProvider(ctx);
    //        PackageProvider packageProvider = new PackageProvider(ctx);
    //        PackageHeader pkg = contractProvider.GetPackageInContract(contractNo);
    //        IEnumerable<PackageDetailViewModel> package = packageProvider.GetDetail(pkg.ID);
    //        page._PackageDetail = package.ToList();
    //        page.gvwPackage.DataSource = package;
    //        page.gvwPackage.DataBind();
    //        page.CalculateTotalInvoiceAndPayment();
    //        page.txtNotes.Text = contractNo;
    //    }
    //}

    private void LoadPackageDetail()
    {
        PackageHeader pkg = contractProvider.GetPackageInContract(txtContractNo.Text);
        IEnumerable<PackageDetailViewModel> package = packageProvider.GetDetail(pkg.ID);
        _PackageDetail = package.ToList();
        gvwPackage.DataSource = package;
        gvwPackage.DataBind();
        CalculateTotalInvoiceAndPayment();

        lblPackage.Text = pkg.Name;
    }

    protected void gvwPackage_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    private void _CalculateTotalInvoiceAndPayment(decimal discountValue, out decimal totalInvoice, out decimal totalPayment)
    {
        totalInvoice = (_PackageDetail.Any() ?
            _PackageDetail.Sum(row => ((row.UnitPrice * row.Quantity) - row.Discount / 100 * (row.UnitPrice * row.Quantity)) * 1 /*(row.IsTaxed ? 1.1M : 1M)*/) : 0);
        totalInvoice -= discountValue;
        totalPayment = (_PaymentDetail.Any() ? _PaymentDetail.Sum(row => row.Amount) : 0M);        
    }

    private void CalculateTotalInvoiceAndPayment()
    {
        decimal totalInvoice = 0, totalPayment = 0;
        _CalculateTotalInvoiceAndPayment(
            0, 
            out totalInvoice, 
            out totalPayment);
        lblTotalInvoice.Text = totalInvoice.ToString("###,##0.00");
        lblTotalPayment.Text = totalPayment.ToString("###,##0.00");
    }


    protected void gvwPackage_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int packageID = Convert.ToInt32(e.CommandArgument);
        int rowIndex = ((e.CommandSource as LinkButton).NamingContainer as GridViewRow).RowIndex;
        if (e.CommandName == "EditPackage")
        {
            gvwPackage.EditIndex = rowIndex;
            gvwPackage.DataSource = _PackageDetail;
            gvwPackage.DataBind();
        }
        else if (e.CommandName == "DeletePackage")
        {
            _PackageDetail.RemoveAll(package => package.ID == packageID);
            gvwPackage.DataSource = _PackageDetail;
            gvwPackage.DataBind();
        }
        else if (e.CommandName == "CancelPackage")
        {
            gvwPackage.EditIndex = -1;
            gvwPackage.DataSource = _PackageDetail;
            gvwPackage.DataBind();
        }
        else if (e.CommandName == "SavePackage")
        {
            int quantity = Convert.ToInt32((gvwPackage.Rows[rowIndex].Cells[3].Controls[1] as TextBox).Text);
            int unitPrice = Convert.ToInt32(Convert.ToDecimal((gvwPackage.Rows[rowIndex].Cells[5].Controls[1] as TextBox).Text));
            decimal discount = Convert.ToDecimal((gvwPackage.Rows[rowIndex].Cells[6].Controls[1] as TextBox).Text);
            bool isTaxed = (gvwPackage.Rows[rowIndex].Cells[7].Controls[1] as CheckBox).Checked;

            var package = _PackageDetail.SingleOrDefault(row => row.ID == packageID);
            if (package != null)
            {
                int index = _PackageDetail.FindIndex(row => row.ID == packageID);
                package.Quantity = quantity;
                package.UnitPrice = unitPrice;
                package.Discount = discount;
                package.IsTaxed = isTaxed;
                package.Total = quantity * unitPrice - (quantity * unitPrice * discount / 100);
                _PackageDetail[index] = package;                        
                gvwPackage.EditIndex = -1;                
                gvwPackage.DataSource = _PackageDetail;
                gvwPackage.DataBind();                
            }
        }
        CalculateTotalInvoiceAndPayment();
    }
    protected void gvwPackage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        PackageDetailViewModel row = e.Row.DataItem as PackageDetailViewModel;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.FindControl("chkIsTaxable") as CheckBox).Checked = row.IsTaxed;
        }
    }

    protected void btnAddPayment_Click(object sender, EventArgs e)
    {
        _PaymentDetail.Add(
            new PaymentDetailViewModel()
            {
                ID = _PaymentDetail.Any() ? _PaymentDetail.Max(pay => pay.ID) + 1 : 1,
                PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue),                
                PaymentType = ddlPaymentType.SelectedItem.Text,
                CreditCardTypeID = String.IsNullOrEmpty(ddlCreditCardType.SelectedItem.Value) ? (int?) null : Convert.ToInt32(ddlCreditCardType.SelectedValue),
                CreditCardType = ddlCreditCardType.SelectedItem.Text,
                ApprovalCode = txtApprovalCode.Text,
                Notes = txtPaymentNotes.Text,
                Amount = Convert.ToDecimal(txtPaymentAmount.Text)
            });
        txtPaymentNotes.Text = String.Empty;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.Validate("FreshMember");
        if (this.IsValid)
        {
            decimal totalInvoice = 0, totalPayment = 0;
            _CalculateTotalInvoiceAndPayment(
                0, 
                out totalInvoice, 
                out totalPayment);
            //if (totalInvoice != totalPayment || (totalInvoice == 0 && totalPayment == 0))
            if (totalInvoice != totalPayment)
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblStatus,
                    "Total Invoice is not equal Total Payment. Please verify this transaction again",
                    LabelStyleNames.ErrorMessage);
                return;
            }

            string invoiceNo = invoiceProvider.CreateFreshMemberInvoice(
                Convert.ToInt32(ddlBranch.SelectedValue),
                txtContractNo.Text,
                calDate.SelectedDate,
                Convert.ToInt32(ddlSales.SelectedValue),                
                txtNotes.Text,
                0,
                _PackageDetail,
                _PaymentDetail);

            Response.Redirect(String.Format("FreshMemberCompleted.aspx?InvoiceNo={0}", invoiceNo));
        }
    }
    protected void btnDummy_Click(object sender, EventArgs e)
    {
        LoadPackageDetail();
        CalculateTotalInvoiceAndPayment();
    }

    protected void cuvContractNo_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = txtContractNo.Text.Trim().Length > 0 && 
                       contractProvider.IsValidContract(args.Value);
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