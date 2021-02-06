using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitnessManagement.ViewModels
{
    [Serializable]
    public class PaymentDetailViewModel
    {
        public int ID { get; set; }
        public int PaymentID { get; set; }
        public int? CreditCardTypeID { get; set; }
        public string CreditCardType { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string ApprovalCode { get; set; }
        public string Notes { get; set; }
    }
}