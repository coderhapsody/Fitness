using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for CreditCardTypeProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class CreditCardTypeProvider
    {
        private FitnessDataContext ctx;

        public CreditCardTypeProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(string description)
        {
            CreditCardType obj = new CreditCardType();
            obj.Description = description;
            EntityHelper.SetAuditFieldForInsert(obj, HttpContext.Current.User.Identity.Name);
            ctx.CreditCardTypes.InsertOnSubmit(obj);
            ctx.SubmitChanges();
        }


        public void Update(int id, string description)
        {
            CreditCardType obj = ctx.CreditCardTypes.Single(row => row.ID == id);
            obj.Description = description;
            EntityHelper.SetAuditFieldForUpdate(obj, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void Delete(int[] id)
        {
            ctx.CreditCardTypes.DeleteAllOnSubmit(
                ctx.CreditCardTypes.Where(row => id.Contains(row.ID)));
        }

        public CreditCardType Get(int id)
        {
            return ctx.CreditCardTypes.SingleOrDefault(row => row.ID == id);
        }


        public IEnumerable<CreditCardType> GetAll()
        {
            return ctx.CreditCardTypes;
        }
    }
}