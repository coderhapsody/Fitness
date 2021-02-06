using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for ItemTypeProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class ItemTypeProvider
    {
        private FitnessDataContext ctx;

        public ItemTypeProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(string description)
        {
            ItemType obj = new ItemType();
            obj.Description = description;
            EntityHelper.SetAuditFieldForInsert(obj, HttpContext.Current.User.Identity.Name);
            ctx.ItemTypes.InsertOnSubmit(obj);
            ctx.SubmitChanges();
        }


        public void Update(int id, string description)
        {
            ItemType obj = ctx.ItemTypes.Single(row => row.ID == id);
            obj.Description = description;
            EntityHelper.SetAuditFieldForUpdate(obj, HttpContext.Current.User.Identity.Name);            
            ctx.SubmitChanges();
        }

        public void Delete(int[] id)
        {
            ctx.ItemTypes.DeleteAllOnSubmit(
                ctx.ItemTypes.Where(row => id.Contains(row.ID)));
        }

        public ItemType Get(int id)
        {
            return ctx.ItemTypes.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<ItemType> GetAll()
        {
            return ctx.ItemTypes;
        }

    }
}