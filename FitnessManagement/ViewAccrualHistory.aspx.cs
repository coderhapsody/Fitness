using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewAccrualHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void gvwMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        WebFormHelper.HideGridViewRowId(e);
    }
    protected void gvwMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int id = Convert.ToInt32(e.Row.Cells[0].Text);
            var hypPrint = e.Row.FindControl("hypPrint") as HyperLink;

            if (hypPrint == null)
                return;

            hypPrint.Attributes["onclick"] = String.Format("showSimplePopUp('PrintPreview.aspx?RDL=SalesReceiptAccrual&InvoiceAccrualID={0}');", id);
        }
    }
}