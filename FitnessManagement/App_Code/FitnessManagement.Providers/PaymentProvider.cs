using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

/// <summary>
/// Summary description for PaymentProvider
/// </summary>
public class PaymentProvider
{
    private FitnessDataContext ctx;
    private AutoNumberProvider autoNumberProvider = UnityContainerHelper.Container.Resolve<AutoNumberProvider>();

	public PaymentProvider(FitnessDataContext context)
	{
        this.ctx = context;
	}

    public PaymentHeader GetPayment(string paymentNo)
    {
        return ctx.PaymentHeaders.SingleOrDefault(row => row.PaymentNo == paymentNo && !row.VoidDate.HasValue);
    }

    public PaymentHeader GetPaymentOfInvoice(string invoiceNo)
    {
        return (from pay in ctx.PaymentHeaders
                join inv in ctx.InvoiceHeaders on pay.InvoiceID equals inv.ID
                where inv.InvoiceNo == invoiceNo && !pay.VoidDate.HasValue
                select pay).SingleOrDefault();
    }

    public IEnumerable<PaymentDetailViewModel> GetDetail(string invoiceNo)
    {
        var invoice = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
        if (invoice != null)
        {
            return from d in ctx.PaymentDetails
                    join h in ctx.PaymentHeaders on d.PaymentID equals h.ID
                    join cct in ctx.CreditCardTypes on d.CreditCardTypeID equals cct.ID into temp
                    from t in temp.DefaultIfEmpty()
                    join pt in ctx.PaymentTypes on d.PaymentTypeID equals pt.ID
                   where h.InvoiceID == invoice.ID
                      && !h.VoidDate.HasValue
                   select new PaymentDetailViewModel
                   {
                       ID = d.ID,
                       PaymentID = d.PaymentID,
                       PaymentTypeID = d.PaymentTypeID,
                       PaymentType = pt.Description,
                       CreditCardTypeID = d.CreditCardTypeID,
                       CreditCardType = t.Description,
                       ApprovalCode = d.ApprovalCode,
                       Amount = d.Amount
                   };
        }

        return null;
    }

    public string Create(
        DateTime date,
        string invoiceNo,
        IEnumerable<PaymentDetailViewModel> detail)
    {
        string paymentNo = String.Empty;

        var invoice = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
        if (invoice != null)
        {
            paymentNo = autoNumberProvider.Generate(invoice.BranchID, "PM", date.Month, date.Year);

            PaymentHeader h = new PaymentHeader();
            h.Date = date;
            h.InvoiceID = invoice.ID;
            h.PaymentNo = paymentNo;
            h.VoidDate = (DateTime?)null;
            EntityHelper.SetAuditFieldForInsert(h, HttpContext.Current.User.Identity.Name);
            foreach (var model in detail)
            {
                PaymentDetail d = new PaymentDetail();
                d.CreditCardTypeID = model.CreditCardTypeID;
                d.PaymentTypeID = model.PaymentTypeID;
                d.Amount = model.Amount;
                d.ApprovalCode = model.ApprovalCode;
                h.PaymentDetails.Add(d);
                ctx.PaymentHeaders.InsertOnSubmit(h);
            }

            autoNumberProvider.Increment("PM", invoice.BranchID, date.Year);
            ctx.SubmitChanges();
        }

        return paymentNo;
        
    }

    public void Edit(
        DateTime date,
        string invoiceNo,
        IEnumerable<PaymentDetailViewModel> detail)
    {
        var invoice = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
        if (invoice != null)
        {
            PaymentHeader h = ctx.PaymentHeaders.SingleOrDefault(pay => pay.InvoiceID == invoice.ID);
            if (h != null)
            {
                ctx.PaymentDetails.DeleteAllOnSubmit(
                    ctx.PaymentDetails.Where(pay => pay.PaymentID == h.ID));

                h.Date = date;
                h.InvoiceID = invoice.ID;
                EntityHelper.SetAuditFieldForInsert(h, HttpContext.Current.User.Identity.Name);
                foreach (var model in detail)
                {
                    PaymentDetail d = new PaymentDetail();
                    d.CreditCardTypeID = model.CreditCardTypeID;
                    d.PaymentTypeID = model.PaymentTypeID;
                    d.Amount = model.Amount;
                    d.ApprovalCode = model.ApprovalCode;
                    h.PaymentDetails.Add(d);
                    ctx.PaymentHeaders.InsertOnSubmit(h);
                }

                autoNumberProvider.Increment("PM", invoice.BranchID, date.Year);
                ctx.SubmitChanges();
            }
        }

    }

}