using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for PersonProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public static class PersonConnection
    {
        public static readonly string Father = "F";
        public static readonly string Mother = "M";
        public static readonly string Guardian = "G";
        public static readonly string PickUp = "P";
    }

    public class PersonProvider
    {


        private FitnessDataContext ctx;

        public PersonProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public bool FatherIsExist(string customerCode)
        {
            return ctx.Persons.Where(p => p.Customer.Barcode == customerCode && p.Connection == "F").Count() > 0;            
        }

        public bool MotherIsExist(string customerCode)
        {
            return ctx.Persons.Where(p => p.Customer.Barcode == customerCode && p.Connection == "M").Count() > 0;
        }

        public void Add(
            string customerCode,
            string connection,
            string name,
            DateTime? birthDate,
            string idCardNo,
            string email,
            string phone1,
            string phone2,
            string photo)
        {
            Customer customer = ctx.Customers.SingleOrDefault(cust => cust.Barcode == customerCode);
            if (customer != null)
            {
                Person obj = new Person();
                obj.Connection = connection;
                obj.Name = name;
                obj.BirthDate = birthDate;
                obj.IDCardNo = idCardNo;
                obj.Phone1 = phone1;
                obj.Phone2 = phone2;
                obj.Photo = photo;
                obj.Customer = customer;
                EntityHelper.SetAuditFieldForInsert(obj, HttpContext.Current.User.Identity.Name);
                ctx.Persons.InsertOnSubmit(obj);
                ctx.SubmitChanges();
            }
        }


        public void Update(
            int id, 
            string customerCode,
            string connection,
            string name,
            DateTime? birthDate,
            string idCardNo,
            string email,
            string phone1,
            string phone2,
            string photo     
            )
        {
            Customer customer = ctx.Customers.SingleOrDefault(cust => cust.Barcode == customerCode);
            if (customer != null)
            {
                Person obj = ctx.Persons.Single(row => row.ID == id);
                obj.Connection = connection;
                obj.Name = name;
                obj.BirthDate = birthDate;
                obj.IDCardNo = idCardNo;
                obj.Phone1 = phone1;
                obj.Phone2 = phone2;
                obj.Email = email;
                obj.Photo = photo;
                obj.Customer = customer;
                EntityHelper.SetAuditFieldForUpdate(obj, HttpContext.Current.User.Identity.Name);
                ctx.SubmitChanges();
            }
        }

        public void Delete(int[] id)
        {
            ctx.Persons.DeleteAllOnSubmit(
                ctx.Persons.Where(row => id.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public Person Get(int id)
        {
            return ctx.Persons.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<Person> GetParentsInformation(string customerCode)
        {
            return ctx.Persons.Where(row => row.Customer.Barcode == customerCode);
        }

        public void UpdatePhoto(int id, string fileName)
        {
            Person obj = ctx.Persons.Single(row => row.ID == id);
            obj.Photo = fileName;
            EntityHelper.SetAuditFieldForUpdate(obj, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void DeletePhoto(int id)
        {
            Person obj = ctx.Persons.Single(row => row.ID == id);
            obj.Photo = null;
            EntityHelper.SetAuditFieldForUpdate(obj, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }
    }
}