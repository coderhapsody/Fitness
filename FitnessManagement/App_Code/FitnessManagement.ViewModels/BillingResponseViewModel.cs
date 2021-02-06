using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BillingResponseViewModel
/// </summary>
namespace FitnessManagement.ViewModels
{
    public class BillingRejectionViewModel : BillingViewModel
    {
        public string DeclineCode { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class BillingAcceptedViewModel : BillingViewModel
    {
        public string VerificationCode { get; set; }
        public string InvoiceNo { get; set; }
    }
}