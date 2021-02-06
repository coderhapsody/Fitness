using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using FitnessManagement.Providers;

public class InvoiceProvider
{
    public static readonly string FRESH_MEMBER_INVOICE = "F";
    public static readonly string EXISTING_MEMBER_INVOICE = "X";
    public static readonly int ITEM_ACCOUNT_ADMINISTRATION_FEE = 18;

    private FitnessDataContext ctx;
    private AutoNumberProvider autoNumberProvider;

    public InvoiceProvider(FitnessDataContext context)
	{
        this.ctx = context;
        this.autoNumberProvider = new AutoNumberProvider(context);        
	}
    

    public string CreateFreshMemberInvoice(
        int branchID,
        string contractNo,        
        DateTime purchasedate,
        int employeeID,        
        string notes,
        decimal discountValue,
        IEnumerable<PackageDetailViewModel> detail,
        IEnumerable<PaymentDetailViewModel> paymentDetail)
    {
        Contract contract = ctx.Contracts.SingleOrDefault(con => con.ContractNo == contractNo);
        if (contract != null)
        {
            InvoiceHeader header = new InvoiceHeader();
            header.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", purchasedate.Month, purchasedate.Year);
            header.BranchID = branchID;
            header.Contract = contract;
            header.Date = purchasedate;            
            header.EmployeeID = employeeID;
            header.InvoiceType = FRESH_MEMBER_INVOICE;
            header.Notes = notes;
            header.DiscountValue = discountValue;
            header.CustomerID = contract.CustomerID;
            header.VoidDate = (DateTime?)null;
            EntityHelper.SetAuditFieldForInsert(header, HttpContext.Current.User.Identity.Name);
            foreach (var model in detail)
            {
                InvoiceDetail d = new InvoiceDetail();
                d.ItemID = model.ItemID;
                d.Quantity = model.Quantity;
                d.UnitPrice = model.UnitPrice;
                d.Discount = model.Discount;
                d.IsTaxable = model.IsTaxed;
                header.InvoiceDetails.Add(d);
                ctx.InvoiceDetails.InsertOnSubmit(d);
            }

            PaymentHeader pay = new PaymentHeader();
            pay.Date = purchasedate;
            pay.InvoiceHeader = header;
            pay.PaymentNo = autoNumberProvider.Generate(branchID, "PM", purchasedate.Month, purchasedate.Year);
            pay.VoidDate = (DateTime?)null;
            EntityHelper.SetAuditFieldForInsert(pay, HttpContext.Current.User.Identity.Name);            

            foreach (var payDetail in paymentDetail)
            {
                PaymentDetail payd = new PaymentDetail();
                payd.Amount = payDetail.Amount;
                payd.CreditCardTypeID = payDetail.CreditCardTypeID;
                payd.PaymentTypeID = payDetail.PaymentTypeID;
                payd.ApprovalCode = payDetail.ApprovalCode;
                payd.Notes = payDetail.Notes;
                pay.PaymentDetails.Add(payd);
                ctx.PaymentDetails.InsertOnSubmit(payd);
            }



            //CustomerStatusHistory custStatusHist = new CustomerStatusHistory();
            //custStatusHist.Customer = cust;
            //custStatusHist.CustomerStatusID =

            contract.PurchaseDate = purchasedate;
            //contract.ActiveDate = DateTime.Now;
            contract.Status = FitnessManagement.Providers.ContractStatus.PAID;

            autoNumberProvider.Increment("OR", header.BranchID, purchasedate.Year);
            autoNumberProvider.Increment("PM", header.BranchID, purchasedate.Year);
            
            

            ctx.SubmitChanges();

            return header.InvoiceNo;
        }

        return null;
    }

