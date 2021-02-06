using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;
using System.IO;
using FitnessManagement.Configuration;
using Microsoft.Practices.Unity;

/// <summary>
/// Summary description for Customer
/// </summary>
namespace FitnessManagement.Providers
{

    public class CustomerProvider
    {
        private FitnessDataContext ctx;
        public CustomerProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void GetMinMaxCustomerJoinYear(out int minYear, out int maxYear)
        {
            minYear = ctx.Customers.Min(cust => cust.CreatedWhen).Year;
            maxYear = ctx.Customers.Max(cust => cust.CreatedWhen).Year;
        }

        public void AddCreditCardChangeHistory(int customerID, int creditCardTypeID, string creditCardNo, string creditCardHolderName, string creditCardIDNo, DateTime creditCardExpireDate)
        {
            CreditCardChangeHistory ccChangeHistory = new CreditCardChangeHistory();
            ccChangeHistory.CustomerID = customerID;
            ccChangeHistory.CreditCardTypeID = creditCardTypeID;
            ccChangeHistory.CreditCardNo = creditCardNo;
            ccChangeHistory.CreditCardHolderName = creditCardHolderName;
            ccChangeHistory.CreditCardExpiredDate = creditCardExpireDate;
            ccChangeHistory.CreditCardIDNo = creditCardIDNo;
            EntityHelper.SetAuditFieldForUpdate(ccChangeHistory, HttpContext.Current.User.Identity.Name);
            ctx.CreditCardChangeHistories.InsertOnSubmit(ccChangeHistory);
            ctx.SubmitChanges();
        }

        public void DeleteCreditCardChangeHistory(int creditCardChangeHistoryID)
        {
            ctx.CreditCardChangeHistories.DeleteOnSubmit(
                ctx.CreditCardChangeHistories.Single(cc => cc.ID == creditCardChangeHistoryID));
            ctx.SubmitChanges();
        }

        //public void Add(
        //    string barcode,
        //    string firstName,
        //    string lastName,
        //    string address,
        //    string zipCode,
        //    string email,
        //    string phone,
        //    int homeBranchID,
        //    int activeContractID,
        //    int areaID,
        //    int partnerID,
        //    int personID,
        //    string cardNo,
        //    int creditCardTypeID,
        //    string photo,
        //    bool isActive)            
        //{
        //    Customer cust = new Customer();
        //    cust.Barcode = barcode;
        //    cust.FirstName = firstName;
        //    cust.LastName = lastName;
        //    cust.Address = address;
        //    cust.ZipCode = zipCode;
        //    cust.Email = email;
        //    cust.Phone = phone;
        //    cust.HomeBranchID = homeBranchID;
        //    cust.ActiveContractID = activeContractID;
        //    cust.AreaID = areaID;
        //    cust.PartnerID = partnerID;
        //    cust.PersonID = personID;
        //    cust.CardNo = cardNo;
        //    cust.CreditCardTypeID = creditCardTypeID;
        //    cust.Photo = photo;
        //    cust.IsActive = isActive;
        //    EntityHelper.SetAuditFieldForInsert(cust, HttpContext.Current.User.Identity.Name);
        //    ctx.Customers.InsertOnSubmit(cust);
        //    ctx.SubmitChanges();
        //}

        public IEnumerable<Customer> GetCustomersByBillingType(int billingTypeID)
        {
            return ctx.Customers.Where(cust => cust.BillingTypeID == billingTypeID);
        }


