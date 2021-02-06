using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BillingViewModel
/// </summary>
namespace FitnessManagement.ViewModels
{
    public class BillingViewModel
    {
        public string ContractNo { get; set; }
        public string MerchantCode { get; set; }
        public string CustomerBarcode { get; set; }
        public string CreditCardNo { get; set; }
        public DateTime CreditCardExpiredDate { get; set; }
        public decimal DuesAmount { get; set; }
        public string CreditCardName { get; set; }
        public string Note { get; set; }        
    }
}