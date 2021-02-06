using FitnessManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.ViewModels;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Collections;

/// <summary>
/// Summary description for ClassProvider
/// </summary>
public class ClassProvider
{
    private FitnessDataContext ctx;
	public ClassProvider(FitnessDataContext context)
	{
        this.ctx = context;
	}

    public IDictionary<int, bool> GetAttendancesStatus(int classRunningID)
    {
        Dictionary<int, bool> result = new Dictionary<int,bool>();
        var query = from att in ctx.ClassAttendances
                    where att.ClassRunningID == classRunningID
                    select new
                    {
                        att.CustomerID,
                        att.IsAttend
                    };
        foreach (var item in query)
            result.Add(item.CustomerID, item.IsAttend);

        return result;
    }

    public void CopyAttendances(int currentClassRunningID, int fromClassRunningID)
    {
        ctx.proc_ClassRunningCopyAttendanceFromDate(currentClassRunningID, fromClassRunningID);
    }

    public ClassRunning GetClassRunning(int classRunningID)
    {
        return ctx.ClassRunnings.SingleOrDefault(run => run.ID == classRunningID);
    }

    public void AddParticipant(int classRunningID, int customerID)
    {
        ClassAttendance classAttendance = new ClassAttendance();
        classAttendance.ClassRunningID = classRunningID;
        classAttendance.CustomerID = customerID;
        classAttendance.IsAttend = false;
        ctx.ClassAttendances.InsertOnSubmit(classAttendance);
        ctx.SubmitChanges();
    }

    public void DeleteParticipant(int classRunningID, int customerID)
    {
        ClassAttendance classAttendance = ctx.ClassAttendances.SingleOrDefault(att => att.ClassRunningID == classRunningID && att.CustomerID == customerID);
        if (classAttendance != null)
        {            
            ctx.ClassAttendances.DeleteOnSubmit(classAttendance);
            ctx.SubmitChanges();
        }
    }

    public void AddTimeSlot(int branchID, int dayOfWeek, string time)
    {
        ClassTimeSlot timeSlot = new ClassTimeSlot();
        timeSlot.BranchID = branchID;
        timeSlot.DayOfWeek = dayOfWeek;
        timeSlot.StartTime = time;
        ctx.ClassTimeSlots.InsertOnSubmit(timeSlot);
        ctx.SubmitChanges();
    }

    public void UpdateTimeSlot(int id, int branchID, int dayOfWeek, string time)
    {
        ClassTimeSlot timeSlot = ctx.ClassTimeSlots.SingleOrDefault(ts => ts.ID == id);
        if (timeSlot != null)
        {
            timeSlot.BranchID = branchID;
            timeSlot.DayOfWeek = dayOfWeek;
            timeSlot.StartTime = time;
            ctx.ClassTimeSlots.InsertOnSubmit(timeSlot);
            ctx.SubmitChanges();
        }
    }

    public void DeleteTimeSlot(int id)
    {
        ClassTimeSlot timeSlot = ctx.ClassTimeSlots.SingleOrDefault(ts => ts.ID == id);
        ctx.ClassTimeSlots.DeleteOnSubmit(timeSlot);
        ctx.SubmitChanges();
    }

    public void DeleteTimeSlot(int[] id)
    {
        foreach (int timeSlotID in id)
        {
            ClassTimeSlot timeSlot = ctx.ClassTimeSlots.SingleOrDefault(ts => ts.ID == timeSlotID);
            ctx.ClassTimeSlots.DeleteOnSubmit(timeSlot);
        }
        ctx.SubmitChanges();        
    }

    public IEnumerable<string> GetTimeSlots(int branchID, int dayOfWeek)
    {
        return ctx.ClassTimeSlots.Where(ts => ts.BranchID == branchID && ts.DayOfWeek == dayOfWeek).Select(ts => ts.StartTime);
    }

    public void AddRoom(string code, string name, bool isActive)
    {
        ClassRoom room = new ClassRoom();
        room.Code = code;
        room.Name = name;        
        room.IsActive = isActive;
        EntityHelper.SetAuditFieldForInsert(room, HttpContext.Current.User.Identity.Name);
        ctx.ClassRooms.InsertOnSubmit(room);
        ctx.SubmitChanges();
    }

    public void UpdateRoom(int id, string code, string name, bool isActive)
    {
        ClassRoom room = ctx.ClassRooms.SingleOrDefault(classRoom => classRoom.ID == id);
        if (room != null)
        {
            room.Code = code;
            room.Name = name;            
            room.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(room, HttpContext.Current.User.Identity.Name);            
            ctx.SubmitChanges();
        }
    }

