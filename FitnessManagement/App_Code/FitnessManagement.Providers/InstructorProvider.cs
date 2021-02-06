using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InstructorProvider
/// </summary>
public class InstructorProvider
{
    private FitnessDataContext ctx;

    public InstructorProvider(FitnessDataContext context)
	{
        this.ctx = context;	
	}

    public void AddInstructor(
        string barcode,
        string name,
        DateTime hireDate,
        string status,
        string email,
        string homePhone,
        string cellPhone,
        bool isActive)
    {
        Instructor inst = new Instructor();
        inst.Barcode = barcode;
        inst.Name = name;
        inst.HireDate = hireDate;
        inst.Status = status;
        inst.Email = email;
        inst.HomePhone = homePhone;
        inst.CellPhone = cellPhone;
        inst.IsActive = isActive;
        EntityHelper.SetAuditFieldForInsert(inst, HttpContext.Current.User.Identity.Name);
        ctx.Instructors.InsertOnSubmit(inst);
        ctx.SubmitChanges();
    }

    public void UpdateInstructor(
        int id,
        string barcode,
        string name,
        DateTime hireDate,
        string status,
        string email,
        string homePhone,
        string cellPhone,
        bool isActive)
    {
        Instructor inst = ctx.Instructors.SingleOrDefault(instructor => instructor.ID == id);
        if (inst != null)
        {
            inst.Barcode = barcode;
            inst.Name = name;
            inst.HireDate = hireDate;
            inst.Status = status;
            inst.Email = email;
            inst.HomePhone = homePhone;
            inst.CellPhone = cellPhone;
            inst.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(inst, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }
    }

    public void DeleteInstructor(int id)
    {
        Instructor inst = ctx.Instructors.SingleOrDefault(instructor => instructor.ID == id);
        if (inst != null)
        {
            ctx.Instructors.DeleteOnSubmit(inst);
            ctx.SubmitChanges();
        }
    }
    

    public IEnumerable<Instructor> GetAllInstructors()
    {
        return ctx.Instructors;
    }

    public IEnumerable<Instructor> GetActiveInstructors()
    {
        return ctx.Instructors.Where(inst => inst.IsActive);
    }


    public Instructor GetInstructor(int id)
    {
        return ctx.Instructors.SingleOrDefault(inst => inst.ID == id);
    }

    public Instructor GetInstructor(string barcode)
    {
        return ctx.Instructors.SingleOrDefault(inst => inst.Barcode == barcode);
    }

    public InstructorCheckInViewModel DoCheckIn(int branchID, string barcode)
    {
        InstructorCheckInViewModel instVM = null;

        Instructor inst = GetInstructor(barcode);
        if (inst != null)
        {
            InstructorAttendance instAtt = new InstructorAttendance();
            instAtt.InstructorID = inst.ID;
            instAtt.Date = DateTime.Today;
            instAtt.AttendanceIn = DateTime.Now;
            instAtt.BranchID = branchID;

            instVM = new InstructorCheckInViewModel();
            instVM.Barcode = inst.Barcode;
            instVM.BranchID = branchID;
            instVM.CheckInWhen = instAtt.AttendanceIn;
            instVM.Name = inst.Name;

            ctx.InstructorAttendances.InsertOnSubmit(instAtt);
            ctx.SubmitChanges();
        }

        return instVM;
    }

    public List<InstructorCheckInViewModel> GetInstructorCheckInHistory(int branchID)
    {
        var query = from inst in ctx.InstructorAttendances
                    where inst.BranchID == branchID
                    orderby inst.AttendanceIn descending
                    select new InstructorCheckInViewModel
                    {
                        BranchID = branchID,
                        Barcode = inst.Instructor.Barcode,
                        CheckInWhen = inst.AttendanceIn,
                        Name = inst.Instructor.Name
                    };
        return query.ToList();
    }
}