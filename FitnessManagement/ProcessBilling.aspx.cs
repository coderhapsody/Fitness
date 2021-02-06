using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;
using FitnessManagement.Data;

public partial class ProcessBilling : System.Web.UI.Page
{
    PackageProvider packageProvider = UnityContainerHelper.Container.Resolve<PackageProvider>();
    CustomerStatusProvider customerStatusProvider = UnityContainerHelper.Container.Resolve<CustomerStatusProvider>();
    BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
    BillingProvider billingProvider = UnityContainerHelper.Container.Resolve<BillingProvider>();
    BillingTypeProvider billingTypeProvider = UnityContainerHelper.Container.Resolve<BillingTypeProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            cblMembershipType.DataSource = packageProvider.GetAll();
            cblMembershipType.DataTextField = "Name";
            cblMembershipType.DataValueField = "ID";
            cblMembershipType.DataBind();

            cblCustomerStatus.DataSource = customerStatusProvider.GetAll();
            cblCustomerStatus.DataTextField = "Description";
            cblCustomerStatus.DataValueField = "ID";
            cblCustomerStatus.DataBind();

            DynamicControlBinding.BindDropDown(ddlBillingType, billingTypeProvider.GetActiveBillingTypes().Where(bt => bt.ID > 1) , "Description", "ID", false);
            DynamicControlBinding.BindDropDown(ddlBranch, branchProvider.GetActiveBranches(), "Name", "ID", false);

            calFindDateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            calFindDateTo.SelectedDate = DateTime.Today;
            cblMembershipType.Items.Cast<ListItem>().ToList().ForEach(item => item.Selected = true);
        }
    }


    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        gvwBilling.DataSource=  billingProvider.GetBillingData(
            Convert.ToInt32(ddlBranch.SelectedValue),
            Convert.ToInt32(ddlBillingType.SelectedValue),
            cblCustomerStatus.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Convert.ToInt32(item.Value)).ToArray(),
            cblMembershipType.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Convert.ToInt32(item.Value)).ToArray(),
            calFindDateFrom.SelectedDate,
            calFindDateTo.SelectedDate).OrderBy(data => data.CustomerBarcode) ;
        gvwBilling.DataBind();
        foreach (GridViewRow gridRow in gvwBilling.Rows)
        {
            (gridRow.Cells[gridRow.Cells.Count - 1].Controls[1] as CheckBox).Checked = true;
        }
    }
    protected void btnProcessAll_Click(object sender, EventArgs e)
    {
        if (billingProvider.IsMerchantCodeValid(Convert.ToInt32(ddlBranch.SelectedValue)))
        {
            string[] contractNumbers = gvwBilling.Rows.Cast<GridViewRow>()
                                            .Where(row => (row.Cells[row.Cells.Count - 1].Controls[1] as CheckBox).Checked)
                                            .Select(row => row.Cells[4].Text).ToArray();
            billingProvider.DeleteBillingHistoryByProcessDate(DateTime.Today);
            string billingFile = billingProvider.ProcessBilling(
                Convert.ToInt32(ddlBranch.SelectedValue),
                Convert.ToInt32(ddlBillingType.SelectedValue),
                contractNumbers,
                DateTime.Today,
                User.Identity.Name,
                calFindDateFrom.SelectedDate,
                calFindDateTo.SelectedDate);
            ClientScript.RegisterStartupScript(this.GetType(), "billing", String.Format("alert('Billing file created {0}');", billingFile), true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "billing", String.Format("alert('Invalid Merchant Code, please go to master Branch menu to set it up.');"), true);
        }
    }
}