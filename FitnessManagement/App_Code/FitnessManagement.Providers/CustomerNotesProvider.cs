using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for CustomerNotesProvider
/// </summary>
namespace FitnessManagement.Providers
{

    public class CustomerNotesProvider
    {
        FitnessDataContext ctx;
        public CustomerNotesProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(
            string customerBarcode,
            string notes,
            short priority
            )
        {
            int customerID = ctx.Customers.SingleOrDefault(c => c.Barcode == customerBarcode).ID;
            Add(customerID, notes, priority);
        }

        public void Delete(int id)
        {
            CustomerNote node = ctx.CustomerNotes.SingleOrDefault(note => note.ID == id);
            if (node != null)
            {
                ctx.CustomerNotes.DeleteOnSubmit(node);
                ctx.SubmitChanges();
            }
        }

        public void Add(
            int customerID,
            string notes,
            short priority
            )
        {
            CustomerNote note = new CustomerNote();
            note.CustomerID = customerID;
            note.Notes = notes;
            note.Priority = priority;
            EntityHelper.SetAuditFieldForInsert(note, HttpContext.Current.User.Identity.Name);
            ctx.CustomerNotes.InsertOnSubmit(note);
            ctx.SubmitChanges();
        }

        public void Update(
            int customerID,
            string notes,
            short priority
            )
        {
            CustomerNote note = new CustomerNote();
            note.CustomerID = customerID;
            note.Notes = notes;
            note.Priority = priority;
            EntityHelper.SetAuditFieldForInsert(note, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }




        public void ToggleNote(int id)
        {
            CustomerNote note = ctx.CustomerNotes.SingleOrDefault(n => n.ID == id);
            if (note != null)
            {
                note.Priority = Convert.ToInt16((note.Priority - (short)1) * (short)-1);
                ctx.SubmitChanges();
            }
        }

        public IEnumerable<CustomerNote> GetTopNotes(string customerBarcode)
        {
            var query = (from note in ctx.CustomerNotes
                         join cust in ctx.Customers on note.CustomerID equals cust.ID
                         where cust.Barcode == customerBarcode
                         orderby note.CreatedWhen descending
                         select note).Take(3);
            return query;
        }
    }
}