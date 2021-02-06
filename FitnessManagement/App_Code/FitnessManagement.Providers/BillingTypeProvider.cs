using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for BillingTypeProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public enum BillingTypeEnum
    {
        ManualPayment = 1,
        AutoPayment = 2
    }


    public class BillingTypeProvider
    {
        FitnessDataContext ctx;

        public BillingTypeProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(string description, short autoPayDay, bool isActive)
        {
            BillingType bil = new BillingType();
            bil.Description = description;
            bil.AutoPayDay = autoPayDay;
            bil.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(bil, HttpContext.Current.User.Identity.Name);
            ctx.BillingTypes.InsertOnSubmit(bil);
            ctx.SubmitChanges();
        }

        public void Update(int id, string description, short autoPayDay, bool isActive)
        {
            BillingType bil = ctx.BillingTypes.Single(row => row.ID == id);
            bil.Description = description;
            bil.AutoPayDay = autoPayDay;
            bil.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(bil, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }
        
        public void Delete(int[] billingsID)
        {
            ctx.BillingTypes.DeleteAllOnSubmit(ctx.BillingTypes.Where(row => billingsID.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public BillingType Get(int id)
        {
            return ctx.BillingTypes.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<BillingType> GetActiveBillingTypes()
        {
            return ctx.BillingTypes.Where(b => b.IsActive);
        }

        public IEnumerable<BillingType> GetAll()
        {
            return ctx.BillingTypes;
        }
    }
}