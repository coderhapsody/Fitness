using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewCustomerStatusHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
            var dataRow = e.Row.DataItem as DataRowView;
            if (dataRow != null)
            {
                if (Convert.ToBoolean(dataRow["Void"]))
                    e.Row.BackColor = Color.OrangeRed;
            }
        }
    }
}