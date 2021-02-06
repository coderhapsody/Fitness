using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;

/// <summary>
/// Summary description for MonthlyClosingProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class MonthlyClosingProvider
    {
        private FitnessDataContext ctx;

        public MonthlyClosingProvider(FitnessDataContext context)
        {
            this.ctx = context; 
        }

        public bool IsClosed(int branchID, int month, int year)
        {
            return ctx.MonthlyClosings.Count(closing => closing.BranchID == branchID &&
                                                        closing.ClosingMonth == month &&
                                                        closing.ClosingYear == year &&
                                                        closing.IsClosed) > 0;
        }

        public IEnumerable<int> GetClosingYears()
        {
            return ctx.MonthlyClosings.Select(closing => closing.ClosingYear).ToList().Union(Enumerable.Range(DateTime.Today.Year, 1)).Distinct();
        }

        public IEnumerable<MonthlyClosingViewModel> GetAllClosing(int branchID, int year)
        {
            var query = from closing in ctx.MonthlyClosings
                        join branch in ctx.Branches on closing.BranchID equals branch.ID
                        where closing.BranchID == branchID
                           && closing.ClosingYear == year
                           && closing.IsClosed
                        select new MonthlyClosingViewModel
                        {
                            BranchID = branch.ID,
                            BranchName = branch.Name,
                            ClosingMonthName = CommonHelper.GetMonthNames()[closing.ClosingMonth],
                            ClosingYear = closing.ClosingYear,
                            ChangedWhen = closing.ChangedWhen,
                            ChangedWho = closing.ChangedWho
                        };
            return query.AsEnumerable();
        }

        public void DoMonthlyClosing(int branchID, int month, int year)
        {
            MonthlyClosing monthlyClosing = ctx.MonthlyClosings.SingleOrDefault(
                closing => closing.BranchID == branchID &&
                closing.ClosingMonth == month &&
                closing.ClosingYear == year);

            if (monthlyClosing == null)
            {
                monthlyClosing = new MonthlyClosing();
                monthlyClosing.BranchID = branchID;
                monthlyClosing.ClosingMonth = month;
                monthlyClosing.ClosingYear = year;
                monthlyClosing.IsClosed = true;
                EntityHelper.SetAuditFieldForInsert(monthlyClosing, HttpContext.Current.User.Identity.Name);
                ctx.MonthlyClosings.InsertOnSubmit(monthlyClosing);
            }
            else
            {
                monthlyClosing.IsClosed = true;
                EntityHelper.SetAuditFieldForUpdate(monthlyClosing, HttpContext.Current.User.Identity.Name);
            }

            MonthlyClosingHistory closingHist = new MonthlyClosingHistory();
            closingHist.BranchID = branchID;
            closingHist.IsClosing = true;
            closingHist.UserName = HttpContext.Current.User.Identity.Name;
            closingHist.CreatedWhen = DateTime.Now;

            ctx.SubmitChanges();
        }


        public void UndoMonthlyClosing(int branchID, int month, int year, string reason)
        {
            MonthlyClosing monthlyClosing = ctx.MonthlyClosings.SingleOrDefault(
                closing => closing.BranchID == branchID &&
                closing.ClosingMonth == month &&
                closing.ClosingYear == year);

            if (monthlyClosing != null)
            {
                monthlyClosing.IsClosed = false;
                EntityHelper.SetAuditFieldForInsert(monthlyClosing, HttpContext.Current.User.Identity.Name);                
            }

            MonthlyClosingHistory closingHist = new MonthlyClosingHistory();
            closingHist.BranchID = branchID;
            closingHist.IsClosing = false;
            closingHist.Notes = reason;
            closingHist.UserName = HttpContext.Current.User.Identity.Name;
            closingHist.CreatedWhen = DateTime.Now;

            ctx.SubmitChanges();
        }


    }
}