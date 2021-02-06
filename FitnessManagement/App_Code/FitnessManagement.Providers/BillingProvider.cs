using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for BillingProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class BillingProvider
    {
        private FitnessDataContext ctx;
        public BillingProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public BillingHeader GetBillingInfo(string batchNo)
        {
            return ctx.BillingHeaders.SingleOrDefault(b => b.BatchNo == batchNo);
        }

        public void DeleteBillingHistoryByProcessDate(DateTime date)
        {
            foreach(var billing in ctx.BillingHeaders.Where(b => b.ProcessDate == date))
            {
                ctx.BillingDetails.DeleteAllOnSubmit(billing.BillingDetails);
                ctx.BillingHeaders.DeleteOnSubmit(billing);
            }
            ctx.SubmitChanges();
        }

        public IEnumerable<int> GetYears()
        {
            IList<int> years = ctx.BillingHeaders.Select(b => b.ProcessDate.Year).Distinct().ToList();
            if (years.Count == 0)
                years.Add(DateTime.Today.Year);

            return years;
        }

        public bool IsMerchantCodeValid(int branchID)
        {
            Branch branch = ctx.Branches.SingleOrDefault(br => br.ID == branchID);
            if (branch != null)
            {
                return !String.IsNullOrEmpty(branch.MerchantCode.Trim());
            }

            return false;
        }

        public string ProcessBilling(int branchID, int billingTypeID, string[] contractNumbers, DateTime processDate, string processedByUserName, DateTime nextDueDateFrom, DateTime nextDueDateTo)
        {
            
            string billingFile = String.Empty;
            try
            {
                Employee employee = ctx.Employees.SingleOrDefault(emp => emp.UserName == processedByUserName);
                Branch branch = ctx.Branches.SingleOrDefault(b => b.ID == branchID);
                if (employee != null)
                {
                    billingFile = String.Format("{0}{1}.txt", branch.Code, processDate.ToString("ddMMyy"));

                    IList<BillingViewModel> bills = null;
                    IEnumerable<BillingViewModel> billings = GenerateBillingData(branchID, contractNumbers, nextDueDateFrom, nextDueDateTo);
                    bills = billings.ToList();
                    bills = CreateBillingInvoices(branchID, billingTypeID, bills, processDate, employee.ID, billingFile);
                    CreateBillingFile(branchID, bills, processDate);                    
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Write(ex);
                throw;
            }

            return billingFile;
        }

        public string GenerateBillingUnpaidInvoice(int branchID, string[] invoicesNo)
        {
            List<BillingViewModel> list = new List<BillingViewModel>();
            Branch branch = ctx.Branches.SingleOrDefault(b => b.ID == branchID);
            string fileName = "";
            if (branch != null)
            {
                foreach (string invoiceNo in invoicesNo)
                {
                    InvoiceHeader invoice = ctx.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
                    if (invoice != null)
                    {
                        BillingViewModel billing = new BillingViewModel();
                        billing.MerchantCode = branch.MerchantCode;
                        billing.CustomerBarcode = invoice.Customer.Barcode;
                        billing.CreditCardNo = invoice.Customer.CardNo;
                        billing.CreditCardExpiredDate = invoice.Customer.ExpiredDate.Value;
                        billing.DuesAmount = invoice.InvoiceDetails.Sum(inv => (inv.Quantity * inv.UnitPrice) - (inv.Discount / 100 * inv.Quantity * inv.UnitPrice));
                        billing.CreditCardName = invoice.Customer.CardHolderName;
                        billing.Note = "THE GYM MONTHLY PAYMENT," + invoice.InvoiceNo;
                        list.Add(billing);
                    }
                }
            }
            fileName = CreateBillingFile(branchID, list, DateTime.Today, true);
            return fileName;
        }

        private string CreateBillingFile(int branchID, IEnumerable<BillingViewModel> billings, DateTime processDate, bool resend = false)
        {
            Branch branch = ctx.Branches.SingleOrDefault(b => b.ID == branchID);
            string branchCode = branch.Code;
            string fileName = String.Format("{0}{1}.txt", branchCode, processDate.ToString("ddMMyy"));
            IList<string> lines = new List<string>();
            foreach (BillingViewModel billing in billings)
            {
                StringBuilder billingLine = new StringBuilder();
                billingLine.Append(billing.MerchantCode);
                billingLine.Append(billing.CustomerBarcode.PadRight(25));
                billingLine.Append(billing.CreditCardNo);
                billingLine.Append(billing.CreditCardExpiredDate.ToString("MMyy"));
                billingLine.Append("000");
                billingLine.Append(Convert.ToInt32(billing.DuesAmount));
                billingLine.Append("00");
                billingLine.Append(billing.CreditCardName.PadRight(26));
                billingLine.Append(billing.Note.PadRight(71));
                billingLine.Append("0");                
                lines.Add(billingLine.ToString());
            }
            if (resend)
                fileName = "RESEND_" + fileName;
            File.WriteAllLines(HttpContext.Current.Server.MapPath("~/Billing/") + fileName, lines.ToArray());

            return fileName;
        }

        private IEnumerable<BillingViewModel> GenerateBillingData(int branchID, string[] contractNumbers, DateTime nextDueDateFrom, DateTime nextDueDateTo)
        {
            Branch branch = ctx.Branches.SingleOrDefault(b => b.ID == branchID);
            if (branch != null)
            {
                string merchantCode = branch.MerchantCode;
                foreach (string contractNo in contractNumbers)
                {

                    Contract contract = ctx.Contracts.SingleOrDefault(c => c.ContractNo == contractNo);
                    Customer customer =  contract.Customer;
                    decimal duesAmount = 0;                    

                    if(contract != null && customer != null)
                    {
                        var query = (from data in ctx.AllCustomersDetails
                                     where data.CustomerID == customer.ID
                                       && data.NextDuesDate.Value >= nextDueDateFrom
                                       && data.NextDuesDate.Value <= nextDueDateTo
                                     select data).ToList();
                        if (query != null)
                        {
                            duesAmount = query.Last().CustomerStatusID == 6 ? contract.PackageHeader.FreezeFee : contract.DuesAmount;
                        }

                        string customerBarcode = customer.Barcode;
                        string creditCardNo = customer.CardNo;
                        DateTime creditCardExpiredDate = customer.ExpiredDate.Value;
                        string creditCardName = customer.CardHolderName;
                        //decimal duesAmount = duesAmount;
                        string note = "THE GYM MONTHLY PAYMENT"; // contractNo;
                        yield return new BillingViewModel
                        {
                            MerchantCode = merchantCode,
                            CreditCardExpiredDate = creditCardExpiredDate,
                            CreditCardName = creditCardName,
                            CreditCardNo = creditCardNo,
                            CustomerBarcode = customerBarcode,
                            Note = note,
                            DuesAmount = duesAmount,
                            ContractNo = contractNo
                        };
                    }
                    
                }
            }
        }

        private IList<BillingViewModel> CreateBillingInvoices(int branchID, int billingTypeID, IEnumerable<BillingViewModel> billings, DateTime processDate, int processedByEmployeeID, string billingFileName)
        {
            List<BillingViewModel> list = new List<BillingViewModel>();
            InvoiceProvider invoiceProvider = new InvoiceProvider(ctx);
            AutoNumberProvider autoNumberProvider = new AutoNumberProvider(ctx);
            string autoNumber = autoNumberProvider.Generate(branchID, "BL", processDate.Month, processDate.Year);            

            BillingHeader billingHeader = new BillingHeader();
            billingHeader.BatchNo = autoNumber;
            billingHeader.BillingTypeID = billingTypeID;
            billingHeader.BranchID = branchID;
            billingHeader.UserName = ctx.Employees.Single(emp => emp.ID == processedByEmployeeID).UserName;
            billingHeader.ProcessDate = processDate;
            billingHeader.FileName = billingFileName;

            foreach (BillingViewModel billing in billings)
            {
                Contract contract = ctx.Contracts.SingleOrDefault(c => c.ContractNo == billing.ContractNo);
                Customer customer = contract.Customer;
                PackageHeader package = contract.PackageHeader;
                if (contract != null && package != null && customer != null)
                {
                    InvoiceDetailViewModel invoiceDetail = new InvoiceDetailViewModel();
                    invoiceDetail.InvoiceID = 0;
                    invoiceDetail.ItemID = contract.BillingItemID.Value;
                    invoiceDetail.Quantity = 1;
                    invoiceDetail.UnitPrice = billing.DuesAmount;
                    invoiceDetail.Discount = 0;
                    invoiceDetail.IsTaxable = true;
                    
                    PaymentDetailViewModel paymentDetail = new PaymentDetailViewModel();
                    paymentDetail.PaymentTypeID = ctx.PaymentTypes.SingleOrDefault(p => p.Description == "Credit Card").ID;
                    paymentDetail.CreditCardTypeID = customer.CreditCardTypeID.HasValue ? customer.CreditCardTypeID.Value : ctx.CreditCardTypes.SingleOrDefault(cc => cc.Description == "Visa").ID;
                    paymentDetail.ApprovalCode = String.Empty;
                    paymentDetail.Amount = billing.DuesAmount;
                    paymentDetail.Notes = "Auto Pay";
                    paymentDetail.PaymentID = 0;

                    InvoiceHeader invoiceHeader = invoiceProvider.CreateExistingMemberInvoiceForBilling(branchID,
                        processDate,
                        customer.Barcode,
                        processedByEmployeeID,
                        "Auto pay " + contract.ContractNo,
                        0,
                        new List<InvoiceDetailViewModel>() { invoiceDetail },
                        new List<PaymentDetailViewModel>() { paymentDetail });


                    BillingDetail billingDetail = new BillingDetail();
                    billingDetail.Amount = billing.DuesAmount;
                    billingDetail.Contract = contract;
                    billingDetail.Customer = customer;
                    billingDetail.InvoiceHeader = invoiceHeader;
                    billingDetail.PackageHeader = package;
                    billingHeader.BillingDetails.Add(billingDetail);
                    
                    ctx.BillingHeaders.InsertOnSubmit(billingHeader);

                    billing.Note += "," + invoiceHeader.InvoiceNo;
                    list.Add(billing);
                }

                if (contract.NextDuesDate.HasValue)
                {
                    contract.NextDuesDate = contract.NextDuesDate.Value.AddMonths(contract.DuesInMonth);
                }
            }
            

            autoNumberProvider.Increment("BL", branchID, processDate.Year);
            ctx.SubmitChanges();

            return list;
        }

        public IEnumerable<AllCustomersDetail> GetBillingData(
            int branchID, 
            int billingTypeID,
            int[] customerStatusID,
            int[] packagesID, 
            DateTime nextDueDateFrom, 
            DateTime nextDueDateTo)
        {
            return from data in ctx.AllCustomersDetails
                   where packagesID.Contains(data.PackageID.Value)
                      && customerStatusID.Contains(data.CustomerStatusID)
                      && data.HomeBranchID == branchID
                      && data.NextDuesDate.HasValue
                      && data.BillingTypeID.HasValue
                      && data.BillingTypeID.Value == billingTypeID
                      && data.NextDuesDate.Value >= nextDueDateFrom
                      && data.NextDuesDate.Value <= nextDueDateTo
                   select data;
        }


        public int ProcessBillingResult(string batchNo, string fileName)
        {
            List<BillingRejectionViewModel> rejections = ReadBillingResultFile(fileName).ToList();
            foreach (BillingRejectionViewModel rejection in rejections)
            {
                InvoiceHeader invoice = ctx.InvoiceHeaders.SingleOrDefault(ih => ih.InvoiceNo == rejection.InvoiceNo && !ih.VoidDate.HasValue);
                if (invoice != null)
                {                    
                    var payments = invoice.PaymentHeaders.Where(ph => !ph.VoidDate.HasValue);
                    foreach(var payment in payments)                    
                    {
                        payment.VoidDate = DateTime.Today;
                        payment.VoidReason = "AUTO PAY DECLINED: " + rejection.DeclineCode;                        
                    }

                    CustomerStatusHistory status = new CustomerStatusHistory();
                    status.Customer = invoice.Customer;
                    status.Date = DateTime.Today;
                    status.CustomerStatusID = 4; // billing problem
                    status.Notes = "AUTO PAY DECLINED OF INVOICE " +  invoice.InvoiceNo + ", REJECTION CODE: " + rejection.DeclineCode;
                    status.StartDate = DateTime.Today;
                    EntityHelper.SetAuditFieldForInsert(status, HttpContext.Current.User.Identity.Name);
                }                              
            }            
           
            BillingHeader billing = ctx.BillingHeaders.SingleOrDefault(b => b.BatchNo == batchNo);
            billing.ResultProcessDate = DateTime.Now;
            foreach (var detail in billing.BillingDetails)
            {
                var rejection = rejections.SingleOrDefault(reject => reject.InvoiceNo == detail.InvoiceHeader.InvoiceNo);
                if(rejection != null)
                {
                    detail.BillingResult = rejection.DeclineCode;
                }
            }

            ctx.SubmitChanges();

            return rejections.Count;
                
            //foreach (var billingDetail in billing.BillingDetails)
            //{                
            //    InvoiceHeader invoice = billingDetail.InvoiceHeader;
            //    if (invoice != null)
            //    {
            //        PaymentHeader payment = invoice.PaymentHeaders.SingleOrDefault(ph => !ph.VoidDate.HasValue);
            //        if (payment != null)
            //        {
            //            payment.VoidDate = DateTime.Today;
            //            payment.VoidReason = "AUTO PAY DECLINED: " + rejection.DeclineCode;
            //        }
            //    }
            //}
        }

        public IEnumerable<BillingAcceptedViewModel> ReadBillingUnpaidAcceptedFile(string fileName)
        {
            IList<BillingAcceptedViewModel> list = new List<BillingAcceptedViewModel>();
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                BillingAcceptedViewModel model = new BillingAcceptedViewModel();

                model.MerchantCode = line.Substring(0, 8).Trim();
                model.CustomerBarcode = line.Substring(9, 25).Trim();
                model.CreditCardNo = line.Substring(34, 16).Trim();
                model.CreditCardExpiredDate = new DateTime(Convert.ToInt32("20" + line.Substring(52, 2)), Convert.ToInt32(line.Substring(50, 2)), 1);
                model.CreditCardName = line.Substring(66, 26).Trim();
                model.Note = line.Substring(91, 71).Trim();

                try
                {
                    string[] notes = model.Note.Split(',');
                    //model.ContractNo = notes[0];
                    model.InvoiceNo = notes[1];
                    model.InvoiceNo = model.InvoiceNo.Replace(".", "X");
                }
                catch
                {
                }
                
                model.VerificationCode = line.Substring(161, 10).Trim();

                yield return model;
            }
        }


        public IEnumerable<BillingRejectionViewModel> ReadBillingResultFile(string fileName)
        {
            IList<BillingRejectionViewModel> list = new List<BillingRejectionViewModel>();
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                BillingRejectionViewModel model = new BillingRejectionViewModel();

                model.MerchantCode = line.Substring(0, 8).Trim();
                model.CustomerBarcode = line.Substring(9, 25).Trim();
                model.CreditCardNo = line.Substring(34, 16).Trim();
                model.CreditCardExpiredDate = new DateTime(Convert.ToInt32("20" + line.Substring(52,2)), Convert.ToInt32(line.Substring(50, 2)), 1);
                model.CreditCardName = line.Substring(66, 26).Trim();
                model.Note = line.Substring(91, 71).Trim();

                try
                {
                    string[] notes = model.Note.Split(',');
                    //model.ContractNo = notes[0];
                    model.InvoiceNo = notes[1];
                    model.InvoiceNo = model.InvoiceNo.Replace(".", "X");
                }
                catch
                {
                }

                model.DeclineCode = line.Substring(169, 2).Trim();

                yield return model;
            }
        }

        public int ProcessBillingUnpaidInvoice(string fileName)
        {
            // accepted
            List<BillingAcceptedViewModel> acceptedInvoices = ReadBillingUnpaidAcceptedFile(fileName).ToList();
            AutoNumberProvider autoNumberProvider = new AutoNumberProvider(ctx);
            foreach (BillingAcceptedViewModel acceptedInvoice in acceptedInvoices)
            {
                InvoiceHeader invoice = ctx.InvoiceHeaders.SingleOrDefault(ih => ih.InvoiceNo == acceptedInvoice.InvoiceNo);
                Customer cust = invoice.Customer;
                if (invoice != null)
                {
                    var payments = invoice.PaymentHeaders.Where(ph => !ph.VoidDate.HasValue);
                    foreach (var payment in payments)
                    {
                        payment.VoidDate = DateTime.Today;
                        //payment.VoidReason = "AUTO PAY DECLINED: " + acceptedInvoice.DeclineCode;
                    }

                    string paymentNo = autoNumberProvider.Generate(invoice.BranchID, "PM", DateTime.Today.Month, DateTime.Today.Year);

                    PaymentHeader h = new PaymentHeader();
                    h.Date = DateTime.Today;
                    h.InvoiceID = invoice.ID;
                    h.PaymentNo = paymentNo;
                    h.VoidDate = (DateTime?)null;
                    EntityHelper.SetAuditFieldForInsert(h, HttpContext.Current.User.Identity.Name);
                    
                    PaymentDetail d = new PaymentDetail();
                    d.CreditCardTypeID = cust.CreditCardTypeID;
                    d.PaymentTypeID = 4; // credit  card
                    d.Amount = invoice.InvoiceDetails.Sum(inv => (inv.Quantity * inv.UnitPrice) - (inv.Discount / 100 * (inv.Quantity * inv.UnitPrice)));
                    d.ApprovalCode = acceptedInvoice.VerificationCode;
                    h.PaymentDetails.Add(d);
                    ctx.PaymentHeaders.InsertOnSubmit(h);                    

                    autoNumberProvider.Increment("PM", invoice.BranchID, DateTime.Today.Year);

                    CustomerStatusHistory status = new CustomerStatusHistory();
                    status.Customer = invoice.Customer;
                    status.Date = DateTime.Today;
                    status.CustomerStatusID = 1; // OK
                    status.Notes = "AUTO PAY ACCEPTED FOR INVOICE " + invoice.InvoiceNo;
                    status.StartDate = DateTime.Today;
                    EntityHelper.SetAuditFieldForInsert(status, HttpContext.Current.User.Identity.Name);
                }
            }

            ctx.SubmitChanges();

            return acceptedInvoices.Count;
        }
    }
}