using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for DocumentTypeProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class DocumentTypeProvider
    {
        FitnessDataContext ctx;

        public DocumentTypeProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(string description, bool isLastState, int changeStatusIDTo)
        {
            DocumentType docType = new DocumentType();
            docType.Description = description;
            docType.IsLastState = isLastState;
            docType.ChangeCustomerStatusIDTo = changeStatusIDTo == 0 ? (int?)null : changeStatusIDTo;
            ctx.DocumentTypes.InsertOnSubmit(docType);
            ctx.SubmitChanges();
        }

        public void Update(int id, string description, bool isLastState, int changeStatusIDTo)
        {
            DocumentType docType = ctx.DocumentTypes.SingleOrDefault(dt => dt.ID == id);
            if (docType != null)
            {
                docType.Description = description;
                docType.IsLastState = isLastState;
                docType.ChangeCustomerStatusIDTo = changeStatusIDTo == 0 ? (int?) null : changeStatusIDTo;
                ctx.SubmitChanges();
            }
        }

        public void Delete(int[] id)
        {
            ctx.DocumentTypes.DeleteAllOnSubmit(
                ctx.DocumentTypes.Where(dt => id.Contains(dt.ID)));
            ctx.SubmitChanges();
        }

        public void Delete(int id)
        {
            ctx.DocumentTypes.DeleteOnSubmit(
                ctx.DocumentTypes.Single(dt => dt.ID == id));
            ctx.SubmitChanges();
        }

        public DocumentType Get(int id)
        {
            return ctx.DocumentTypes.SingleOrDefault(docType => docType.ID == id);
        }
    }
}