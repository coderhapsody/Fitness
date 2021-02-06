using FitnessManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomerCheckInViewModel
/// </summary>
namespace FitnessManagement.ViewModels
{

    [System.Runtime.Serialization.DataContract]
    public class CustomerCheckInViewModel
    {
        [System.Runtime.Serialization.DataMember]
        public IList<string> Messages { get; set; }

        [System.Runtime.Serialization.DataMember]
        public IList<string> PickUpPersons { get; set; }

        [System.Runtime.Serialization.DataMember]
        public IList<string> PickUpPhotos { get; set; }

        [System.Runtime.Serialization.DataMember]
        public IList<string> Classes { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string CustomerBarcode { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Photo { get; set; }

        [System.Runtime.Serialization.DataMember]
        public bool IsPhotoExist { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string CustomerName { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string CustomerStatus { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string CustomerStatusColor { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string CustomerStatusBackgroundColor { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Age { get; set; }        

        [System.Runtime.Serialization.DataMember]
        public string PackageName { get; set; }

        [System.Runtime.Serialization.DataMember]
        public DateTime? When { get; set; }

        [System.Runtime.Serialization.DataMember]
        public bool AllowCheckIn { get; set; }
    }

    

}