    public string CreateExistingMemberInvoice(
        int branchID,
        DateTime date,        
        string customerCode,
        int employeeID,
        string notes,
        decimal discountValue,
        IEnumerable<InvoiceDetailViewModel> detail,
        IEnumerable<PaymentDetailViewModel> paymentDetail)
    {
        Customer cust = ctx.Customers.SingleOrDefault(c => c.Barcode == customerCode);


        InvoiceHeader header = new InvoiceHeader();
        header.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", date.Month, date.Year);
        header.BranchID = branchID;                        
        header.Date = date;
        if (cust == null)
        {
            header.Customer = null;
            header.CustomerName = customerCode;
        }
        else
        {
            header.Customer = cust;
            header.CustomerName = null;
        }

        header.EmployeeID = employeeID;
        header.InvoiceType = EXISTING_MEMBER_INVOICE;
        header.Notes = notes;
        header.DiscountValue = discountValue;
        EntityHelper.SetAuditFieldForInsert(header, HttpContext.Current.User.Identity.Name);
        foreach (var model in detail)
        {
            InvoiceDetail d = new InvoiceDetail();
            d.InvoiceID = model.InvoiceID;
            d.ItemID = model.ItemID;
            d.Quantity = model.Quantity;
            d.UnitPrice = model.UnitPrice;
            d.Discount = model.Discount;
            d.IsTaxable = model.IsTaxable;
            header.InvoiceDetails.Add(d);
            ctx.InvoiceDetails.InsertOnSubmit(d);
        }

        var payments = paymentDetail.ToList();
        if (payments.Count > 0)
        {
            PaymentHeader pay = new PaymentHeader();
            pay.Date = date;
            pay.InvoiceHeader = header;
            pay.PaymentNo = autoNumberProvider.Generate(branchID, "PM", date.Month, date.Year);
            pay.VoidDate = null;
            EntityHelper.SetAuditFieldForInsert(pay, HttpContext.Current.User.Identity.Name);

            foreach (var payDetail in payments)
            {
                PaymentDetail payd = new PaymentDetail();
                payd.Amount = payDetail.Amount;
                payd.CreditCardTypeID = payDetail.CreditCardTypeID;
                payd.PaymentTypeID = payDetail.PaymentTypeID;
                payd.ApprovalCode = payDetail.ApprovalCode;
                payd.Notes = payDetail.Notes;
                pay.PaymentDetails.Add(payd);
                ctx.PaymentDetails.InsertOnSubmit(payd);
            }
            autoNumberProvider.Increment("PM", header.BranchID, date.Year);
        }

        autoNumberProvider.Increment("OR", header.BranchID, date.Year);
        
        ctx.SubmitChanges();

        return header.InvoiceNo;                
    }

    public InvoiceHeader CreateExistingMemberInvoiceForBilling(
        int branchID,
        DateTime date,
        string customerCode,
        int employeeID,
        string notes,
        decimal discountValue,
        IEnumerable<InvoiceDetailViewModel> detail,
        IEnumerable<PaymentDetailViewModel> paymentDetail)        
    {
        Customer cust = ctx.Customers.SingleOrDefault(c => c.Barcode == customerCode);
        if (cust != null)
        {
            InvoiceHeader header = new InvoiceHeader();
            header.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", date.Month, date.Year);
            header.BranchID = branchID;
            header.Date = date;
            header.Customer = cust;
            header.EmployeeID = employeeID;
            header.InvoiceType = EXISTING_MEMBER_INVOICE;
            header.Notes = notes;
            header.DiscountValue = discountValue;
            EntityHelper.SetAuditFieldForInsert(header, HttpContext.Current.User.Identity.Name);
            foreach (var model in detail)
            {
                InvoiceDetail d = new InvoiceDetail();
                d.InvoiceID = model.InvoiceID;
                d.ItemID = model.ItemID;
                d.Quantity = model.Quantity;
                d.UnitPrice = model.UnitPrice;
                d.Discount = model.Discount;
                d.IsTaxable = model.IsTaxable;
                header.InvoiceDetails.Add(d);
                ctx.InvoiceDetails.InsertOnSubmit(d);
            }

            PaymentHeader pay = new PaymentHeader();
            pay.Date = date;
            pay.InvoiceHeader = header;
            pay.PaymentNo = autoNumberProvider.Generate(branchID, "PM", date.Month, date.Year);
            pay.VoidDate = null;
            EntityHelper.SetAuditFieldForInsert(pay, HttpContext.Current.User.Identity.Name);

            foreach (var payDetail in paymentDetail)
            {
                PaymentDetail payd = new PaymentDetail();
                payd.Amount = payDetail.Amount;
                payd.CreditCardTypeID = payDetail.CreditCardTypeID;
                payd.PaymentTypeID = payDetail.PaymentTypeID;
                payd.ApprovalCode = payDetail.ApprovalCode;
                payd.Notes = payDetail.Notes;
                pay.PaymentDetails.Add(payd);
                ctx.PaymentDetails.InsertOnSubmit(payd);
            }

            autoNumberProvider.Increment("OR", header.BranchID, date.Year);
            autoNumberProvider.Increment("PM", header.BranchID, date.Year);

            return header;
        }

        return null;
    }

