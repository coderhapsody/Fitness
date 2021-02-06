using FitnessManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SchoolProvider
/// </summary>
public class SchoolProvider
{
    private FitnessDataContext ctx;

    public SchoolProvider(FitnessDataContext context)
    {
        ctx = context;
    }

    public void Add(string name)
    {
        School school = new School();
        school.Name = name;
        EntityHelper.SetAuditFieldForInsert(school, HttpContext.Current.User.Identity.Name);
        ctx.Schools.InsertOnSubmit(school);
        ctx.SubmitChanges();
    }

    public void Update(int id, string name)
    {
        School school = ctx.Schools.Where(row => row.ID == id).Single();
        school.Name = name;
        EntityHelper.SetAuditFieldForUpdate(school, HttpContext.Current.User.Identity.Name);
        ctx.SubmitChanges();
    }

    public void Delete(int[] schoolsID)
    {
        ctx.Schools.DeleteAllOnSubmit(ctx.Schools.Where(row => schoolsID.Contains(row.ID)));
        ctx.SubmitChanges();
    }

    public School Get(int id)
    {
        return ctx.Schools.Where(row => row.ID == id).SingleOrDefault();
    }

    public IEnumerable<School> GetAll()
    {
        return ctx.Schools;
    }

    public string GetSchoolName(int id)
    {
        string result = String.Empty;
        var school = ctx.Schools.Where(row => row.ID == id).SingleOrDefault();
        result = school == null ? String.Empty : school.Name;
        return result;
    }
}