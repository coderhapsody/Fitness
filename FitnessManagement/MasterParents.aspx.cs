using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using System.IO;
using System.Configuration;

public partial class MasterParents : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion


    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();
    PersonProvider personProvider = UnityContainerHelper.Container.Resolve<PersonProvider>();
    CustomerStatusProvider customerStatusProvider = UnityContainerHelper.Container.Resolve<CustomerStatusProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwParent);

        if (!this.IsPostBack)
        {
            mvwParents.ActiveViewIndex = 0;
            GetCustomerInfo(Request.QueryString["CustomerCode"]);
        }
    }

    private void GetCustomerInfo(string customerCode)
    {
        Customer cust = customerProvider.Get(customerCode);
        if (cust != null)
        {
            CustomerStatusHistory customerStatusHistory = customerStatusProvider.GetLatestStatus(customerCode);

            lblBarcode.Text = cust.Barcode;
            lblName.Text = String.Format("{0} {1}", cust.FirstName, cust.LastName);
//            lblStatus.Text = customerStatusHistory == null ? "OK" : customerStatusHistory.CustomerStatus.Description;
            lblDateOfBirth.Text = cust.DateOfBirth.HasValue ? cust.DateOfBirth.Value.ToString("dddd, dd MMMM yyyy") : String.Empty;
        }
    }

    protected void sdsParents_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@CustomerBarcode"].Value = Request.QueryString["CustomerCode"];
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
                fupPhoto.SaveAs(Server.MapPath(ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoPersons]) + @"\" + fileName);
            }
            else
                fileName = Convert.ToString(ViewState["Photo"]);

            switch (RowID)
            {
                case 0:
                    if (personProvider.FatherIsExist(Request.QueryString["CustomerCode"]) && ddlConnection.SelectedValue == "F")
                    {
                        WebFormHelper.SetLabelTextWithCssClass(
                            lblStatus,
                            "Information for father is already exist",
                            LabelStyleNames.ErrorMessage);
                        return;
                    }

                    if (personProvider.MotherIsExist(Request.QueryString["CustomerCode"]) && ddlConnection.SelectedValue == "M")
                    {
                        WebFormHelper.SetLabelTextWithCssClass(
                            lblStatus,
                            "Information for mother is already exist",
                            LabelStyleNames.ErrorMessage);
                        return;
                    }

                    personProvider.Add(
                        Request.QueryString["CustomerCode"],
                        ddlConnection.SelectedValue,
                        txtName.Text,
                        chkUnknownBirthDate.Checked ? (DateTime?)null : calDateOfBirth.SelectedDate,
                        txtIDCardNo.Text,
                        txtEmail.Text,
                        txtPhone1.Text,
                        txtPhone2.Text,
                        fileName);
                    break;
                default:
                    personProvider.Update(
                        RowID,
                        Request.QueryString["CustomerCode"],
                        ddlConnection.SelectedValue,
                        txtName.Text,
                        chkUnknownBirthDate.Checked ? (DateTime?)null : calDateOfBirth.SelectedDate,
                        txtIDCardNo.Text,
                        txtEmail.Text,
                        txtPhone1.Text,
                        txtPhone2.Text,
                        fileName);

                    break;
            }
            mvwParents.ActiveViewIndex = 0;
            gvwParent.DataBind();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(
                lblStatus,
                ex.Message,
                LabelStyleNames.ErrorMessage);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwParents.ActiveViewIndex = 0;
        gvwParent.DataBind();
    }
    protected void gvwParent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            RowID = id;
            mvwParents.ActiveViewIndex = 1;
            Person person = personProvider.Get(id);
            ddlConnection.SelectedValue = person.Connection;
            ddlConnection.Enabled = false;
            txtEmail.Text = person.Email;
            txtName.Text = person.Name;
            txtPhone1.Text = person.Phone1;
            txtPhone2.Text = person.Phone2;
            txtIDCardNo.Text = person.IDCardNo;
            calDateOfBirth.SelectedDate = person.BirthDate.HasValue ? person.BirthDate.Value : DateTime.Today;
            chkUnknownBirthDate.Checked = !person.BirthDate.HasValue;

            if (!String.IsNullOrEmpty(person.Photo))
            {
                FileInfo file = new FileInfo(person.Photo);
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoPersons] + @"\" + file.Name.Substring(0, file.Name.IndexOf(".")) + file.Extension + ".ashx?w=200";
            }
            else
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoPersons] + @"\default.png";
             chkDeletePhoto.Checked = false;

             if (Request.QueryString["PickUpPerson"] == null)
                 btnSave.Enabled = !String.IsNullOrEmpty(Request.QueryString["CustomerCode"]);
             else
                 if (ddlConnection.SelectedItem.Text.ToUpper() != "PICK UP PERSON")
                     btnSave.Enabled = false;
                 else
                     btnSave.Enabled = true;

            ddlConnection.Focus();
        }
    }
    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        mvwParents.ActiveViewIndex = 1;
        RowID = 0;
        imgPhoto.ImageUrl = ConfigurationManager.AppSettings[ApplicationSettingKeys.FolderPhotoPersons] + @"\default.png";
        WebFormHelper.ClearTextBox(txtEmail, txtName, txtPhone1, txtPhone2);
        ddlConnection.Enabled = String.IsNullOrEmpty(Request.QueryString["PickUpPerson"]);
        ddlConnection.SelectedIndex = String.IsNullOrEmpty(Request.QueryString["PickUpPerson"]) ? 0 : 3;        
        ddlConnection.Focus();
    }
    protected void gvwParent_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }
    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int[] selectedID = WebFormHelper.GetRowIdForDeletion(gvwParent);
            personProvider.Delete(selectedID);
            gvwParent.DataBind();
            btnCancel_Click(sender,e );
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }
    protected void gvwParent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["PickUpPerson"]))
            {
                if (e.Row.Cells[1].Text.ToUpper() != "PICK UP PERSON")
                    (e.Row.Cells[e.Row.Cells.Count - 1].Controls[1] as CheckBox).Enabled = false;
            }
        }
    }
}