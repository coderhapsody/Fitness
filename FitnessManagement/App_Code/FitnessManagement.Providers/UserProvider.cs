using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;

namespace FitnessManagement.Providers
{    
    public class UserProvider
    {
        FitnessDataContext ctx;

        public UserProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public IEnumerable<Branch> GetCurrentUserBranchID(string userName)
        {
            IQueryable<Branch> query = ctx.UserAtBranches.Where(user => user.UserName == userName).Select(row => row.Branch).Concat(ctx.Branches).Distinct();
            return query;
        }

        public IEnumerable<Branch> GetCurrentActiveBranches(string userName)
        {
            IQueryable<Branch> query = ctx.UserAtBranches.Where(user => user.UserName == userName).Select(row => row.Branch);
            return query;
        }

        public IEnumerable<Branch> GetSelectedBranches(string userName)
        {
            return ctx.UserAtBranches.Where(user => user.UserName == userName).Select(row => row.Branch);
        }        

        public void UpdateUserAtBranch(string userName, IEnumerable<int> branchesID)
        {
            ctx.UserAtBranches.DeleteAllOnSubmit(ctx.UserAtBranches.Where(row => row.UserName == userName));

            foreach (int id in branchesID)
                ctx.UserAtBranches.InsertOnSubmit(
                    new UserAtBranch() 
                    { 
                        BranchID = id, 
                        UserName = userName 
                    });

            ctx.SubmitChanges();
        }

        public void AddSuccessLogInHistory(string userName)
        {
            LogInHistory log = new LogInHistory();
            log.UserName = userName;
            log.LogInWhen = DateTime.Now;
            log.CanLogIn = true;
            ctx.LogInHistories.InsertOnSubmit(log);
            ctx.SubmitChanges();
        }

        public void AddFailedLogInHistory(string userName)
        {
            LogInHistory log = new LogInHistory();
            log.UserName = userName;
            log.LogInWhen = DateTime.Now;
            log.CanLogIn = false;
            ctx.LogInHistories.InsertOnSubmit(log);
            ctx.SubmitChanges();
        }

    }
}