    public void ProcessVoid(string invoiceNo, string reason, bool voidPaymentOnly)
    {
        var invoice = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
        if (invoice != null)
        {
            if (!voidPaymentOnly)
            {
                invoice.VoidDate = DateTime.Now;
                invoice.VoidReason = reason;

                if (invoice.InvoiceType == FRESH_MEMBER_INVOICE)
                {
                    invoice.Contract.Status = FitnessManagement.Providers.ContractStatus.UNPAID;
                    invoice.Contract.ActiveDate = null;
                }

                EntityHelper.SetAuditFieldForUpdate(invoice, HttpContext.Current.User.Identity.Name);
            }
            var payment = invoice.PaymentHeaders.Where(p => p.InvoiceID == invoice.ID && !p.VoidDate.HasValue).FirstOrDefault();
            if (payment != null)
            {
                payment.VoidDate = invoice.VoidDate;
                payment.VoidReason = reason;
                EntityHelper.SetAuditFieldForUpdate(payment, HttpContext.Current.User.Identity.Name);                       
            }
            

            ctx.SubmitChanges();
        }        
    }

    public InvoiceHeader GetInvoice(string invoiceNo)
    {
        return ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
    }

    public IEnumerable<InvoiceDetailViewModel> GetDetail(string invoiceNo)
    {
        var invoice = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
        if (invoice != null)
        {
            return from d in ctx.InvoiceDetails
                    join i in ctx.Items on d.ItemID equals i.ID
                   where d.InvoiceID == invoice.ID
                   select new InvoiceDetailViewModel
                   {
                       ID = d.ID,
                       InvoiceID = d.InvoiceID,
                       ItemID = d.ItemID,
                       ItemBarcode = i.Barcode,
                       ItemDescription = i.Description,
                       Quantity = d.Quantity,
                       UnitPrice = d.UnitPrice,
                       IsTaxable = d.IsTaxable,
                       Discount = d.Discount,
                       NetAmount = ((d.Quantity * d.UnitPrice) - ((d.Quantity * d.UnitPrice) * d.Discount/100)) / (d.IsTaxable ? 1.1M : 1M),
                       Total = d.Quantity * d.UnitPrice - (d.Quantity * d.UnitPrice) * d.Discount / 100
                   };
        }

        return null;
    }

    public string UpdateInvoiceAndPayment(string invoiceNo, DateTime invoiceDate, DateTime date, List<InvoiceDetailViewModel> invoiceDetail, List<PaymentDetailViewModel> paymentDetail)
    {
        PaymentProvider payment = new PaymentProvider(ctx);
        InvoiceHeader invoice = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
        invoice.Date = invoiceDate;
        string paymentNo = String.Empty;
        if(invoice != null)
        {
            ctx.InvoiceDetails.DeleteAllOnSubmit(invoice.InvoiceDetails.AsEnumerable());            
            foreach (var model in invoiceDetail)
            {
                InvoiceDetail d = new InvoiceDetail();
                d.InvoiceID = invoice.ID;
                d.ItemID = model.ItemID;
                d.Quantity = model.Quantity;
                d.UnitPrice = model.UnitPrice;
                d.Discount = model.Discount;
                d.IsTaxable = model.IsTaxable;
                invoice.InvoiceDetails.Add(d);
                ctx.InvoiceDetails.InsertOnSubmit(d);
            }
            paymentNo = payment.Create(date, invoiceNo, paymentDetail);
            ctx.SubmitChanges();
        }

        return paymentNo;
    }

