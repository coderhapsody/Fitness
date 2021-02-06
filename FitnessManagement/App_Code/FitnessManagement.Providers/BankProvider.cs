using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using System.Data.Linq;

/// <summary>
/// Summary description for BankProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class BankProvider
    {
        FitnessDataContext ctx;

        public BankProvider(FitnessDataContext context)
        {
            ctx = context;
        }

        public void Add(
            string name,
            bool isActive)
        {
            Bank bank = new Bank();
            bank.Name = name;
            bank.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(bank, HttpContext.Current.User.Identity.Name);
            ctx.Banks.InsertOnSubmit(bank);
            ctx.SubmitChanges();
        }


        public void Update(
            int id,
            string name,
            bool isActive)
        {
            Bank bank = ctx.Banks.Where(row => row.ID == id).Single();
            bank.Name = name;
            bank.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(bank, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }


        public void Delete(int[] banksID)
        {
            ctx.Banks.DeleteAllOnSubmit(ctx.Banks.Where(row => banksID.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public string GetBankName(int id)
        {
            return ctx.Banks.Any(row => row.ID == id) ?
                ctx.Banks.SingleOrDefault(row => row.ID == id).Name : String.Empty;
        }

        public Bank Get(int id)
        {
            return ctx.Banks.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<Bank> GetActiveBanks()
        {
            return ctx.Banks.Where(bank => bank.IsActive);
        }
    }

}