    public void DeleteRoom(int id)
    {
        ClassRoom room = ctx.ClassRooms.SingleOrDefault(classRoom => classRoom.ID == id);
        ctx.ClassRooms.DeleteOnSubmit(room);
        ctx.SubmitChanges();
    }

    public ClassRoom GetRoom(int id)
    {
        ClassRoom room = ctx.ClassRooms.SingleOrDefault(classRoom => classRoom.ID == id);
        return room;
    }

    public IEnumerable<ClassRoom> GetActiveClassRooms(int branchID)
    {
        return from room in ctx.ClassRooms
               join activeRoom in ctx.ActiveClassRoomInBranches on room.ID equals activeRoom.ClassRoomID
               where room.IsActive && activeRoom.BranchID == branchID
               select room;
    }

    public IEnumerable<ClassRoom> GetAllClassRooms(int branchID)
    {
        return from room in ctx.ClassRooms
               join activeRoom in ctx.ActiveClassRoomInBranches on room.ID equals activeRoom.ClassRoomID
               select room;
    }

    public IEnumerable<ClassRoom> GetAllClassRooms()
    {
        return from room in ctx.ClassRooms               
               select room;
    }

    public void AddClass(string code, string name, bool isActive, bool isPaid)
    {
        Class cls = new Class();
        cls.Code = code;
        cls.Name = name;
        cls.IsActive = isActive;
        cls.IsPaid = isPaid;
        EntityHelper.SetAuditFieldForInsert(cls, HttpContext.Current.User.Identity.Name);
        ctx.Classes.InsertOnSubmit(cls);
        ctx.SubmitChanges();
    }

    public void UpdateClass(int id, string code, string name, bool isActive, bool isPaid)
    {
        Class cls = ctx.Classes.SingleOrDefault(c => c.ID == id);
        if (cls != null)
        {
            cls.Code = code;
            cls.Name = name;
            cls.IsActive = isActive;
            cls.IsPaid = isPaid;
            EntityHelper.SetAuditFieldForUpdate(cls, HttpContext.Current.User.Identity.Name);            
            ctx.SubmitChanges();
        }
    }

    public void DeleteClass(int id)
    {
        Class @class = ctx.Classes.SingleOrDefault(cls => cls.ID == id);
        ctx.Classes.DeleteOnSubmit(@class);
        ctx.SubmitChanges();
    }

    public void DeleteClass(int[] arrayID)
    {
        foreach (int id in arrayID)
        {
            Class @class = ctx.Classes.SingleOrDefault(cls => cls.ID == id);
            ctx.Classes.DeleteOnSubmit(@class);            
        }
        ctx.SubmitChanges();
    }

    public IEnumerable<Class> GetAllClasses()
    {
        return ctx.Classes;
    }

    public IEnumerable<Class> GetAllActiveClasses()
    {
        return ctx.Classes.Where(cls => cls.IsActive);
    }

    public Class GetClass(int id)
    {
        return ctx.Classes.SingleOrDefault(cls => cls.ID == id);
    }


    public void AddSchedule(int dayOfWeek, int branchID, int year, int month, int classID, string level, int roomID, string timeStart, string timeEnd, int instructorID)
    {
        ClassScheduleDetail sch = new ClassScheduleDetail();
        sch.BranchID = branchID;
        sch.Month = Convert.ToByte(month);
        sch.Year = year;
        sch.DayOfWeek = dayOfWeek;
        EntityHelper.SetAuditFieldForInsert(sch, HttpContext.Current.User.Identity.Name);
        sch.ClassID = classID;
        sch.Level = level;
        sch.ClassRoomID = roomID;
        sch.TimeStart = timeStart;
        sch.TimeEnd = timeEnd;
        sch.InstructorID = instructorID;
        ctx.ClassScheduleDetails.InsertOnSubmit(sch);
        ctx.SubmitChanges();
    }

    public IEnumerable<BranchRoomCapacityViewModel> GetBranchesByClassRoom(int roomID)
    {
        return from branch in ctx.Branches
               join activeRoom in ctx.ActiveClassRoomInBranches on branch.ID equals activeRoom.BranchID
               where activeRoom.ClassRoomID == roomID
               select new BranchRoomCapacityViewModel
               {
                   BranchID = branch.ID,
                   CurrentBranch = branch,
                   Capacity = activeRoom.Capacity
               };
    }

