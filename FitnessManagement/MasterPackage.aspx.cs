using Catalyst.Extension;
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

public partial class MasterPackage : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }


    public List<PackageDetailViewModel> Detail
    {
        get { return ViewState["Detail"] as List<PackageDetailViewModel>; }
        set { ViewState["Detail"] = value; }
    }

    #endregion

    ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();
    PackageProvider packageProvider = UnityContainerHelper.Container.Resolve<PackageProvider>();
    ItemProvider itemProvider = UnityContainerHelper.Container.Resolve<ItemProvider>();
    UserProvider userProvider = UnityContainerHelper.Container.Resolve<UserProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetGridViewPageSize(gvwMaster);
            FillDropDown();
        }
    }

    private void FillDropDown()
    {
        int[] branchesID = userProvider.GetCurrentActiveBranches(User.Identity.Name).Select(branch => branch.ID).ToArray();
        ddlItem.Items.Add(String.Empty);
        foreach (var item in itemProvider.GetAll(branchesID))
            ddlItem.Items.Add(new ListItem(String.Format("{0} - {1}", item.Barcode, item.Description), item.ID.ToString()));
        ddlItem.SelectedIndex = 0;
    }

    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }

    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwAddEdit);
        RowID = 0;
        WebFormHelper.ClearTextBox(txtName, txtQuantity, txtUnitPrice);
        ddlItem.SelectedIndex = 0;
        Detail = new List<PackageDetailViewModel>();
        RefreshDetail();
        chkIsActive.Checked = true;
        txtName.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int[] id = WebFormHelper.GetRowIdForDeletion(gvwMaster);
            packageProvider.Delete(id);
            Refresh();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvwForm.SetActiveView(viwRead);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            this.Validate("AddEdit");
            if (this.IsValid)
            {
                if (gvwDetail.Rows.Count > 0)
                {
                    switch (RowID)
                    {
                        case 0:
                            packageProvider.Add(
                                txtName.Text,
                                Convert.ToInt32(ddlDuesInMonth.SelectedValue),
                                chkIsActive.Checked,
                                chkOpenEnd.Checked, 
                                Convert.ToDecimal(txtFreezeFee.Text),
                                Detail);
                            break;
                        default:
                            packageProvider.Update(
                                RowID,
                                txtName.Text,
                                Convert.ToInt32(ddlDuesInMonth.SelectedValue),
                                chkIsActive.Checked,
                                chkOpenEnd.Checked,
                                Convert.ToDecimal(txtFreezeFee.Text),
                                Detail);
                            break;
                    }
                    Refresh();
                }
                else
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblMessageDetail,
                        "Detail of package must have one or more items",
                        LabelStyleNames.ErrorMessage);
                }

            }
        }
        catch (Exception ex)
        {
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
        try
        {
            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit);
                PackageHeader package = packageProvider.Get(id);
                ddlDuesInMonth.SelectedValue = package.PackageDuesInMonth.ToString();
                txtName.Text = package.Name;
                chkIsActive.Checked = package.IsActive;
                Detail = packageProvider.GetDetail(id).ToList();
                chkOpenEnd.Checked = package.OpenEnd;
                txtFreezeFee.Text = Convert.ToString(package.FreezeFee);
                RefreshDetail();
                txtName.Focus();
            }
            else if (e.CommandName == "DefineClass")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                RowID = id;
                mvwForm.SetActiveView(viwAddEdit2);
                RefreshActiveClassInPackages();
                
                
            }
        }
        catch (Exception ex)
        {
            mvwForm.SetActiveView(viwRead);
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    private void RefreshActiveClassInPackages()
    {
        lblPackageName.Text = packageProvider.Get(RowID).Name;

        var classes = classProvider.GetAllActiveClasses();
        cblClass.DataSource = classes;
        cblClass.DataValueField = "ID";
        cblClass.DataTextField = "Name";
        cblClass.DataBind();

        var activeClasses = packageProvider.PopulateClassForSelectedPackage(RowID);
        foreach (var cls in activeClasses)
        {
            cblClass.Items.FindByValue(cls.ID.ToString()).Selected = true;
        }
    }

    private void RefreshDetail()
    {
        gvwDetail.DataSource = Detail;
        gvwDetail.DataBind();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwMaster.DataBind();
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@Name"].Value = txtFindName.Text;
    }
    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        int id = Detail.Any() ? Detail.Max(row => row.ID) + 1 : 1;
        Item selectedItem = itemProvider.Get(Convert.ToInt32(ddlItem.SelectedValue));
        Detail.Add(
            new PackageDetailViewModel()
            {
                ID = id,
                ItemBarcode = selectedItem.Barcode,
                ItemDescription = selectedItem.Description,
                ItemID = selectedItem.ID,
                Quantity = Convert.ToInt32(txtQuantity.Text),
                UnitPrice = Convert.ToDecimal(txtUnitPrice.Text)
            });
        WebFormHelper.ClearTextBox(txtQuantity, txtUnitPrice);
        ddlItem.SelectedIndex = 0;
        gvwDetail.DataSource = Detail;
        gvwDetail.DataBind();
    }
    protected void gvwDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "DeleteRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Detail.RemoveAll(package => package.ID == id);
            gvwDetail.DataSource = Detail;
            gvwDetail.DataBind();
        }
    }
    protected void gvwDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }
    protected void gvwClassDays_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
        }
    }
    protected void btnSaveClassPackage_Click(object sender, EventArgs e)
    {
        List<int> classesID = cblClass.Items.Cast<ListItem>()
            .Where(item => item.Selected)
            .Select(item => item.Value).ToList().CastAs(k => Convert.ToInt32(k));
        packageProvider.UpdateClassPackage(RowID, classesID.ToList());
        mvwForm.SetActiveView(viwRead);
        
    }
    protected void sdsClassPackage_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@PackageID"].Value = RowID;
    }
    protected void btnCancelPackage_Click(object sender, EventArgs e)
    {
        mvwForm.ActiveViewIndex = 0;
    }
}