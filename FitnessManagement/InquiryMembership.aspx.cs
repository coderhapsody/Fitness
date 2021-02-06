using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InquiryMembership : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        RadFilter1.FireApplyCommand();
        RadGrid1.MasterTableView.ExportToExcel();
    }
    protected void btnApplyFilter_Click(object sender, EventArgs e)
    {
        RadFilter1.FireApplyCommand();
    }
}