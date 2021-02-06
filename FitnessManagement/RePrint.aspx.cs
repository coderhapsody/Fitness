using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Providers;
using FitnessManagement.Data;
using System.Web.Security;

public partial class RePrint : System.Web.UI.Page
{
    InvoiceProvider invoiceProvider = UnityContainerHelper.Container.Resolve<InvoiceProvider>();
    EmployeeProvider employeeProvider = UnityContainerHelper.Container.Resolve<EmployeeProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnReprint_Click(object sender, EventArgs e)
    {
        InvoiceHeader invoice = invoiceProvider.GetInvoice(txtInvoiceNo.Text);
        if (invoice != null)
        {
            mopAuth.Show();
        }
        
        
    }
    protected void btnPopupOK_Click(object sender, EventArgs e)
    {
        if (Membership.ValidateUser(txtUserName.Text, txtPassword.Text) &&
            employeeProvider.CanReprintInvoice(txtUserName.Text))
        {
            InvoiceHeader invoice = invoiceProvider.GetInvoice(txtInvoiceNo.Text);

            mopAuth.Hide();
            switch (invoice.InvoiceType)
            {
                case "F":
                    Response.Redirect(String.Format("FreshMemberCompleted.aspx?InvoiceNo={0}", invoice.InvoiceNo));
                    break;

                case "X":
                    Response.Redirect(String.Format("ExistingMemberCompleted.aspx?InvoiceNo={0}", invoice.InvoiceNo));
                    break;
            }
        }
        else
        {
            mopAuth.Hide();
            WebFormHelper.SetLabelTextWithCssClass(lblStatus0,
                "Invalid user name/password, or user does not have permission to reprint invoice.",
                LabelStyleNames.ErrorMessage);

        }
    }
}