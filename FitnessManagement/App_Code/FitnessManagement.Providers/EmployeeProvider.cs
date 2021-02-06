using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using System.Web.Security;

/// <summary>
/// Summary description for EmployeeProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class EmployeeProvider
    {
        private FitnessDataContext ctx;

        public EmployeeProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public bool CanApproveDocument(string userName)
        {
            return ctx.Employees.Single(emp => emp.UserName == userName).CanApproveDocument;
        }

        public bool CanReprintInvoice(string userName)
        {
            return ctx.Employees.Single(emp => emp.UserName == userName).CanReprint;
        }

        public void Add(string userName,
                        string barcode,
                        string firstName,
                        int homeBranchID,
                        string email,
                        bool canApproveDocument)
        {
            Employee emp = new Employee();
            emp.UserName = userName;
            emp.Barcode = barcode;
            emp.FirstName = firstName;
            emp.HomeBranchID = homeBranchID;
            emp.Email = email;
            emp.IsActive = true;
            emp.CanApproveDocument = canApproveDocument;
            EntityHelper.SetAuditFieldForInsert(emp, HttpContext.Current.User.Identity.Name);
            ctx.Employees.InsertOnSubmit(emp);
            ctx.SubmitChanges();
        }

        public void Update(string userName, string barcode, string firstName, int homeBranchID, string email)
        {
            Employee emp = ctx.Employees.SingleOrDefault(user => user.UserName == userName);
            if (emp != null)
            {
                emp.UserName = userName;
                emp.Barcode = barcode;
                emp.FirstName = firstName;
                emp.HomeBranchID = homeBranchID;
                emp.Email = email;
                EntityHelper.SetAuditFieldForUpdate(emp, HttpContext.Current.User.Identity.Name);
                ctx.SubmitChanges();
            }
            else
                Add(userName, barcode, firstName, homeBranchID, email, false);
        }

        public void Update(int id,
                           string barcode,
                           string userName,
                           int homeBranchID,
                           string firstName,
                           string lastName,
                           string phone,
                           string email,
                           bool deletePhoto,
                           string photoFileName,
                           bool isActive,
                           bool canApproveDocument,
                           bool canEditActiveContract,
                           bool canReprint)
        {
            Employee emp = ctx.Employees.Single(row => row.ID == id);
            emp.Barcode = barcode;
            emp.UserName = userName;
            emp.FirstName = firstName;
            emp.LastName = lastName;
            emp.HomeBranchID = homeBranchID;
            emp.Photo = deletePhoto ? null : photoFileName;
            emp.Phone = phone;
            emp.Email = email;
            emp.IsActive = isActive;
            emp.CanApproveDocument = canApproveDocument;
            emp.CanEditActiveContract = canEditActiveContract;
            emp.CanReprint = canReprint;
            EntityHelper.SetAuditFieldForUpdate(emp, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void Delete(string userName)
        {
            Employee emp = ctx.Employees.SingleOrDefault(row => row.UserName == userName);
            if (emp != null)
            {
                ctx.Employees.DeleteOnSubmit(emp);
                ctx.SubmitChanges();
            }
        }

        public Employee Get(string userName)
        {
            return ctx.Employees.SingleOrDefault(row => row.UserName == userName);
        }

        public Employee Get(int employeeID)
        {
            return ctx.Employees.SingleOrDefault(row => row.ID == employeeID);
        }

        public string GetName(string userName)
        {
            Employee emp = Get(userName);
            return emp == null ? String.Empty : String.Format("{0} {1}", emp.FirstName, emp.LastName);
        }

        public IEnumerable<Employee> GetSales(bool activeOnly = true)
        {
            string[] userNames = Roles.GetUsersInRole("Sales");
            var query = from emp in ctx.Employees 
                        where userNames.Contains(emp.UserName)
                        select emp;

            if(activeOnly)
                query = query.Where(employee => employee.IsActive);

            return query.ToList();
        }

        public IEnumerable<Employee> GetAll(int branchID)
        {
            var query = from emp in ctx.Employees
                        where emp.IsActive && emp.HomeBranchID == branchID
                        select emp;
            return query.ToList();
        }

        public IEnumerable<Employee> GetAll()
        {
            var query = from emp in ctx.Employees
                        where emp.IsActive
                        select emp;
            return query.ToList();
        }
    }
}