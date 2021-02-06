using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using Catalyst.Extension;
using System.IO;
using System.Configuration;

public partial class MasterPickUpPerson : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();
    CustomerStatusProvider customerStatusProvider = UnityContainerHelper.Container.Resolve<CustomerStatusProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    AreaProvider areaProvider = UnityContainerHelper.Container.Resolve<AreaProvider>();
    ContractProvider contractProvider = UnityContainerHelper.Container.Resolve<ContractProvider>();
    CreditCardTypeProvider creditCardTypeProvider = UnityContainerHelper.Container.Resolve<CreditCardTypeProvider>();
    BankProvider bankProvider = UnityContainerHelper.Container.Resolve<BankProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, gvwMaster);

        if (!this.IsPostBack)
        {
            mvwForm.SetActiveView(viwRead);
            FillDropDown();
        }

    }

    private void FillDropDown()
    {
        ddlFindBranch.DataSource = branchProvider.GetActiveBranches();
        ddlFindBranch.DataTextField = "Name";
        ddlFindBranch.DataValueField = "ID";
        ddlFindBranch.DataBind();

        
    }

    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@BranchID"].Value = ddlFindBranch.SelectedValue;
        e.Command.Parameters["@Barcode"].Value = txtFindBarcode.Text;
        e.Command.Parameters["@Name"].Value = txtFindName.Text;
    }
    protected void gvwMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            RowID = id;
            
        }
    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);
    }
    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string customerCode = e.Row.Cells[1].Text;
            
            (e.Row.FindControl("hypPerson") as System.Web.UI.HtmlControls.HtmlAnchor).Attributes["onclick"] =
                String.Format("window.open('MasterParents.aspx?PickUpPerson=1&CustomerCode={0}', 'Parent', 'width=800,height=500,location=no,resizable=yes')", customerCode);                
        }
    }
}