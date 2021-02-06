using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using System.Data.Linq;

/// <summary>
/// Summary description for CustomerStatusProvider
/// </summary>
namespace FitnessManagement.Providers
{
    //public enum CustomerStatusType
    //{
    //    OK = 1,
    //    Pending =2,
    //    Terminated =3,
    //    BillingProblem = 4,
    //    Discontinue =5,
    //    Freeze =6
    //}

    public class CustomerStatusProvider
    {
        FitnessDataContext ctx;

        public CustomerStatusProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(string description, string color, string backgroundColor)
        {
            CustomerStatus obj = new CustomerStatus();
            obj.Description = description;
            obj.Color = color + "|" + backgroundColor;
            EntityHelper.SetAuditFieldForInsert(obj, HttpContext.Current.User.Identity.Name);
            ctx.CustomerStatus.InsertOnSubmit(obj);
            ctx.SubmitChanges();
        }

        public void Update(int id, string description, string color, string backgroundColor)
        {
            CustomerStatus obj = ctx.CustomerStatus.Single(row => row.ID == id);
            obj.Description = description;
            obj.Color = color + "|" + backgroundColor;
            EntityHelper.SetAuditFieldForUpdate(obj, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void Delete(int[] id)
        {
            ctx.CustomerStatus.DeleteAllOnSubmit(
                ctx.CustomerStatus.Where(row => id.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public CustomerStatus Get(int id)
        {
            return ctx.CustomerStatus.SingleOrDefault(row => row.ID == id);
        }

        public string GetStatusColor(string status)
        {
            try
            {
                return ctx.CustomerStatus.SingleOrDefault(row => row.Description == status).Color;
            }
            catch { }

            return "#ff0000|#ffffff";
        }

        public IEnumerable<CustomerStatus> GetAll()
        {
            return ctx.CustomerStatus;
        }

        public IEnumerable<CustomerStatusHistory> GetStatusHistory(string customerBarcode)
        {
            return ctx.CustomerStatusHistories.Where(cust => cust.Customer.Barcode == customerBarcode);
        }

        public CustomerStatusHistory GetLatestStatus(string customerBarcode)
        {            
            return ctx.CustomerStatusHistories
                      .Where(
                          cust =>                                           
                              cust.ChangeStatusDocument.VoidDate.HasValue == false &&
                              cust.Customer.Barcode == customerBarcode && cust.StartDate <= DateTime.Today &&
                              cust.EndDate.GetValueOrDefault(new DateTime(2099, 12, 31)) >= DateTime.Today
                )
                      .OrderByDescending(cs => cs.StartDate).ThenByDescending(cs => cs.ChangedWhen)
                      .Select(c => c)
                      .FirstOrDefault();
            
        }

        public void AddStatusHistory(int customerStatusID, string customerCode, DateTime startDate)
        {
            AddStatusHistory(customerStatusID, customerCode, startDate, (DateTime?)null);
        }

        public void AddStatusHistory(int customerStatusID, string customerCode, DateTime startDate, DateTime? endDate)
        {
            Customer cust = ctx.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            CustomerStatus custStatus = ctx.CustomerStatus.SingleOrDefault(c => c.ID == customerStatusID);
            if (cust != null && custStatus != null)
                AddStatusHistory(custStatus, cust, startDate, endDate);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, string customerCode, DateTime startDate)
        {
            Customer cust = ctx.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            if (cust != null)
                AddStatusHistory(customerStatus, cust, startDate, (DateTime?)null);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, string customerCode, DateTime startDate, DateTime? endDate)
        {
            Customer cust = ctx.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            if (cust != null)
                AddStatusHistory(customerStatus, cust, startDate, endDate);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, Customer customer, DateTime startDate)
        {
            AddStatusHistory(customerStatus, customer, startDate, (DateTime?)null);
        }

        public void AddStatusHistory(CustomerStatus customerStatus, Customer customer, DateTime startDate, DateTime? endDate)
        {
            var csh = new CustomerStatusHistory();
            csh.CustomerStatusID = customerStatus.ID;
            csh.CustomerID = customer.ID;
            csh.StartDate = startDate;
            csh.EndDate = endDate;
            EntityHelper.SetAuditFieldForInsert(csh, HttpContext.Current.User.Identity.Name);
            ctx.CustomerStatusHistories.InsertOnSubmit(csh);
            ctx.SubmitChanges();
        }



        public string GetStatusBackgroundColor(string p)
        {
            throw new NotImplementedException();
        }
    }
}