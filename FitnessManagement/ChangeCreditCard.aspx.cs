using FitnessManagement.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;

public partial class ChangeCreditCard : System.Web.UI.Page
{
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();
    CreditCardTypeProvider creditCardTypeProvider = UnityContainerHelper.Container.Resolve<CreditCardTypeProvider>();
    BankProvider bankProvider = UnityContainerHelper.Container.Resolve<BankProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ddlCreditCardType.DataSource = creditCardTypeProvider.GetAll();
            ddlCreditCardType.DataTextField = "Description";
            ddlCreditCardType.DataValueField = "ID";
            ddlCreditCardType.DataBind();
            
            ddlBank.DataSource = bankProvider.GetActiveBanks();
            ddlBank.DataTextField = "Name";
            ddlBank.DataValueField = "ID";
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, String.Empty);

            var cust = customerProvider.Get(Request["barcode"]);
            lblCustomer.Text = String.Format("{0} - {1} {2}", cust.Barcode, cust.FirstName.Trim(), cust.LastName.Trim());
        }
    }
    protected void sdsMaster_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@CustomerBarcode"].Value = Request["barcode"];
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCreditCardNo.Text.Trim().Length == 16 && ValidationHelper.IsValidCreditCardNumber(txtCreditCardNo.Text.Trim()))
            {
                customerProvider.UpdateCreditCardInfo(
                    Request["barcode"],
                    Convert.ToInt32(ddlCreditCardType.SelectedValue),
                    Convert.ToInt32(ddlBank.SelectedValue),
                    txtCardHolderName.Text,
                    txtCardHolderIDNo.Text,
                    txtCreditCardNo.Text,
                    calExpireDate.SelectedDate,
                    txtReason.Text);

                ClientScript.RegisterStartupScript(this.GetType(), "_alert",
                    String.Format("alert('Credit card information for {0} has been updated.')", Request["barcode"]), true);

                DynamicControlBinding.ClearTextBox(txtCardHolderIDNo, txtCardHolderName, txtCreditCardNo, txtReason);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "_alert",
                    String.Format("alert('Credit card number is invalid')"), true);
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "_alert",
                String.Format("alert('{0}')", ex.Message), true);                
        }
        finally
        {
            gvwMaster.DataBind();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DynamicControlBinding.HideGridViewRowId(0, e);
    }

}