    public int ProcessFirstAccrualInvoices(int branchID, int[] invoiceIDs, DateTime processDate)
    {        
        int processedInvoices = 0;
        foreach (var invoiceID in invoiceIDs)
        {
            var invoiceHeader = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.ID == invoiceID);
            decimal adminFee = 0;
            if (invoiceHeader != null)
            {
                // invoice
                int duesInMonth = invoiceHeader.Contract.DuesInMonth;
                var iah = new InvoiceAccrualHeader();
                iah.InvoiceID = invoiceHeader.ID;
                iah.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", processDate.Month, processDate.Year);
                autoNumberProvider.Increment("OR", branchID, processDate.Year);
                iah.TotalAmount = ctx.func_GetTotalInvoice(invoiceHeader.InvoiceNo).GetValueOrDefault();
                iah.AccrualDate = processDate;
                //iah.AccrualAmount = iah.TotalAmount / duesInMonth;                
                iah.CreatedWhen = DateTime.Now;
                iah.CreatedWho = HttpContext.Current.User.Identity.Name;                
                iah.SumAccrualPeriod = 1;                
                iah.TotalAccrualPeriod = duesInMonth;

                var details = invoiceHeader.InvoiceDetails.Where(invd => !invd.Item.Description.Contains("Admin Fee"));
                var total = 0M;
                foreach (var detail in details)
                {
                    var iad = new InvoiceAccrualDetail
                              {
                                  Discount =
                                      detail.Discount / (detail.Item.ItemAccountID == ITEM_ACCOUNT_ADMINISTRATION_FEE
                                          ? 1
                                          : duesInMonth),
                                  IsTaxable = detail.IsTaxable,
                                  ItemID = detail.ItemID,
                                  Quantity = detail.Quantity,
                                  UnitPrice =
                                      detail.UnitPrice / (detail.Item.ItemAccountID == ITEM_ACCOUNT_ADMINISTRATION_FEE
                                          ? 1
                                          : duesInMonth)
                              };
                    total += iad.UnitPrice * iad.Quantity - (iad.Discount / 100 * iad.UnitPrice * iad.Quantity) -
                             (invoiceHeader.DiscountValue / duesInMonth);
                    iah.InvoiceAccrualDetails.Add(iad);
                }
                iah.AccrualAmount = total;
                iah.SumAccrualAmount = iah.AccrualAmount;
                ctx.InvoiceAccrualHeaders.InsertOnSubmit(iah);

                // payment
                var paymentHeader = invoiceHeader.PaymentHeaders.FirstOrDefault(pay => !pay.VoidDate.HasValue);
                string paymentNo = String.Empty;
                if (paymentHeader != null)
                {
                    var payAccHeader = new PaymentAccrualHeader();
                    payAccHeader.InvoiceAccrualHeader = iah;
                    paymentNo = autoNumberProvider.Generate(branchID,
                        "PM",
                        processDate.Month,
                        processDate.Year);
                    payAccHeader.PaymentNo = paymentNo; 
                    autoNumberProvider.Increment("PM", branchID, processDate.Year);
                    payAccHeader.Date = processDate;                        
                    payAccHeader.CreatedWhen = DateTime.Now;
                    payAccHeader.CreatedWho = HttpContext.Current.User.Identity.Name;

                    var paymentDetails = paymentHeader.PaymentDetails.ToList();
                    foreach (var paymentDetail in paymentDetails)
                    {
                        var payAccDetail = new PaymentAccrualDetail();
                        payAccDetail.Amount = total / paymentDetails.Count;
                        payAccDetail.ApprovalCode = paymentDetail.ApprovalCode;
                        payAccDetail.CreditCardTypeID = paymentDetail.CreditCardTypeID;
                        payAccDetail.Notes = paymentDetail.Notes;
                        payAccDetail.PaymentTypeID = paymentDetail.PaymentTypeID;
                        
                        payAccHeader.PaymentAccrualDetails.Add(payAccDetail);
                    }
                    ctx.PaymentAccrualHeaders.InsertOnSubmit(payAccHeader);
                }


                // invoice                
                var iahAdminFee = new InvoiceAccrualHeader();
                iahAdminFee.InvoiceID = invoiceHeader.ID;
                iahAdminFee.InvoiceNo = iah.InvoiceNo + "AF";
                //iahAdminFee.TotalAmount = ctx.func_GetTotalInvoice(invoiceHeader.InvoiceNo).GetValueOrDefault();
                iahAdminFee.AccrualDate = processDate;
                //iah.AccrualAmount = iah.TotalAmount / duesInMonth;                
                iahAdminFee.CreatedWhen = DateTime.Now;
                iahAdminFee.CreatedWho = HttpContext.Current.User.Identity.Name;
                iahAdminFee.SumAccrualPeriod = 0;
                iahAdminFee.TotalAccrualPeriod = 0;

                var detailsAdminFee = invoiceHeader.InvoiceDetails.Where(invd => invd.Item.Description.Contains("Admin Fee"));
                var totalAdminFee = 0M;
                foreach (var detail in detailsAdminFee)
                {
                    var iadAdminFee = new InvoiceAccrualDetail
                    {
                        Discount =
                            detail.Discount / (detail.Item.ItemAccountID == ITEM_ACCOUNT_ADMINISTRATION_FEE
                                ? 1
                                : duesInMonth),
                        IsTaxable = false,
                        ItemID = detail.ItemID,
                        Quantity = detail.Quantity,
                        UnitPrice =
                            detail.UnitPrice / (detail.Item.ItemAccountID == ITEM_ACCOUNT_ADMINISTRATION_FEE
                                ? 1
                                : duesInMonth)
                    };
                    totalAdminFee += iadAdminFee.UnitPrice * iadAdminFee.Quantity - (iadAdminFee.Discount / 100 * iadAdminFee.UnitPrice * iadAdminFee.Quantity) -
                             (invoiceHeader.DiscountValue / duesInMonth);
                    iahAdminFee.InvoiceAccrualDetails.Add(iadAdminFee);
                }
                iahAdminFee.AccrualAmount = totalAdminFee;
                iahAdminFee.TotalAmount = totalAdminFee;
                iahAdminFee.SumAccrualAmount = iahAdminFee.AccrualAmount;
                ctx.InvoiceAccrualHeaders.InsertOnSubmit(iahAdminFee);
                iah.TotalAmount -= iahAdminFee.TotalAmount;
                // payment
                var paymentHeaderAdminFee = invoiceHeader.PaymentHeaders.FirstOrDefault(pay => !pay.VoidDate.HasValue);
                if (paymentHeaderAdminFee != null)
                {
                    var payAccHeader = new PaymentAccrualHeader();
                    payAccHeader.InvoiceAccrualHeader = iahAdminFee;
                    payAccHeader.PaymentNo = paymentNo + "AF";                    
                    payAccHeader.Date = processDate;
                    payAccHeader.CreatedWhen = DateTime.Now;
                    payAccHeader.CreatedWho = HttpContext.Current.User.Identity.Name;

                    var paymentDetails = paymentHeaderAdminFee.PaymentDetails.ToList();
                    foreach (var paymentDetail in paymentDetails)
                    {
                        var payAccDetail = new PaymentAccrualDetail();
                        payAccDetail.Amount = totalAdminFee / paymentDetails.Count;
                        payAccDetail.ApprovalCode = paymentDetail.ApprovalCode;
                        payAccDetail.CreditCardTypeID = paymentDetail.CreditCardTypeID;
                        payAccDetail.Notes = paymentDetail.Notes;
                        payAccDetail.PaymentTypeID = paymentDetail.PaymentTypeID;

                        payAccHeader.PaymentAccrualDetails.Add(payAccDetail);
                    }
                    ctx.PaymentAccrualHeaders.InsertOnSubmit(payAccHeader);
                }                
                
                processedInvoices++;
            }            
        }
        ctx.SubmitChanges();
        return processedInvoices;

    }

    public int ProcessAccrualInvoices(int branchID, int[] accrualInvoiceIDs, DateTime processDate)
    {
        int processedInvoices = 0;

        foreach (var accrualInvoiceID in accrualInvoiceIDs)
        {
            var iah = ctx.InvoiceAccrualHeaders.SingleOrDefault(inv => inv.ID == accrualInvoiceID);
            var invoiceHeader = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.ID == iah.InvoiceID);

            if (invoiceHeader != null && iah != null && iah.TotalAccrualPeriod > iah.SumAccrualPeriod)
            {
                // invoice
                int duesInMonth = iah.TotalAccrualPeriod;
                var invoiceDetails = invoiceHeader.InvoiceDetails;
                var nextInvoiceAccrual = new InvoiceAccrualHeader();
                nextInvoiceAccrual.InvoiceID = iah.InvoiceID;
                nextInvoiceAccrual.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", processDate.Month, processDate.Year);
                autoNumberProvider.Increment("OR", branchID, processDate.Year);

                nextInvoiceAccrual.AccrualDate = processDate;
                nextInvoiceAccrual.TotalAmount = iah.TotalAmount;
                nextInvoiceAccrual.TotalAccrualPeriod = iah.TotalAccrualPeriod;
                //nextInvoiceAccrual.AccrualAmount = iah.AccrualAmount;
                nextInvoiceAccrual.SumAccrualPeriod = iah.SumAccrualPeriod + 1;
                //nextInvoiceAccrual.SumAccrualAmount = iah.SumAccrualAmount + iah.AccrualAmount;
                nextInvoiceAccrual.CreatedWhen = DateTime.Now;
                nextInvoiceAccrual.CreatedWho = HttpContext.Current.User.Identity.Name;


                var total = 0M;
                foreach (var detail in invoiceDetails)
                {
                    if (detail.Item.ItemAccountID != ITEM_ACCOUNT_ADMINISTRATION_FEE)
                    {
                        var iad = new InvoiceAccrualDetail
                                  {
                                      Discount = detail.Discount / duesInMonth,
                                      IsTaxable = detail.IsTaxable,
                                      ItemID = detail.ItemID,
                                      Quantity = detail.Quantity,
                                      UnitPrice = detail.UnitPrice / duesInMonth
                                  };
                        total += iad.UnitPrice * iad.Quantity - (iad.Discount / 100 * iad.UnitPrice * iad.Quantity) -
                                 (invoiceHeader.DiscountValue / duesInMonth);
                        nextInvoiceAccrual.InvoiceAccrualDetails.Add(iad);
                    }
                }
                nextInvoiceAccrual.AccrualAmount = total;
                nextInvoiceAccrual.SumAccrualAmount = iah.SumAccrualAmount + total;
                
                ctx.InvoiceAccrualHeaders.InsertOnSubmit(nextInvoiceAccrual);


                // payment
                var paymentHeader = invoiceHeader.PaymentHeaders.FirstOrDefault(pay => !pay.VoidDate.HasValue);
                if (paymentHeader != null)
                {
                    var payAccHeader = new PaymentAccrualHeader();
                    payAccHeader.InvoiceAccrualHeader = nextInvoiceAccrual;
                    payAccHeader.PaymentNo = autoNumberProvider.Generate(branchID,
                        "PM",
                        processDate.Month,
                        processDate.Year);
                    autoNumberProvider.Increment("PM", branchID, processDate.Year);
                    payAccHeader.Date = processDate;    
                    payAccHeader.CreatedWhen = DateTime.Now;
                    payAccHeader.CreatedWho = HttpContext.Current.User.Identity.Name;

                    var paymentDetails = paymentHeader.PaymentDetails.ToList();
                    foreach (var paymentDetail in paymentDetails)
                    {
                        var payAccDetail = new PaymentAccrualDetail();
                        payAccDetail.Amount = total / paymentDetails.Count;
                        payAccDetail.ApprovalCode = paymentDetail.ApprovalCode;
                        payAccDetail.CreditCardTypeID = paymentDetail.CreditCardTypeID;
                        payAccDetail.Notes = paymentDetail.Notes;
                        payAccDetail.PaymentTypeID = paymentDetail.PaymentTypeID;

                        payAccHeader.PaymentAccrualDetails.Add(payAccDetail);
                    }
                    ctx.PaymentAccrualHeaders.InsertOnSubmit(payAccHeader);
                }

                processedInvoices++;
            }
        }
        ctx.SubmitChanges();

        return processedInvoices;
    }
}