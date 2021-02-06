using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitnessManagement.ViewModels
{
    [Serializable]
    public class InvoiceDetailViewModel
    {
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public int ItemID { get; set; }
        public string ItemBarcode { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }        
        public bool IsTaxable { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Total { get; set; }
    }
}
