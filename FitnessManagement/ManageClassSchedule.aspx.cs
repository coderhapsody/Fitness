using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.Providers;
using System.IO;

public partial class ManageClassSchedule : System.Web.UI.Page
{
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();
    InstructorProvider instructorProvider = UnityContainerHelper.Container.Resolve<InstructorProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead1);

            DataBindingHelper.PopulateActiveBranches(ddlBranch, User.Identity.Name, false);
            DynamicControlBinding.BindDropDown(ddlMonth, CommonHelper.GetMonthNames(), "Value", "Key", false);
            DynamicControlBinding.BindDropDown(ddlDayOfWeek, CommonHelper.GetDayNames(), "Value", "Key", false);
            DynamicControlBinding.BindDropDown(ddlInstructor, instructorProvider.GetActiveInstructors(), "Name", "ID", false);            
            for (int year = DateTime.Today.Year - 1; year <= DateTime.Today.Year; year++)
                ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));

            ddlYear.SelectedValue = DateTime.Today.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            ddlDayOfWeek.SelectedValue = DateTime.Today.DayOfWeek.ToString();

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            classProvider.AddSchedule(
                Convert.ToInt32(ddlDayOfWeek.SelectedValue),
                Convert.ToInt32(ddlBranch.SelectedValue),
                Convert.ToInt32(ddlYear.SelectedValue),
                Convert.ToInt32(ddlMonth.SelectedValue),
                Convert.ToInt32(ddlClass.SelectedValue),
                ddlLevel.SelectedValue,
                Convert.ToInt32(ddlClassRoom.SelectedValue),
                ddlTimeStart.SelectedItem.Text.Split('-')[0],
                ddlTimeStart.SelectedItem.Text.Split('-')[1],
                Convert.ToInt32(ddlInstructor.SelectedValue));
            gvwDetail.DataBind();
            ddlTimeStart.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "_error", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btnSelectOtherPeriod_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwRead1);
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwAddEdit);
        lblBranch.Text = ddlBranch.SelectedItem.Text;
        lblPeriod.Text = ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;
        ddlDayOfWeek.SelectedValue = Convert.ToString((int)DateTime.Today.DayOfWeek == 0 ? 7 : (int)DateTime.Today.DayOfWeek);
        DynamicControlBinding.BindDropDown(ddlClass, classProvider.GetAllActiveClasses().ToList(), "Name", "ID", false);
        DynamicControlBinding.BindDropDown(ddlClassRoom, classProvider.GetActiveClassRooms(Convert.ToInt32(ddlBranch.SelectedValue)).ToList(), "Name", "ID", false);
        ddlTimeStart.DataSource = classProvider.GetTimeSlots(Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlDayOfWeek.SelectedValue));
        ddlTimeStart.DataBind();
        
        Form.DefaultButton = btnSave.UniqueID;
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
        e.Command.Parameters["@Year"].Value = ddlYear.SelectedValue;
        e.Command.Parameters["@Month"].Value = ddlMonth.SelectedValue;
        e.Command.Parameters["@DayOfWeek"].Value = ddlDayOfWeek.SelectedValue;
    }
    protected void btnRefreshDayOfWeek_Click(object sender, EventArgs e)
    {
        gvwDetail.DataBind();
    }
    protected void gvwDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.HideGridViewRowId(0, e);
    }
    protected void gvwDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (classProvider.VerifyScheduleDeletion(id))
            {
                classProvider.DeleteSchedule(id);
                gvwDetail.DataBind();
            }
            else
            {
                WebFormHelper.SetLabelTextWithCssClass(
                    lblStatusDelete,
                    "Cannot delete schedule because it has attendees, please clear all of the attendees first to delete this schedule.",
                    LabelStyleNames.ErrorMessage);
            }
        }
        else if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            mvwForm.ActiveViewIndex = 2;            
            DynamicControlBinding.BindDropDown(ddlInstructor0, instructorProvider.GetActiveInstructors(), "Name", "ID", false);
            DynamicControlBinding.BindDropDown(ddlClass0, classProvider.GetAllActiveClasses().ToList(), "Name", "ID", false);
            DynamicControlBinding.BindDropDown(ddlClassRoom0, classProvider.GetActiveClassRooms(Convert.ToInt32(ddlBranch.SelectedValue)).ToList(), "Name", "ID", false);
            ddlTimeStart0.DataSource = classProvider.GetTimeSlots(Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlDayOfWeek.SelectedValue));
            ddlTimeStart0.DataBind();

            ClassScheduleDetail schedule = classProvider.GetSchedule(id);
            if (schedule != null)
            {
                lblBranch0.Text = ddlBranch.SelectedItem.Text;
                lblPeriod0.Text = ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;
                lblDayOfWeek0.Text = ddlDayOfWeek.SelectedItem.Text;                    
                ddlClass0.SelectedValue = schedule.ClassID.ToString();
                ddlClassRoom0.SelectedValue = schedule.ClassRoomID.ToString();
                ddlInstructor0.SelectedValue = schedule.InstructorID.ToString();
                ddlTimeStart0.Items.FindByText(schedule.TimeStart + "-" + schedule.TimeEnd).Selected = true;
                ddlLevel0.SelectedValue = schedule.Level;
                ViewState["ScheduleID"] = id;
            }
            Form.DefaultButton = btnUpdate.UniqueID;

        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        FileInfo fi = new FileInfo(fupFile.FileName);
        if(fi.Extension.ToUpper() != ".XLS")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "_error", "alert('Not excel file ');", true);
            return;
        }
        fupFile.SaveAs(Server.MapPath("~/Temp/") + fupFile.FileName);
        try
        {
            classProvider.UploadFromExcel(
                Convert.ToInt32(ddlBranch.SelectedValue),
                Convert.ToInt32(ddlYear.SelectedValue),
                Convert.ToInt32(ddlMonth.SelectedValue),
                Server.MapPath("~/Temp/") + fupFile.FileName);
            ClientScript.RegisterStartupScript(this.GetType(), "_error", "alert('Schedules uploaded successfully');", true);
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblUploadError, ex.Message, LabelStyleNames.ErrorMessage);            
        }
    }
    protected void ddlDayOfWeek_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlTimeStart.DataSource = classProvider.GetTimeSlots(Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlDayOfWeek.SelectedValue));
        ddlTimeStart.DataBind();
    }
    protected void btnCancelUpdate_Click(object sender, EventArgs e)
    {

        mvwForm.ActiveViewIndex = 1;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(ViewState["ScheduleID"]);
            classProvider.UpdateSchedule(id,
                Convert.ToInt32(ddlClass0.SelectedValue),
                Convert.ToInt32(ddlClassRoom0.SelectedValue),
                Convert.ToInt32(ddlInstructor0.SelectedValue),
                ddlLevel0.SelectedValue);
            mvwForm.ActiveViewIndex = 1;
            gvwDetail.DataBind();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblStatusUpdate,
                ex.Message,
                LabelStyleNames.ErrorMessage);
        }
    }

    protected void btnCopyFromLastMonth_Click(object sender, EventArgs e)
    {
        DateTime currentPeriod = new DateTime(
            Convert.ToInt32(ddlYear.SelectedValue),
            Convert.ToInt32(ddlMonth.SelectedValue),
            1);

        classProvider.CopyScheduleFromLastMonth(
            Convert.ToInt32(ddlBranch.SelectedValue),
            currentPeriod.Year,
            currentPeriod.Month);

        gvwDetail.DataBind();
    }
}

public class ClassScheduleViewModel
{
    public string Code { get; set; }
    public string Subject { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}