    public void UpdateRoomsAtBranch(int roomID, IList<BranchRoomCapacityViewModel> branchesID)
    {
        foreach (var item in ctx.ActiveClassRoomInBranches.Where(room => room.ClassRoomID == roomID))
            ctx.ActiveClassRoomInBranches.DeleteOnSubmit(item);

        foreach (var item in branchesID)
        {
            ActiveClassRoomInBranch room = new ActiveClassRoomInBranch();
            room.ClassRoomID = roomID;
            room.BranchID = item.BranchID;
            room.Capacity = Convert.ToInt16(item.Capacity);
            ctx.ActiveClassRoomInBranches.InsertOnSubmit(room);
        }

        ctx.SubmitChanges();
        
    }

    public void DeleteSchedule(int id)
    {
        var schedule = ctx.ClassScheduleDetails.SingleOrDefault(sch => sch.ID == id);

        foreach (ClassRunning classRunning in schedule.ClassRunnings)
            ctx.ClassRunnings.DeleteOnSubmit(classRunning);

        ctx.ClassScheduleDetails.DeleteOnSubmit(schedule);
        ctx.SubmitChanges();
    }

    public void UploadFromExcel(int branchID, int year, int month, string fileName)
    {
        IList<ExcelClassScheduleViewModel> data = ReadFromExcel(fileName);
        
        ctx.ClassScheduleDetails.DeleteAllOnSubmit(
            ctx.ClassScheduleDetails
            .Where(row => row.BranchID == branchID && 
                          row.Year == year && 
                          row.Month == month));

        foreach (var row in data)
        {
            Class cls = ctx.Classes.SingleOrDefault(item => item.Code == row.ClassCode);
            if (cls != null)
            {
                ClassRoom room = ctx.ClassRooms.SingleOrDefault(item => item.Code == row.ClassRoomCode);
                if (room != null)
                {
                    Instructor inst = ctx.Instructors.SingleOrDefault(item => item.Barcode == row.InstructorBarcode);
                    if (inst != null)
                    {
                        TimeSpan startTime, endTime;
                        if (TimeSpan.TryParse(row.TimeStart, out startTime))
                        {
                            if (TimeSpan.TryParse(row.TimeEnd, out endTime))
                            {
                                if (row.Level.Length <= 10)
                                {
                                    if (row.DayOfWeek >= 1 && row.DayOfWeek <= 7)
                                    {
                                        ClassScheduleDetail csd = new ClassScheduleDetail();
                                        csd.BranchID = branchID;
                                        csd.Year = year;
                                        csd.Month = month;
                                        csd.DayOfWeek = row.DayOfWeek;
                                        csd.ClassID = cls.ID;
                                        csd.ClassRoomID = room.ID;
                                        csd.InstructorID = inst.ID;
                                        csd.TimeStart = row.TimeStart;
                                        csd.TimeEnd = row.TimeEnd;
                                        csd.Level = row.Level;
                                        csd.IsActive = true;
                                        EntityHelper.SetAuditFieldForInsert(csd, HttpContext.Current.User.Identity.Name);
                                        ctx.ClassScheduleDetails.InsertOnSubmit(csd);
                                    }
                                    else
                                    {
                                        throw new Exception("Wrong format in Excel file: day of week should be between 1 and 7, 1 for monday and 7 for sunday");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Wrong format in Excel file: level should be less than 10 chars");
                                }
                            }
                            else
                            {
                                throw new Exception("Wrong format in Excel file: invalid time end for " + row.TimeEnd);
                            }
                        }
                        else
                        {
                            throw new Exception("Wrong format in Excel file: invalid time start for " + row.TimeStart);
                        }
                    }
                    else
                    {
                        throw new Exception("Wrong format in Excel file: No instructor defined for " + row.InstructorBarcode);
                    }
                }
                else
                {
                    throw new Exception("Wrong format in Excel file: No class room defined for " + row.ClassRoomCode);
                }
            }
            else
            {
                throw new Exception("Wrong format in Excel file: No class defined for " + row.ClassCode);
            }
        }

        ctx.SubmitChanges();
    }

    private IList<ExcelClassScheduleViewModel> ReadFromExcel(string fileName)
    {
        IList<ExcelClassScheduleViewModel> result = new List<ExcelClassScheduleViewModel>();

        using(FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            HSSFWorkbook workbook = new HSSFWorkbook(fs);
            HSSFSheet sheet = workbook.GetSheetAt(0) as HSSFSheet;
            IEnumerator rows = sheet.GetRowEnumerator();
            int rowIndex = 1;
            while (rows.MoveNext())
            {
                if (rowIndex > 1)
                {
                    HSSFRow row = rows.Current as HSSFRow;
                    ExcelClassScheduleViewModel model = new ExcelClassScheduleViewModel();
                    model.DayOfWeek = Convert.ToInt32(row.Cells[0].StringCellValue);
                    model.ClassCode = row.Cells[1].StringCellValue;
                    model.Level = row.Cells[2].StringCellValue;
                    model.ClassRoomCode = row.Cells[3].StringCellValue;
                    model.InstructorBarcode = row.Cells[4].StringCellValue;
                    model.TimeStart = row.Cells[5].StringCellValue;
                    model.TimeEnd = row.Cells[6].StringCellValue;
                    result.Add(model);
                }
                rowIndex++;
            }
        }


        return result;
    }

    public void UpdateAttendancesStatus(int classRunningID, int runningInstructorID, int runningAssistantID, string notes, int[] values)
    {
        ClassRunning classRunning = ctx.ClassRunnings.SingleOrDefault(cls => cls.ID == classRunningID);
        if (classRunning != null)
        {
            classRunning.RunningInstructorID = runningInstructorID;
            classRunning.RunningAssistantID = runningAssistantID == 0 ? (int?)null : runningAssistantID;
            classRunning.Notes = notes;

            if(!classRunning.RunningStartWhen.HasValue)
                classRunning.RunningStartWhen = DateTime.Now;

            var attendances = ctx.ClassAttendances.Where(cls => cls.ClassRunningID == classRunningID);
            foreach (var attendance in attendances)
                attendance.IsAttend = false;

            foreach (var value in values)
            {
                ClassAttendance attendance =
                    ctx.ClassAttendances.SingleOrDefault(
                        att => att.ClassRunningID == classRunningID && att.CustomerID == value);
                if (attendance != null)
                {
                    attendance.IsAttend = true;
                }
            }

            ctx.SubmitChanges();
        }
    }

    public ClassScheduleDetail GetSchedule(int id)
    {
        return ctx.ClassScheduleDetails.SingleOrDefault(sched => sched.ID == id);
    }

    public void UpdateSchedule(int id, int classID, int classRoomID, int instructorID, string level)
    {
        ClassScheduleDetail schedule = GetSchedule(id);
        if (schedule != null)
        {
            schedule.ClassID = classID;
            schedule.ClassRoomID = classRoomID;
            schedule.InstructorID = instructorID;
            schedule.Level = level;
            EntityHelper.SetAuditFieldForUpdate(schedule, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }
    }

    public bool VerifyScheduleDeletion(int id)
    {
        ClassScheduleDetail schedule = GetSchedule(id);
        if (schedule != null)
        {
            foreach (ClassRunning classRunning in schedule.ClassRunnings)
            {
                if (classRunning.ClassAttendances.Count > 0)
                    return false;
            }
        }

        return true;
    }

    public void CopyScheduleFromLastMonth(int branchID, int year, int month)
    {
        DateTime lastPeriod = new DateTime(year, month, 1).AddMonths(-1);

        var schedules =
            ctx.ClassScheduleDetails.Where(
                sch => sch.BranchID == branchID && sch.Year == lastPeriod.Year && sch.Month == lastPeriod.Month);

        ctx.ClassScheduleDetails.DeleteAllOnSubmit(
            ctx.ClassScheduleDetails.Where(sch => sch.BranchID == branchID && sch.Year == year && sch.Month == month));
        ctx.SubmitChanges();

        foreach (var schedule in schedules)
        {
            var newSchedule = new ClassScheduleDetail();
            newSchedule.InstructorID = schedule.InstructorID;
            newSchedule.Level = schedule.Level;
            newSchedule.ClassRoomID = schedule.ClassRoomID;
            newSchedule.ClassID = schedule.ClassID;
            newSchedule.DayOfWeek = schedule.DayOfWeek;
            newSchedule.TimeStart = schedule.TimeStart;
            newSchedule.TimeEnd = schedule.TimeEnd;
            newSchedule.BranchID = schedule.BranchID;            
            newSchedule.Year = year;
            newSchedule.Month = month;
            newSchedule.IsActive = schedule.IsActive;
            EntityHelper.SetAuditFieldForInsert(newSchedule, HttpContext.Current.User.Identity.Name);
            ctx.ClassScheduleDetails.InsertOnSubmit(newSchedule);
        }
        ctx.SubmitChanges();
    }
}