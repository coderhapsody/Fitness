using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catalyst.Extension;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.Providers;
using System.Data;

public partial class ManageClassRunning : System.Web.UI.Page
{
    ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();
    InstructorProvider instructorProvider = UnityContainerHelper.Container.Resolve<InstructorProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(View1);
            var branches = branchProvider.GetActiveBranches(User.Identity.Name);
            PopulateBranches(branches);
        }
    }

    private void PopulateBranches(IEnumerable<Branch> branches)
    {
        ddlBranch.DataSource = branches;
        ddlBranch.DataTextField = "Name";
        ddlBranch.DataValueField = "ID";
        ddlBranch.DataBind();

        calDate.SelectedDate = DateTime.Today;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        lblBranch.Text = ddlBranch.SelectedItem.Text;
        lblPeriode.Text = calDate.SelectedDate.ToLongDateString();
        mvwForm.SetActiveView(View2);
        gvwSchedule.DataBind();
    }
    protected void sdsSchedule_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        if (this.IsPostBack)
        {
            e.Command.Parameters["@BranchID"].Value = ddlBranch.SelectedValue;
            e.Command.Parameters["@Date"].Value = calDate.SelectedDate.Date;
        }
    }
    protected void gvwSchedule_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //DynamicControlBinding.HideGridViewRowId(0, e);
    }
    protected void gvwSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = e.Row.DataItem as DataRowView;
            if (row != null)
            {
                //if (Convert.IsDBNull(row["RunningStartWhen"]))
                //    (e.Row.FindControl("btnStartStop") as LinkButton).Text = "Start";
                //else
                //    (e.Row.FindControl("btnStartStop") as LinkButton).Text = "Stop";
                (e.Row.FindControl("btnStartStop") as LinkButton).OnClientClick = "return confirm('Are you sure want to " + (e.Row.FindControl("btnStartStop") as LinkButton).Text + " for " + 
                    e.Row.Cells[1].Text + " by " + e.Row.Cells[3].Text + " at " + e.Row.Cells[4].Text + " until " + e.Row.Cells[5].Text + " ?'); ";
            }
        
        }
    }
    protected void gvwSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandSource is LinkButton)
        {
            if (e.CommandName.Equals("StartStop"))
            {
                LinkButton startStopButton = e.CommandSource as LinkButton;
                if (startStopButton != null)
                {
                    if (startStopButton.Text == "Start")
                    {                        
                        mvwForm.SetActiveView(View4);
                        ClassRunning classRunning = classProvider.GetClassRunning(Convert.ToInt32(e.CommandArgument));
                        if (classRunning != null)
                        {
                            lblBranch4.Text = ddlBranch.SelectedItem.Text;
                            lblPeriod4.Text = calDate.SelectedDate.ToLongDateString();
                            lblClassName4.Text = classRunning.ClassScheduleDetail.Class.Name;
                            lblInstructor4.Text = classRunning.ClassScheduleDetail.Instructor.Name;
                            ViewState["ClassRunningID"] = Convert.ToInt32(e.CommandArgument);
                            DynamicControlBinding.BindDropDown(
                                ddlRunningInstructor,
                                instructorProvider.GetActiveInstructors(),
                                "Name",
                                "ID", true);

                            DynamicControlBinding.BindDropDown(
                                ddlRunningAssistant,
                                instructorProvider.GetActiveInstructors(),
                                "Name",
                                "ID", true);

                            ddlRunningInstructor.SelectedValue = classRunning.RunningInstructorID.HasValue
                                ? classRunning.RunningInstructorID.Value.ToString()
                                : classRunning.ClassScheduleDetail.InstructorID.ToString();

                            ddlRunningAssistant.SelectedValue = classRunning.RunningAssistantID.HasValue
                                ? classRunning.RunningAssistantID.Value.ToString()
                                : String.Empty;

                            txtNotes.Text = Convert.ToString(classRunning.Notes);

                            cblAttendances.DataBind();
                            LoadAttendancesStatus();
                        }                        
                    }
                    else
                    {
                        
                    }
                }
            }
            else if (e.CommandName.Equals("Participants"))
            {
                mvwForm.ActiveViewIndex = 2;          
                ClassRunning classRunning = classProvider.GetClassRunning(Convert.ToInt32(e.CommandArgument));
                if (classRunning != null)
                {
                    lblBranchName3.Text = ddlBranch.SelectedItem.Text;
                    lblPeriod3.Text = calDate.SelectedDate.ToLongDateString();
                    lblClassName3.Text = classRunning.ClassScheduleDetail.Class.Name;
                    lblInstructor3.Text = classRunning.ClassScheduleDetail.Instructor.Name;
                    ViewState["ClassRunningID"] = Convert.ToInt32(e.CommandArgument);
                }
                gvwData.DataBind();
                CountTotalParticipants();
            }
        }
    }
    protected void btnSelectAnotherBranchDate_Click(object sender, EventArgs e)
    {
        mvwForm.ActiveViewIndex = 0;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Customer customer = customerProvider.Get(txtBarcode.Text);
        if (customer != null)
        {
            try
            {
                classProvider.AddParticipant(
                    Convert.ToInt32(ViewState["ClassRunningID"]),
                    customer.ID);
                gvwData.DataBind();
                txtBarcode.Text = String.Empty;
                CountTotalParticipants();
            }
            catch(Exception ex)
            {
                WebFormHelper.SetLabelTextWithCssClass(lblMessage3, ex.Message, LabelStyleNames.ErrorMessage);
            }
        }
    }
    protected void gvwData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.HideGridViewRowId(0, e);
    }
    protected void sdsAttendance_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@ClassRunningID"].Value = Convert.ToInt32(ViewState["ClassRunningID"]);        
    }
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "_prompt",
            "showPromptPopUp('PromptClassRunning.aspx?BranchID=" + ddlBranch.SelectedValue + "&button=" + btnProcessCopy.ClientID + "', '" + hidClassRunningID.ClientID + "', 550, 900);", 
            true);
    }
    protected void btnProcessCopy_Click(object sender, EventArgs e)
    {
        int currentClassRunningID = Convert.ToInt32(ViewState["ClassRunningID"]);
        int fromClassRunningID = Convert.ToInt32(hidClassRunningID.Value);
        if(currentClassRunningID != fromClassRunningID)
        {
            classProvider.CopyAttendances(
                currentClassRunningID,
                fromClassRunningID);
            gvwData.DataBind();
            CountTotalParticipants();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "copy", "alert('Cannot copy from current class running participants');", true);
            CountTotalParticipants();
        }
    
    }

    private void CountTotalParticipants()
    {
        lblTotalParticipants.Text = String.Format("Total participants: {0}", gvwData.Rows.Count.ToString());
    }
    protected void gvwData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteParticipant")
        {
            int customerID = Convert.ToInt32(e.CommandArgument);
            classProvider.DeleteParticipant(
                Convert.ToInt32(ViewState["ClassRunningID"]),
                customerID);
            gvwData.DataBind();
            CountTotalParticipants();
        }
    }

    private void LoadAttendancesStatus()
    {
        foreach(ListItem item in cblAttendances.Items)
            item.Selected = false;

        var result = classProvider.GetAttendancesStatus(Convert.ToInt32(ViewState["ClassRunningID"]));
        foreach (var item in result)
        {
            if(item.Value)
                cblAttendances.Items.FindByValue(item.Key.ToString()).Selected = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            var values = cblAttendances.Items
                .Cast<ListItem>()
                .Where(item => item.Selected)
                .Select(item => item.Value).ToArray();

            var checkedCustomers = Array.ConvertAll<string, int>(values, i => Convert.ToInt32(i));

            classProvider.UpdateAttendancesStatus(
                Convert.ToInt32(ViewState["ClassRunningID"]),
                Convert.ToInt32(ddlRunningInstructor.SelectedValue),
                ddlRunningAssistant.SelectedValue.ToDefaultNumber<int>(),
                txtNotes.Text,
                checkedCustomers);            
            
            mvwForm.SetActiveView(View2);
            gvwSchedule.DataBind();
        }
        catch(Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btnBack3_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(View2);
        gvwSchedule.DataBind();
    }
}