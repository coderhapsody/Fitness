using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using System.Configuration;
using System.IO;

public partial class MasterEmployee : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetGridViewPageSize(gvwMaster);
            FillDropDown();
        }
    }

    private void FillDropDown()
    {
        ddlHomeBranch.DataSource = branchProvider.GetActiveBranches();
        ddlHomeBranch.DataTextField = "Name";
        ddlHomeBranch.DataValueField = "ID";
        ddlHomeBranch.DataBind();
        ddlHomeBranch.Items.Insert(0, String.Empty);

        ddlFindHomeBranch.DataSource = branchProvider.GetActiveBranches();
        ddlFindHomeBranch.DataTextField = "Name";
        ddlFindHomeBranch.DataValueField = "ID";
        ddlFindHomeBranch.DataBind();
        ddlFindHomeBranch.Items.Insert(0, new ListItem("All Branches", "0"));
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwRead);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FileInfo fi = null;
            string fileName = null;

            if (fupPhoto.HasFile && !chkDeletePhoto.Checked)
            {
                fi = new FileInfo(fupPhoto.FileName);
                fileName = chkDeletePhoto.Checked ? null : Guid.NewGuid().ToString().ToUpper() + fi.Extension;
                fupPhoto.SaveAs(Server.MapPath(ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoEmployees]) + @"\" + fileName);
            }
            else
                fileName = Convert.ToString(ViewState["Photo"]);

            employeeProvider.Update(
                RowID,
                txtBarcode.Text,
                lblUserName.Text,
                Convert.ToInt32(ddlHomeBranch.SelectedValue),
                txtFirstName.Text,
                txtLastName.Text,
                txtPhone.Text,
                txtEmail.Text,
                chkDeletePhoto.Checked,
                fileName,
                chkIsActive.Checked,
                chkCanApproveDocument.Checked,
                chkCanEditActiveContract.Checked,
                chkCanReprint.Checked);
            Refresh();
        }
        catch (Exception ex)
        {
            mvwForm.ActiveViewIndex = 0;
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    private void Refresh()
    {
        mvwForm.SetActiveView(viwRead);
        gvwMaster.DataBind();
    }

    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            RowID = id;
            mvwForm.SetActiveView(viwAddEdit);
            Employee employee = employeeProvider.Get(id);
            lblUserName.Text = employee.UserName;
            txtBarcode.Text = employee.Barcode;
            txtFirstName.Text = employee.FirstName;
            txtLastName.Text = employee.LastName;
            txtPhone.Text = employee.Phone;
            txtEmail.Text = employee.Email;
            chkIsActive.Checked = employee.IsActive;
            ddlHomeBranch.SelectedValue = employee.HomeBranchID.ToString();
            chkCanApproveDocument.Checked = employee.CanApproveDocument;
            chkCanEditActiveContract.Checked = employee.CanEditActiveContract;
            chkCanReprint.Checked = employee.CanReprint;
            ViewState["Photo"] = employee.Photo;

            if (!String.IsNullOrEmpty(employee.Photo))
            {
                FileInfo file = new FileInfo(employee.Photo);
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoEmployees] + @"\" + file.Name.Substring(0, file.Name.IndexOf(".")) + file.Extension + ".ashx?w=200";
            }
            else
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoEmployees] + @"\default.png";
            chkDeletePhoto.Checked = false;
            txtBarcode.Focus();
        }
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
        e.Command.Parameters["@Name"].Value = txtFindName.Text;
        e.Command.Parameters["@HomeBranchID"].Value = Convert.ToInt32(ddlFindHomeBranch.SelectedValue);
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
}
