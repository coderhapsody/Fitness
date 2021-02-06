using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for FormAccessProvider
/// </summary>
public class FormAccessProvider
{
    FitnessDataContext ctx;

    public FormAccessProvider(FitnessDataContext context)
	{
        this.ctx = context;
	}

    public FormAccess Get(string userName, string formUrl)
    {
        var query = from frm in ctx.FormAccesses
                    where frm.FormUrl == formUrl && frm.UserName == userName
                    select frm;
        return query.SingleOrDefault();
    }

    public void Save(string userName, string url, bool allowAddNew, bool allowUpdate, bool allowDelete, bool allowRead)
    {
        bool isNew = false;
        var form = ctx.FormAccesses.SingleOrDefault(frm => frm.FormUrl == url && frm.UserName == userName);
        if (form == null)
        {
            form = new FormAccess();
            isNew = true;
        }

        form.UserName = userName;
        form.FormUrl = url;
        form.CanAddNew = allowAddNew;
        form.CanDelete = allowDelete;
        form.CanRead = allowRead;
        form.CanUpdate = allowUpdate;

        if (isNew)
            ctx.FormAccesses.InsertOnSubmit(form);

        ctx.SubmitChanges();
    }
}