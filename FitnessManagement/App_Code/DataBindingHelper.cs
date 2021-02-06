using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using FitnessManagement.Providers;
using Microsoft.Practices.Unity;
using Telerik.Web.UI;

/// <summary>
/// Summary description for DataBindingHelper
/// </summary>
public static class DataBindingHelper
{
    public static void PopulateActiveBranches(DropDownList dropdown, bool addEmptyOption = false)
    {
        BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
        dropdown.DataSource = branchProvider.GetActiveBranches();
        dropdown.DataTextField = "Name";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }

    

    public static void PopulateActiveBranches(DropDownList dropdown, string userName, bool addEmptyOption = false)
    {
        BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
        dropdown.DataSource = branchProvider.GetActiveBranches(userName);
        dropdown.DataTextField = "Name";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }

    public static void PopulateActiveBranches(RadDropDownList dropdown, string userName, bool addEmptyOption = false)
    {
        BranchProvider branchProvider = UnityContainerHelper.Container.Resolve<BranchProvider>();
        dropdown.DataSource = branchProvider.GetActiveBranches(userName);
        dropdown.DataTextField = "Name";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, new DropDownListItem(String.Empty, String.Empty));
    }

    public static void PopulatePaidClasses(DropDownList dropdown, bool addEmptyOption = false)
    {
        ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();
        dropdown.DataSource = classProvider.GetAllClasses().Where(cls => cls.IsPaid);
        dropdown.DataTextField = "Name";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }


    public static void PopulateAllClasses(DropDownList dropdown, bool addEmptyOption = false)
    {
        ClassProvider classProvider = UnityContainerHelper.Container.Resolve<ClassProvider>();
        dropdown.DataSource = classProvider.GetAllClasses();
        dropdown.DataTextField = "Name";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }


    public static void PopulateBillingTypes(DropDownList dropdown, bool addEmptyOption = false)
    {
        BillingTypeProvider provider = UnityContainerHelper.Container.Resolve<BillingTypeProvider>();
        dropdown.DataSource = provider.GetActiveBillingTypes();
        dropdown.DataTextField = "Description";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }

    public static void PopulateCreditCardTypes(DropDownList dropdown, bool addEmptyOption = false)
    {
        CreditCardTypeProvider provider = UnityContainerHelper.Container.Resolve<CreditCardTypeProvider>();
        dropdown.DataSource = provider.GetAll();
        dropdown.DataTextField = "Description";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }


    public static void PopulatePackages(DropDownList dropdown, bool addEmptyOption = false)
    {
        PackageProvider provider = UnityContainerHelper.Container.Resolve<PackageProvider>();
        dropdown.DataSource = provider.GetAll();
        dropdown.DataTextField = "Name";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }

    public static void PopulateCustomerStatus(DropDownList dropdown, bool addEmptyOption = false)
    {
        CustomerStatusProvider provider = UnityContainerHelper.Container.Resolve<CustomerStatusProvider>();
        dropdown.DataSource = provider.GetAll();
        dropdown.DataTextField = "Description";
        dropdown.DataValueField = "ID";
        dropdown.DataBind();

        if (addEmptyOption)
            dropdown.Items.Insert(0, String.Empty);
    }

}