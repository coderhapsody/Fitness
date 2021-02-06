using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using AjaxControlToolkit;
using System.Collections.Specialized;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;

/// <summary>
/// Summary description for AjaxWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AjaxWebService : System.Web.Services.WebService {

    public AjaxWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent();         
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public CascadingDropDownNameValue[] GetItemTypes
        (string knownCategoryValues,
        string category)
    {
        List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
        ItemTypeProvider itemTypeProvider = UnityContainerHelper.Container.Resolve<ItemTypeProvider>();
        foreach (var itemType in itemTypeProvider.GetAll())
            list.Add(
                new CascadingDropDownNameValue(
                    itemType.Description,
                    itemType.ID.ToString()));
        return list.ToArray();
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public CascadingDropDownNameValue[] GetItemsByType
        (string knownCategoryValues,
        string category)
    {
        StringDictionary categoryValues = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

        int itemTypeID = Convert.ToInt32(categoryValues["ItemType"]);
        ItemProvider itemProvider = UnityContainerHelper.Container.Resolve<ItemProvider>();
        List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
        foreach (var item in itemProvider.GetItemsByType(itemTypeID))
            list.Add(
                new CascadingDropDownNameValue(
                    item.Description,
                    item.ID.ToString()));
        return list.ToArray();
    }


    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public CascadingDropDownNameValue[] GetPaymentTypes
        (string knownCategoryValues,
        string category)
    {
        List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
        PaymentTypeProvider paymentTypeProvider = UnityContainerHelper.Container.Resolve<PaymentTypeProvider>();
        foreach (var paymentType in paymentTypeProvider.GetAll())
            list.Add(
                new CascadingDropDownNameValue(
                    paymentType.Description,
                    paymentType.ID.ToString()));
        return list.ToArray();
    }


    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public CascadingDropDownNameValue[] GetCreditCardTypesByPaymentType
        (string knownCategoryValues,
        string category)
    {
        StringDictionary categoryValues = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

        int paymentTypeID = Convert.ToInt32(categoryValues["PaymentType"]);
        CreditCardTypeProvider creditCardTypeProvider = UnityContainerHelper.Container.Resolve<CreditCardTypeProvider>();
        List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
        if (paymentTypeID == 4) //Credit Card
        {
            foreach (var item in creditCardTypeProvider.GetAll())
                list.Add(
                    new CascadingDropDownNameValue(
                        item.Description,
                        item.ID.ToString()));
        }
        return list.ToArray();
    }
}
