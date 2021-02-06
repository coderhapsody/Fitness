using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for AutoNumberProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class AutoNumberProvider
    {
        private FitnessDataContext ctx;

        public AutoNumberProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Update(string formCode, int branchID, int year, int newLastNumber)
        {
            Create(formCode, branchID, year);

            var autoNumber = ctx.AutoNumbers.SingleOrDefault(row => row.FormCode == formCode && row.BranchID == branchID && row.Year == year);
            if (autoNumber != null)
            {
                autoNumber.LastNumber = newLastNumber;
                ctx.SubmitChanges();
            }
        }

        public IEnumerable<AutoNumber> GetByBranch(int branchID)
        {
            return ctx.AutoNumbers.Where(an => an.BranchID == branchID);
        }

        public string GetDescription(string formCode, int branchID)
        {
            return ctx.AutoNumbers.Any(row => row.FormCode == formCode && row.BranchID == branchID) ? ctx.AutoNumbers.SingleOrDefault(row => row.FormCode == formCode && row.BranchID == branchID).Description :
                   String.Empty;
        }

        private void Create(string formCode, int branchID, int year)
        {
            using (var ctx = new FitnessDataContext())
            {
                AutoNumber autoNumber = ctx.AutoNumbers.SingleOrDefault(row => row.FormCode == formCode && row.BranchID == branchID && row.Year == year);
                if (autoNumber == null)
                {
                    autoNumber = new AutoNumber();
                    autoNumber.BranchID = branchID;
                    autoNumber.FormCode = formCode;
                    autoNumber.Year = year;
                    autoNumber.LastNumber = 0;
                    ctx.AutoNumbers.InsertOnSubmit(autoNumber);
                    ctx.SubmitChanges();
                }
            }
        }

        public int GetLastNumber(string formCode, int branchID, int year)
        {
            Create(formCode, branchID, year);
            return ctx.AutoNumbers.SingleOrDefault(row => row.FormCode == formCode && row.BranchID == branchID && row.Year == year).LastNumber;
        }

        public void Increment(string formCode, int branchID, int year)
        {
            var autoNumber = ctx.AutoNumbers.Single(row => row.FormCode == formCode && row.BranchID == branchID && row.Year == year);
            autoNumber.LastNumber++;
            //ctx.SubmitChanges();
        }

        public string Generate(int branchID, string formCode, int month, int year)
        {
            string result = String.Empty;
            int currentNumber = GetLastNumber(formCode, branchID, year);
            string branchCode;

            using (var ctx = new FitnessDataContext())
                branchCode = ctx.Branches.SingleOrDefault(row => row.ID == branchID).Code;

            currentNumber++;

            if (formCode == "CO")
            {
                // contract
                result = String.Format("{0}-{1}-{2}",
                    branchCode.ToUpper(),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("00000"));
            }
            else if (formCode == "CU")
            {
                result = String.Format("{0}{1}",
                    branchCode.ToUpper(),
                    currentNumber.ToString("000000"));
            }
            else if (formCode == "OR")
            {
                result = String.Format("{0}-{1}{2}-{3}",
                    branchCode.ToUpper(),
                    month.ToString("00"),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("00000"));
            }
            else if (formCode == "BL")
            {
                result = String.Format("{0}-{1}{2}-{3}",
                    branchCode.ToUpper(),
                    month.ToString("00"),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("00000"));
            }
            else
            {
                result = String.Format("{0}/{1}{2}{3}/{4}",
                    formCode,
                    branchCode.ToUpper(),
                    month.ToString("00"),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("000"));
            }

            return result;
        }
    }
}