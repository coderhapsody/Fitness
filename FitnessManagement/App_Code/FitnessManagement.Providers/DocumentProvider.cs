using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using FitnessManagement.Providers;

/// <summary>
/// Summary description for DocumentProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class DocumentProvider
    {
        FitnessManagement.Data.FitnessDataContext ctx;

        AutoNumberProvider autoNumberProvider;

        public DocumentProvider(FitnessDataContext context)
        {
            this.ctx = context;
            this.autoNumberProvider = new AutoNumberProvider(context);
        }

        public IEnumerable<DocumentType> GetAllDocumentTypes()
        {
            return ctx.DocumentTypes;
        }

        public DocumentType GetDocumentType(int id)
        {
            return ctx.DocumentTypes.SingleOrDefault(doc => doc.ID == id);
        }

        public ChangeStatusDocument GetChangeStatusDocument(int id)
        {
            return ctx.ChangeStatusDocuments.SingleOrDefault(doc => doc.ID == id);
        }

        public bool CanChangeStatus(string customerCode)
        {
            var query = from cust in ctx.Customers
                        join contract in ctx.Contracts on cust.ID equals contract.CustomerID
                        join invoice in ctx.InvoiceHeaders on contract.ID equals invoice.ContractID
                        where !invoice.VoidDate.HasValue
                           && cust.Barcode == customerCode
                           && invoice.InvoiceType == InvoiceProvider.FRESH_MEMBER_INVOICE
                        select cust;
            return query.Any();
        }

        public void Add(
            int branchID,
            DateTime date,
            DateTime startDate,
            DateTime? endDate,
            string customerCode,
            int documentTypeID,
            string notes)
        {
            Customer cust = ctx.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            Employee emp = ctx.Employees.SingleOrDefault(e => e.UserName == HttpContext.Current.User.Identity.Name);
            if (cust != null && emp != null)
            {
                ChangeStatusDocument doc = new ChangeStatusDocument();
                doc.BranchID = branchID;
                doc.DocumentNo = autoNumberProvider.Generate(branchID, "CS", date.Month, date.Year);
                doc.Date = date;
                doc.StartDate = startDate;
                doc.EndDate = endDate;
                doc.CustomerID = cust.ID;
                doc.EmployeeID = emp.ID;
                doc.DocumentTypeID = documentTypeID;
                doc.Notes = notes;
                EntityHelper.SetAuditFieldForInsert(doc, HttpContext.Current.User.Identity.Name);
                ctx.ChangeStatusDocuments.InsertOnSubmit(doc);

                autoNumberProvider.Increment("CS", doc.BranchID, doc.Date.Year);
                ctx.SubmitChanges();
            }
        }

        public void Update(
            int id,
            DateTime date,
            DateTime startDate,
            DateTime? endDate,
            string customerCode,
            int documentTypeID,
            string notes)
        {
            ChangeStatusDocument doc = ctx.ChangeStatusDocuments.Single(d => d.ID == id);
            Customer cust = ctx.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            Employee emp = ctx.Employees.SingleOrDefault(e => e.UserName == HttpContext.Current.User.Identity.Name);
            if (cust != null && doc != null && emp != null)
            {
                doc.Date = date;
                doc.StartDate = startDate;
                doc.EndDate = endDate;
                doc.CustomerID = cust.ID;
                doc.EmployeeID = emp.ID;
                doc.DocumentTypeID = documentTypeID;
                doc.Notes = notes;
                EntityHelper.SetAuditFieldForUpdate(doc, HttpContext.Current.User.Identity.Name);



                ctx.SubmitChanges();
            }
        }

        public void Approve(int id)
        {
            ChangeStatusDocument doc = ctx.ChangeStatusDocuments.Single(d => d.ID == id);
            Employee emp = ctx.Employees.SingleOrDefault(e => e.ID == doc.EmployeeID);
            if (emp != null && doc != null)
            {
                doc.ApprovedDate = DateTime.Now;
                doc.ApprovedByEmployeeID = emp.ID;
                EntityHelper.SetAuditFieldForUpdate(doc, HttpContext.Current.User.Identity.Name);

                CustomerStatusHistory csh = new CustomerStatusHistory();
                csh.StartDate = doc.StartDate;
                csh.EndDate = doc.EndDate;
                csh.Notes = doc.Notes;
                csh.CustomerID = doc.CustomerID;
                csh.Date = DateTime.Today;
                csh.ChangeStatusDocument = doc;
                EntityHelper.SetAuditFieldForInsert(csh, HttpContext.Current.User.Identity.Name);

                if(doc.DocumentType.ChangeCustomerStatusIDTo.HasValue)
                    csh.CustomerStatusID = doc.DocumentType.ChangeCustomerStatusIDTo.Value;

                //if (doc.DocumentType.IsLastState)
                //{
                //    foreach (Contract activeContract in doc.Customer.Contracts.Where(con => con.Status == ContractStatus.PAID))
                //        activeContract.Status = ContractStatus.CLOSED;
                //}
                 
                ctx.CustomerStatusHistories.InsertOnSubmit(csh);

                ctx.SubmitChanges();
            }
        }

        public void Void(int id)
        {
            var doc = ctx.ChangeStatusDocuments.Single(d => d.ID == id);
            doc.VoidDate = DateTime.Now;
            EntityHelper.SetAuditFieldForUpdate(doc, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }
    }
}