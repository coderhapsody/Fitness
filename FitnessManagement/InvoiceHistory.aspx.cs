using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;

public partial class InvoiceHistory : System.Web.UI.Page
{
    private InvoiceProvider invoiceProvider = UnityContainerHelper.Container.Resolve<InvoiceProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["_ReadOnly"] = Request["ReadOnly"];
    }
    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        bool isReadOnly = false;
        if(ViewState["_ReadOnly"] != null)
            isReadOnly = Convert.ToBoolean(ViewState["_ReadOnly"]);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hypPaymentStatus = e.Row.FindControl("hypPaymentStatus") as HyperLink;
            if (hypPaymentStatus != null && !isReadOnly)
            {
                if (hypPaymentStatus.Text.ToUpper() == "NOT PAID")
                {
                    e.Row.BackColor = Color.PaleVioletRed;
                    e.Row.Style.Add("cursor", "pointer");
                    e.Row.ToolTip = "Click here to create payment for invoice " + e.Row.Cells[0].Text;
                    e.Row.Attributes.Add("onclick",String.Format("window.open('PaymentManual.aspx?InvoiceNo={0}', 'payment', 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,resizable=yes,width=1100,height=600,scrollbars=yes,toolbar=no,status=no')", e.Row.Cells[0].Text));                    
                }                
            }

            string invoiceNo = e.Row.Cells[0].Text;
            var invoiceDetails = invoiceProvider.GetDetail(invoiceNo);
            if (invoiceDetails != null)
            {
                var info = new StringBuilder();
                info.AppendLine("Items for invoice " + invoiceNo + ":");
                info.AppendLine();
                foreach (var invoice in invoiceDetails)
                {
                    info.AppendLine(String.Format("{0} [{1}], Qty: {2}", invoice.ItemBarcode, invoice.ItemDescription,
                        invoice.Quantity));
                }
                e.Row.ToolTip = info.ToString();
            }
        }

    }
}