using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using System.Web.Security;

public partial class EditTemplateText : System.Web.UI.Page
{
    TemplateTextProvider templateProvider = UnityContainerHelper.Container.Resolve<TemplateTextProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtTermsConditions.Text = templateProvider.GetTermsConditionsText();
            txtPresigningNotice.Text = templateProvider.GetPresigningNotice();
            txtReceiptFooterText.Text = templateProvider.GetReceiptFooterText();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            templateProvider.UpdatePresigningNotice(txtPresigningNotice.Text);
            templateProvider.UpdateTermsConditions(txtTermsConditions.Text);
            templateProvider.UpdateReceiptFooterText(txtReceiptFooterText.Text);
            Response.Redirect(FormsAuthentication.DefaultUrl);
        }
        catch (Exception ex)
        {
            WebFormHelper.SetLabelTextWithCssClass(lblStatus, ex.Message, LabelStyleNames.ErrorMessage);                
        }
    }
}