        public void Update(
            int id,
            string barcode,
            string firstName,
            string lastName,
            string surname,
            DateTime dateOfBirth,
            string address,
            string zipCode,
            string mailingAddress,
            string mailingZipCode,
            string email,
            string phone,            
            string cellphone,
            int areaID,
            int schoolID,
            int partnerID,        
            int billingTypeID,
            string cardNo,
            int creditCardTypeID,
            string cardHolderName,
            string cardHolderID,
            int bankID,
            DateTime cardExpiredDate,
            bool deletePhoto,
            string photo)            
        {
            Customer cust = ctx.Customers.Single(row => row.ID == id);
            cust.Barcode = barcode;
            cust.FirstName = firstName;
            cust.LastName = lastName;
            cust.Surname = surname;
            cust.DateOfBirth = dateOfBirth;
            cust.Address = address;
            cust.ZipCode = zipCode;
            cust.MailingAddress = mailingAddress;
            cust.MailingZipCode = mailingZipCode;
            cust.Email = email;
            cust.HomePhone = phone;
            cust.CellPhone1 = cellphone;
            cust.AreaID = areaID == 0 ? (int?)null : areaID;
            cust.SchoolID = schoolID == 0 ? (int?)null : schoolID;
            cust.PartnerID = partnerID == 0 ? (int?)null : partnerID;
            cust.BillingTypeID = billingTypeID;
            if (billingTypeID != 1) // manual payment
            {
                cust.CardNo = cardNo;
                cust.CardHolderName = cardHolderName;
                cust.CardHolderID = cardHolderID;
                cust.CreditCardTypeID = creditCardTypeID;
                cust.BankID = bankID == 0 ? (int?)null : bankID;
                cust.ExpiredDate = cardExpiredDate;
            }
            else
            {
                cust.CardNo = null;
                cust.CardHolderName = null;
                cust.CardHolderID = null;
                cust.CreditCardTypeID = null;
                cust.BankID = bankID == 0 ? (int?)null : bankID;
                cust.ExpiredDate = (DateTime?)null;
            }
            cust.Photo = deletePhoto ? null : photo;
            EntityHelper.SetAuditFieldForUpdate(cust, HttpContext.Current.User.Identity.Name);            
            ctx.SubmitChanges();
        }

