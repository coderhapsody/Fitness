using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for PaymentTypeProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class PaymentTypeProvider
    {
        private FitnessDataContext ctx;

        public PaymentTypeProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(
            string description,
            bool isActive)
        {
            using (var ctx = new FitnessDataContext())
            {
                PaymentType pay = new PaymentType();
                pay.Description = description;
                pay.IsActive = isActive;
                EntityHelper.SetAuditFieldForInsert(pay, HttpContext.Current.User.Identity.Name);
                ctx.PaymentTypes.InsertOnSubmit(pay);
            }
        }

        public void Update(
            int id,
            string description,
            bool isActive)
        {
            using (var ctx = new FitnessDataContext())
            {
                PaymentType pay = ctx.PaymentTypes.Single(row => row.ID == id);
                pay.Description = description;
                pay.IsActive = isActive;
                EntityHelper.SetAuditFieldForUpdate(pay, HttpContext.Current.User.Identity.Name);
                ctx.SubmitChanges();
            }
        }

        public void Delete(int[] paymentTypesID)
        {
            using (var ctx = new FitnessDataContext())
            {
                ctx.PaymentTypes.DeleteAllOnSubmit(ctx.PaymentTypes.Where(row => paymentTypesID.Contains(row.ID)));
                ctx.SubmitChanges();
            }
        }

        public string GetDescription(int paymentTypeID)
        {
            using (var ctx = new FitnessDataContext())
            {
                return ctx.PaymentTypes.Any(row => row.ID == paymentTypeID) ?
                    ctx.PaymentTypes.Single(row => row.ID == paymentTypeID).Description : String.Empty;
            }

        }

        public PaymentType Get(int paymentTypeID)
        {
            using (var ctx = new FitnessDataContext())
            {
                return ctx.PaymentTypes.SingleOrDefault(row => row.ID == paymentTypeID);
            }

        }

        public IEnumerable<PaymentType> GetAll()
        {
            return ctx.PaymentTypes.Where(row => row.IsActive);
        }
    }
}