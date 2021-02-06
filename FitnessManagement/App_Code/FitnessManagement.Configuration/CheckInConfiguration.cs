using FitnessManagement.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ApplicationConfiguration
/// </summary>
namespace FitnessManagement.Configuration
{
    public class CheckInConfiguration
    {
        public string ReportAgreementForm
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<string>("Report.AgreementForm"); }
            set { ConfigurationSingletonProvider.Instance.SetValue("Report.AgreementForm", value); }
        }

        public bool ContractNotActiveAlert
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>("CheckIn.ContractNotActiveAlert"); }
            set { ConfigurationSingletonProvider.Instance.SetValue("CheckIn.ContractNotActiveAlert", value); }
        }

        public bool ContractNotPaid
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>("CheckIn.ContractNotPaid"); }
            set { ConfigurationSingletonProvider.Instance.SetValue("CheckIn.ContractNotPaid", value); }
        }

        public bool BirthdayAlert
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>("CheckIn.BirthdayAlert"); }
            set { ConfigurationSingletonProvider.Instance.SetValue("CheckIn.BirthdayAlert", value); }
        }

        public bool CreditCardExpiringAlert
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>("CheckIn.CreditCardExpiringAlert"); }
            set { ConfigurationSingletonProvider.Instance.SetValue("CheckIn.CreditCardExpiringAlert", value); }
        }

        public bool CreditCardExpired
        {
            get { return ConfigurationSingletonProvider.Instance.GetValue<bool>("CheckIn.CreditCardExpired"); }
            set { ConfigurationSingletonProvider.Instance.SetValue("CheckIn.CreditCardExpired", value); }
        }
    }
}