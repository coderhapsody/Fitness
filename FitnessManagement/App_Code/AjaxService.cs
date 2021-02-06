using Catalyst.FormattedFiles.RSS;
using FitnessManagement.Providers;
using FitnessManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Practices.Unity;

[ServiceContract(Namespace = "CheckInService")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class AjaxService
{
    CustomerProvider customerProvider = UnityContainerHelper.Container.Resolve<CustomerProvider>();
    ItemProvider itemProvider = UnityContainerHelper.Container.Resolve<ItemProvider>();

	// To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
	// To create an operation that returns XML,
	//     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
	//     and include the following line in the operation body:
	//         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

    [OperationContract]
    public decimal GetStdUnitPrice(int itemID)
    {
        var item = itemProvider.Get(itemID);
        return item.UnitPrice;
    }



    [OperationContract]
    public CustomerCheckInViewModel DoCheckIn(int branchID, string customerBarcode, string userName)
    {
        CustomerCheckInViewModel cust = customerProvider.DoCheckIn(branchID, customerBarcode, userName);                
        return cust;
    }

    [OperationContract]
    public CustomerCheckInViewModel[] GetCheckInHistory(int branchID)
    {
        List<CustomerCheckInViewModel> list = customerProvider.GetCheckInHistory(branchID).ToList();
        return list.ToArray();
    }



	// Add more operations here and mark them with [OperationContract]
}
