using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for AreaProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class AreaProvider
    {
        private FitnessDataContext ctx;

        public AreaProvider(FitnessDataContext context)
        {
            ctx = context;
        }

        public void Add(string description)
        {
            Area area = new Area();
            area.Description = description;
            EntityHelper.SetAuditFieldForInsert(area, HttpContext.Current.User.Identity.Name);
            ctx.Areas.InsertOnSubmit(area);
            ctx.SubmitChanges();
        }

        public void Update(int id, string description)
        {
            Area area = ctx.Areas.Where(row => row.ID == id).Single();
            area.Description = description;
            EntityHelper.SetAuditFieldForUpdate(area, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void Delete(int[] areasID)
        {
            ctx.Areas.DeleteAllOnSubmit(ctx.Areas.Where(row => areasID.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public Area Get(int id)
        {
            return ctx.Areas.Where(row => row.ID == id).SingleOrDefault();
        }

        public IEnumerable<Area> GetAll()
        {
            return ctx.Areas;
        }

        public string GetAreaName(int id)
        {
            string result = String.Empty;
            var area = ctx.Areas.Where(row => row.ID == id).SingleOrDefault();
            result = area == null ? String.Empty : area.Description;
            return result;
        }
    }
}