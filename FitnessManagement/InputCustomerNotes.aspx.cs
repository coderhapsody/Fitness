using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using FitnessManagement.Data;
using Microsoft.Practices.Unity;

public partial class InputCustomerNotes : System.Web.UI.Page
{
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();
    CustomerNotesProvider customerNotesProvider = UnityContainerHelper.Container.Resolve<CustomerNotesProvider>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        tblNotes.Visible = String.IsNullOrEmpty(Request["mode"]);
        if (!this.IsPostBack)
        {                        
            string custBarcode = Request["barcode"];
            if (!String.IsNullOrEmpty(custBarcode))
            {
                Customer customer = customerProvider.Get(custBarcode);
                if (customer != null)
                {
                    litCustomerName.Text = String.Format("{0} {1} ({2})", customer.FirstName, customer.LastName, customer.Barcode);
                }
            }

            if (!String.IsNullOrEmpty(Request["mode"]))
                btnSave.Enabled = txtNotes.Enabled = false;
            else
                btnSave.Enabled = txtNotes.Enabled = true;


            lblDate.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }
        //litCustomerName.Text = 
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            customerNotesProvider.Add(
                Request["barcode"],
                txtNotes.Text,
                (short) (chkShowCheckIn.Checked ? 1 : 0));                
            txtNotes.Text = String.Empty;
            chkShowCheckIn.Checked = false;
            gvwNotes.DataBind();
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }
    protected void gvwNotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "DeleteNote")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            customerNotesProvider.Delete(id);
            gvwNotes.DataBind();
        }
        else if (e.CommandName == "Toggle")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            customerNotesProvider.ToggleNote(id);
            gvwNotes.DataBind();

        }
    }
    protected void gvwNotes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
        WebFormHelper.ChangeBackgroundColorRowOnHover(e);

        LinkButton lnbDelete = e.Row.FindControl("lnbDelete") as LinkButton;
        if (lnbDelete != null)
            lnbDelete.Visible = String.IsNullOrEmpty(Request["mode"]);
    }
}