        public void Delete(int[] customersID)
        {
            ctx.Customers.DeleteAllOnSubmit(
                ctx.Customers.Where(row => customersID.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public bool IsExist(string barcode)
        {
            return ctx.Customers.Count(cust => cust.Barcode == barcode) > 0;
        }

        public Customer Get(int id)
        {
            return ctx.Customers.SingleOrDefault(row => row.ID == id);
        }

        public Customer Get(string barcode)
        {
            return ctx.Customers.SingleOrDefault(row => row.Barcode == barcode);
        }

        public void UpdatePhoto(int id, string fileName)
        {
            Customer cust = ctx.Customers.Single(row => row.ID == id);
            cust.Photo = fileName;            
            EntityHelper.SetAuditFieldForUpdate(cust, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void DeletePhoto(int id)
        {
            Customer cust = ctx.Customers.Single(row => row.ID == id);
            cust.Photo = null;
            EntityHelper.SetAuditFieldForUpdate(cust, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public IEnumerable<CustomerCheckInViewModel> GetCheckInHistory(int branchID)
        {
            return ctx.CheckInLogs
                .Where(ck => ck.CheckInWhen.Date == DateTime.Today)
                .OrderByDescending(ck => ck.CheckInWhen)
                .Take(10)
                .Select(ck => new CustomerCheckInViewModel
            {
                CustomerBarcode = ck.Customer.Barcode,
                CustomerName = String.Format("{0} {1}", ck.Customer.FirstName.Trim(), ck.Customer.LastName.Trim()),
                When = ck.CheckInWhen,
                AllowCheckIn = ck.Allowed,
                Messages = ck.Messages.Split('|').ToList()
            });
        }

        public CustomerCheckInViewModel DoCheckIn(int branchID, string customerBarcode, string userName)
        {
            CheckInConfiguration checkInConfiguration = new CheckInConfiguration();

            List<string> messages = new List<string>();
            CustomerCheckInViewModel viewModel = new CustomerCheckInViewModel();
            viewModel.PickUpPersons = new List<string>();
            viewModel.PickUpPhotos = new List<string>();

            CustomerStatusProvider customerStatusProvider = new CustomerStatusProvider(ctx);

            Customer customer = ctx.Customers.SingleOrDefault(cust => cust.Barcode == customerBarcode);
            if (customer != null)
            {
                viewModel.CustomerBarcode = customerBarcode.ToUpper();
                viewModel.CustomerName = String.Format("{0} {1}", customer.FirstName.Trim().ToUpper(), customer.LastName.Trim().ToUpper());
                viewModel.When = DateTime.Now;
                viewModel.Photo = customer.Photo;
                viewModel.Age = customer.DateOfBirth.GetValueOrDefault().ToAgeString();
                viewModel.IsPhotoExist = File.Exists(HttpContext.Current.Server.MapPath("~/Photo/Customers/") + customer.Photo);
                viewModel.AllowCheckIn = true;

                foreach (var person in customer.Persons.Where(p => p.Connection == "P"))
                {
                    viewModel.PickUpPersons.Add(person.Name);
                    viewModel.PickUpPhotos.Add(person.Photo);
                }

                /* Get Messages */

                ContractProvider contractProvider = UnityContainerHelper.Container.Resolve<ContractProvider>();
                var activeContract = contractProvider.GetActiveContracts(customerBarcode).Where(contract => contract.EffectiveDate <= DateTime.Today).FirstOrDefault();
                if (activeContract != null)
                {
                    if (!activeContract.PackageHeader.OpenEnd)
                    {
                        if (activeContract.ExpiredDate < DateTime.Today)
                            messages.Add("CONTRACT " + activeContract.ContractNo + " EXPIRED");
                        else if (activeContract.ExpiredDate.Subtract(DateTime.Today) <= TimeSpan.FromDays(30))
                            messages.Add("CONTRACT " + activeContract.ContractNo + " EXPIRING");
                    }
                    viewModel.PackageName = activeContract.PackageHeader.Name;                    
                }
                else
                {
                    messages.Add("MEMBER DOES NOT HAVE ANY ACTIVE CONTRACT");
                }

                if (checkInConfiguration.ContractNotActiveAlert)
                {
                    var inactiveContracts = customer.Contracts.Where(contract => !contract.ActiveDate.HasValue && !contract.VoidDate.HasValue && contract.EffectiveDate <= DateTime.Today);
                    if (inactiveContracts.Count() > 0)
                        messages.Add("CONTRACT NOT ACTIVE: " + String.Join(", ", inactiveContracts.Select(contract => contract.ContractNo).ToArray()));
                }

                if (checkInConfiguration.ContractNotPaid)
                {
                    var unpaidContracts = customer.Contracts.Where(contract => !contract.PurchaseDate.HasValue && !contract.VoidDate.HasValue && contract.EffectiveDate <= DateTime.Today);
                    if (unpaidContracts.Count() > 0)
                        messages.Add("CONTRACT NOT PAID: " + String.Join(", ", unpaidContracts.Select(contract => contract.ContractNo).ToArray()));
                }

                if (checkInConfiguration.BirthdayAlert)
                {
                    bool isBirthDay = customer.DateOfBirth.GetValueOrDefault().Month == DateTime.Today.Month &&
                                      customer.DateOfBirth.GetValueOrDefault().Day == DateTime.Today.Day;
                    if (isBirthDay)
                        messages.Add("HAPPY BIRTHDAY");
                }

                if (customer.BillingType.ID > 1)
                {
                    // alert for credit card is valid only for non-manual payment
                    if (DateTime.Today >= customer.ExpiredDate.GetValueOrDefault() && checkInConfiguration.CreditCardExpired)
                        messages.Add("CREDIT CARD IS EXPIRED");
                    else
                    {
                        if (checkInConfiguration.CreditCardExpiringAlert)
                        {
                            bool isCreditCardExpiring = customer.ExpiredDate.GetValueOrDefault().Subtract(DateTime.Today) <= TimeSpan.FromDays(30);
                            if (isCreditCardExpiring)
                                messages.Add("CREDIT CARD IS EXPIRING");
                        }
                    }
                }

                CustomerStatusHistory customerStatusHistory = customerStatusProvider.GetLatestStatus(customerBarcode);
                if (customerStatusHistory == null)
                {
                    viewModel.CustomerStatus = "OK";
                }
                else
                {
                    viewModel.CustomerStatus = customerStatusHistory.CustomerStatus.Description;
                }
                string color = customerStatusProvider.GetStatusColor(viewModel.CustomerStatus);
                viewModel.CustomerStatusColor = color.Split('|')[0];
                viewModel.CustomerStatusBackgroundColor = color.Split('|')[1];

                foreach (string customerNote in customer.CustomerNotes
                                                    .Where(note => note.Priority == 1)
                                                    .Select(note => note.Notes))
                    messages.Add(customerNote);

                viewModel.Messages = messages;


                /* Save checkin history */
                CheckInLog checkinlog = new CheckInLog();
                checkinlog.BranchID = branchID;
                checkinlog.CustomerID = customer.ID;

                checkinlog.CustomerStatusID = customerStatusHistory == null ? 1 : customerStatusHistory.CustomerStatusID;
                checkinlog.Employee = ctx.Employees.SingleOrDefault(emp => emp.UserName == userName);
                checkinlog.CheckInWhen = viewModel.When.Value;
                checkinlog.Messages = String.Join("|", messages.ToArray());
                checkinlog.Allowed = viewModel.AllowCheckIn;
                ctx.CheckInLogs.InsertOnSubmit(checkinlog);
                ctx.SubmitChanges();
            }
            else
            {
                viewModel.AllowCheckIn = false;
                messages.Add("INVALID CUSTOMER BARCODE");
                viewModel.Messages = messages;
            }

            return viewModel;
        }

        public void UpdateCreditCardInfo(string barcode, int creditCardTypeID, int bankID, string cardHolderName, string cardHolderIDNo, string creditCardNo, DateTime expiredDate, string reason)
        {
            Customer customer = ctx.Customers.SingleOrDefault(c => c.Barcode == barcode && c.BillingTypeID != 1);
            if (customer != null)
            {
                CreditCardChangeHistory lastCC = ctx.CreditCardChangeHistories.Where(cch => cch.CustomerID == customer.ID).OrderByDescending(cch => cch.ChangedWhen).Take(1).SingleOrDefault();
                if (lastCC != null)
                {
                    if (lastCC.CreditCardTypeID == creditCardTypeID &&
                       lastCC.BankID == bankID &&
                       lastCC.CreditCardHolderName == cardHolderName &&
                       lastCC.CreditCardIDNo == cardHolderIDNo &&
                       lastCC.CreditCardExpiredDate == expiredDate &&
                       lastCC.CreditCardNo == creditCardNo)
                    {
                        throw new Exception("Cannot found any change information since last information of credit card was saved.");
                    }
                }

                CreditCardChangeHistory cc = new CreditCardChangeHistory();
                cc.Customer = customer;
                cc.CreditCardTypeID = creditCardTypeID;
                cc.BankID = bankID;
                cc.CreditCardHolderName = cardHolderName;
                cc.CreditCardIDNo = cardHolderIDNo;
                cc.CreditCardNo = creditCardNo;
                cc.CreditCardExpiredDate = expiredDate;
                cc.Reason = reason;
                cc.ChangedWhen = DateTime.Now;
                cc.ChangedWho = HttpContext.Current.User.Identity.Name;
                ctx.CreditCardChangeHistories.InsertOnSubmit(cc);

                customer.BankID = bankID;
                customer.CreditCardTypeID = creditCardTypeID;
                customer.CardHolderID = cardHolderIDNo;
                customer.CardHolderName = cardHolderName;
                customer.CardNo = creditCardNo;
                customer.ExpiredDate = expiredDate;
                
                ctx.SubmitChanges();
            }
        }

        public bool IsBillingTypeAutoPayment(string custBarcode)
        {
            Customer customer = ctx.Customers.SingleOrDefault(c => c.Barcode == custBarcode);
            if (customer != null)
            {
                return customer.BillingTypeID == 3;
            }

            return false;
        }
    }
}