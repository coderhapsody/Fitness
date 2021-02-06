using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for TemplateTextProvider
/// </summary>
public class TemplateTextProvider
{
    private FitnessDataContext ctx;

	public TemplateTextProvider(FitnessDataContext ctx)
	{
        this.ctx = ctx;
	}

    public string GetTermsConditionsText()
    {
        return File.ReadAllText(HttpContext.Current.Server.MapPath("~/TemplateText/") + "TermsConditions.txt");
    }

    public string GetPresigningNotice()
    {
        return File.ReadAllText(HttpContext.Current.Server.MapPath("~/TemplateText/") + "PresigningNotice.txt");
    }

    public string GetReceiptFooterText()
    {
        return File.ReadAllText(HttpContext.Current.Server.MapPath("~/TemplateText/") + "ReceiptFooterText.txt");
    }

    public void UpdateTermsConditions(string text)
    {
        File.WriteAllText(HttpContext.Current.Server.MapPath("~/TemplateText/") + "TermsConditions.txt", text);
    }

    public void UpdatePresigningNotice(string text)
    {
        File.WriteAllText(HttpContext.Current.Server.MapPath("~/TemplateText/") + "PresigningNotice.txt", text);
    }

    public void UpdateReceiptFooterText(string text)
    {
        File.WriteAllText(HttpContext.Current.Server.MapPath("~/TemplateText/") + "ReceiptFooterText.txt", text);
    }
}